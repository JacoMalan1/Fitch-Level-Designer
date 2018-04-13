using OpenTK;

namespace Fitch_Level_Designer
{

    public enum BlockType
    {

        Solid,
        Spike,
        Goal,
        Start,
        OneUp

    }

    class Block
    {

        private Vector2 pos;
        private BlockType type;
        private int size;

        public Vector2 Position { get { return pos; } set { pos = value; } }
        public BlockType Type { get { return type; } set { type = value; } }
        public int Size { get { return size; } set { size = value; } }

        public Block(Vector2 pos, BlockType type)
        {

            this.pos = pos;
            this.type = type;
            this.size = Editor.world.BlockSize;

        }

        public Block(BlockType type, Vector2 pos, int size)
        {
            this.pos = pos;
            this.type = type;
            this.size = size;
        }

    }
}
