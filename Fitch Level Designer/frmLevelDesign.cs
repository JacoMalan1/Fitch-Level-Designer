using OpenTK;
using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Microsoft.VisualBasic;

namespace Fitch_Level_Designer
{
    public partial class frmLevelDesign : Form
    {

        public static BlockType selectedBlock;
        public static Thread t;
        private int levelWidth;
        private int levelHeight;

        public frmLevelDesign()
        {
            InitializeComponent();
        }

        private void startEditor()
        {

            GameWindow window = new GameWindow(800, 600);
            Editor editor = new Editor(window, levelWidth, levelHeight);

        }

        private void frmLevelDesign_Load(object sender, EventArgs e)
        {

            this.FormClosing += FrmLevelDesign_FormClosing;

            levelHeight = 50;
            levelWidth = 200;

            t = new Thread(new ThreadStart(startEditor), 0);
            t.Start();

            foreach (BlockType b in Enum.GetValues(typeof(BlockType)))
            {

                cbxBlocks.Items.Add(b);

            }

        }

        private void FrmLevelDesign_FormClosing(object sender, FormClosingEventArgs e)
        {

            while (t.IsAlive)
                Editor.ABORT = true;

        }

        private void cbxBlocks_SelectedIndexChanged(object sender, EventArgs e)
        {

            BlockType type;

            switch (cbxBlocks.Text)
            {

                case "Solid":
                    type = BlockType.Solid;
                    break;
                case "Spike":
                    type = BlockType.Spike;
                    break;
                case "Goal":
                    type = BlockType.Goal;
                    break;
                case "OneUp":
                    type = BlockType.OneUp;
                    break;
                case "Start":
                    type = BlockType.Start;
                    break;
                default:
                    type = BlockType.Solid;
                    break;

            }

            selectedBlock = type;

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog d = new SaveFileDialog();
            d.AddExtension = true;
            d.Filter = "Fitch Level Files (*.fl)|*.fl";
            d.InitialDirectory = Directory.GetCurrentDirectory();
            d.ShowDialog();

            string filepath = d.FileName;

            if (filepath == "")
                return;

            if (File.Exists(filepath))
                File.Delete(filepath);

            File.AppendAllText(filepath, levelWidth.ToString() + "\n" + levelHeight.ToString());

            foreach (Block block in Editor.level)
            {

                if (block == null)
                    continue;

                string type = block.Type.ToString();
                string xCoord = block.Position.X.ToString();
                string yCoord = block.Position.Y.ToString();

                File.AppendAllText(filepath, "\n" + type.ToLower() + "," + xCoord + "," + yCoord);

            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog d = new OpenFileDialog();
            d.Multiselect = false;
            d.InitialDirectory = Directory.GetCurrentDirectory();
            d.Filter = "Fitch Level Files (*.fl)|*.fl";
            d.ShowDialog();
            string filename = d.FileName;

            if (filename == "")
                return;

            Editor.level = World.LoadFromFile(Editor.world, filename);

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string sWidth = Interaction.InputBox("Please input a level width.", "Level Width");
            string sHeight = Interaction.InputBox("Please input a level height.", "Level Height");

            int width, height;

            if (!Int32.TryParse(sWidth, out width) || !Int32.TryParse(sHeight, out height))
            {

                MessageBox.Show("Invalid input.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            
            while (t.IsAlive)
            {

                Editor.ABORT = true;

            }

            t = new Thread(new ThreadStart(startEditor));

            Editor.ABORT = false;

            levelHeight = height;
            levelWidth = width;

            t.Start();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            while (t.IsAlive)
                Editor.ABORT = true;

            this.Close();

        }
    }
}
