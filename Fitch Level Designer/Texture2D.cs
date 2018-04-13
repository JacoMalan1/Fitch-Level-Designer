using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitch_Level_Designer
{
    class Texture2D
    {

        private int id;
        private int width;
        private int height;

        public int ID { get { return id; } }
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }

        public Texture2D(int id, int width, int height)
        {

            this.id = id;
            this.width = width;
            this.height = height;

        }

    }
}
