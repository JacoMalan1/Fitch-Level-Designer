using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Fitch_Level_Designer
{
    class SpriteBatch
    {

        public static Texture2D textureSolid = ContentPipe.LoadTexture("solid.png");
        public static Texture2D textureSpike = ContentPipe.LoadTexture("spike.png");
        public static Texture2D textureGoal = ContentPipe.LoadTexture("goal.png");
        public static Texture2D textureOU = ContentPipe.LoadTexture("oneUp.png");
        public static Texture2D textureStart = ContentPipe.LoadTexture("start.png");

        public static void DrawGrid(World world)
        {

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.Begin(PrimitiveType.Lines);
            GL.LineWidth(2);

            int y = 0;
            int x = 0;
            for (int i = 0;i <= world.Size.Y;i++)
            {

                y = i * world.BlockSize;
                x = 0;

                GL.Vertex2(x, y);

                x = (int)world.Size.X * world.BlockSize;

                GL.Vertex2(x, y);

            }

            for (int i = 0; i <= world.Size.X; i++)
            {

                x = i * world.BlockSize;
                y = 0;

                GL.Vertex2(x, y);

                y = (int)world.Size.Y * world.BlockSize;

                GL.Vertex2(x, y);

            }

            GL.End();

        }

        public static void DrawBlock(BlockType type, Vector2 position, float size)
        {
            GL.Color3(Color.White);
            Vector2[] vertices = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };

            int[] indices = new int[6]
            {
                0, 1, 3, 0, 2, 3
            };

            Texture2D texture;
            switch (type)
            {

                case BlockType.Solid:
                    texture = textureSolid;
                    break;
                case BlockType.Spike:
                    texture = textureSpike;
                    break;
                case BlockType.Goal:
                    texture = textureGoal;
                    break;
                case BlockType.OneUp:
                    texture = textureOU;
                    break;
                case BlockType.Start:
                    texture = textureStart;
                    break;
                default:
                    texture = textureSolid;
                    break;
            }

            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

            GL.Begin(PrimitiveType.Triangles);
            foreach (int i in indices)
            {

                GL.TexCoord2(vertices[i].X, vertices[i].Y);
                GL.Vertex2((vertices[i].X + position.X) * size, (vertices[i].Y + position.Y) * size);

            }
            GL.End();
        }

    }
}
