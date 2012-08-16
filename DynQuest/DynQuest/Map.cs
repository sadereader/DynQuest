using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace DynQuest
{
    class Map
    {
        public Tile[,] Board;
        public Map()
        {
            //Initialize the board
            Board = new Tile[100, 100];

            
        }

        public class Tile 
        {
            public Bitmap bitmap;
            public Dictionary<string,string> properties;

            public Tile()
            {
                bitmap = new Bitmap(50, 50, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                properties = new Dictionary<string, string>();
            }
            

        
        }
    }
}
