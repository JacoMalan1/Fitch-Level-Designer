using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System;

namespace Fitch_Level_Designer
{
    class Editor
    {

        public GameWindow window;
        public static World world;
        public static Block[,] level;
        public View view;
        public int clickCount = 0;
        private Vector2 clickPos = Vector2.Zero;
        private Vector2 lastPos = Vector2.Zero;
        private int worldHeight;
        private int worldWidth;
        public static bool ABORT = false;

        public Editor(GameWindow window, int worldWidth, int worldHeight)
        {

            window.Load += Window_Load;
            window.Closing += Window_Closing;
            window.RenderFrame += Window_RenderFrame;
            window.UpdateFrame += Window_UpdateFrame;
            window.Resize += Window_Resize;

            this.window = window;
            this.worldHeight = worldHeight;
            this.worldWidth = worldWidth;
            this.window.Run();

        }

        private void Window_Resize(object sender, EventArgs e)
        {

            GL.Viewport(window.ClientRectangle);
            GL.Viewport(window.ClientRectangle);

            Matrix4 projMat = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projMat);

        }

        private void Window_Load(object sender, EventArgs e)
        {

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Texture2D);

            world = new World(new Vector2(250, 50), 50);
            level = new Block[(int)world.Size.X, (int)world.Size.Y];
            view = new View(Vector2.Zero);
            Input.Init(ref window);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {



        }

        private void Window_UpdateFrame(object sender, FrameEventArgs e)
        {

            if (ABORT)
                window.Close();

            try
            {
                if (Input.MousePress(OpenTK.Input.MouseButton.Left))
                {

                    int x = (int)Math.Floor((Input.mousePos.X + (int)view.Position.X) / world.BlockSize);
                    int y = (int)Math.Floor((Input.mousePos.Y + (int)view.Position.Y) / world.BlockSize);

                    BlockType type = frmLevelDesign.selectedBlock;

                    level[x, y] = new Block(new Vector2(x, y), type);

                }
            }
            catch
            {

            }

            try
            {
                if (Input.MousePress(OpenTK.Input.MouseButton.Right))
                {

                    int x = (int)Math.Floor((Input.mousePos.X + (int)view.Position.X) / world.BlockSize);
                    int y = (int)Math.Floor((Input.mousePos.Y + (int)view.Position.Y) / world.BlockSize);

                    level[x, y] = null;

                }
            }
            catch
            {

            }

            if (Input.MouseDown(OpenTK.Input.MouseButton.Right) && Input.KeyDown(OpenTK.Input.Key.ControlLeft))
            {

                if (clickCount == 0)
                    clickPos = Input.mousePos;
                else if (!(lastPos == Input.mousePos))
                {
                    
                    view.SetPosition(view.Position - (Input.mousePos - clickPos));
                    clickPos = Input.mousePos;

                }
                lastPos = Input.mousePos;

                clickCount++;

            }

            if (Input.MouseRelease(OpenTK.Input.MouseButton.Right))
                clickCount = 0;

            Input.Update(ref window);
            view.Update();

        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {

            Matrix4 projMat = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, 0, 1);

            GL.LoadMatrix(ref projMat);

            view.ApplyTransforms();

            GL.ClearColor(Color.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            foreach (Block block in level)
            {

                if (block == null)
                    continue;

                SpriteBatch.DrawBlock(block.Type, block.Position, world.BlockSize);

            }

            SpriteBatch.DrawGrid(world);

            window.SwapBuffers();

        }
    }
}
