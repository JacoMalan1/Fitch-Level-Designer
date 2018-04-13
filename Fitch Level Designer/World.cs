using System;
using System.IO;
using OpenTK;

namespace Fitch_Level_Designer
{
    class World
    {

        private Vector2 size;
        private int blockSize;

        public Vector2 Size { get { return size; } set { size = value; } }
        public int BlockSize { get { return blockSize; } set { blockSize = value; } }

        public World(Vector2 size, int blockSize)
        {

            this.blockSize = blockSize;
            this.size = size;

        }

        public static Block[,] LoadFromFile(World world, string filePath)
        {
            
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            string[] lines = File.ReadAllLines(filePath);

            Block[,] blocks = new Block[(int)world.Size.X, (int)world.Size.Y];

            for (int j = 0; j < (int)world.Size.X; j++)
            {
                for (int k = 0; k < (int)world.Size.Y; k++)
                {

                    blocks[j, k] = null;

                }
            }

            string type;
            string xCoord;
            string yCoord;
            int pos;
            string temp;

            foreach (string line in lines)
            {

                pos = line.IndexOf(',');
                type = line.Substring(0, pos);
                temp = line.Substring(pos + 1, line.Length - pos - 1);
                pos = temp.IndexOf(',');
                xCoord = temp.Substring(0, pos);
                yCoord = temp.Substring(pos + 1, temp.Length - pos - 1);

                if (line[0] == '#')
                    continue;

                if (type == "solid")
                {

                    int x = Int32.Parse(xCoord);
                    int y = Int32.Parse(yCoord);

                    blocks[x, y] = new Block(BlockType.Solid, new Vector2(x, y), world.blockSize);

                }

                else if (type == "spike")
                {
                    int x = Int32.Parse(xCoord);
                    int y = Int32.Parse(yCoord);

                    blocks[x, y] = new Block(BlockType.Spike, new Vector2(x, y), world.blockSize);
                }

                else if (type == "start")
                {

                    int x = Int32.Parse(xCoord);
                    int y = Int32.Parse(yCoord);

                    blocks[x, y] = new Block(BlockType.Start, new Vector2(x, y), world.blockSize);

                }

                else if (type == "goal")
                {

                    int x = Int32.Parse(xCoord);
                    int y = Int32.Parse(yCoord);

                    blocks[x, y] = new Block(BlockType.Goal, new Vector2(x, y), world.blockSize);

                }
                else if (type == "oneup")
                {

                    int x = Int32.Parse(xCoord);
                    int y = Int32.Parse(yCoord);

                    blocks[x, y] = new Block(BlockType.OneUp, new Vector2(x, y), world.blockSize);

                }

            }

            return blocks;

        }


    }
}
