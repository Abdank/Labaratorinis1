using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public class Maps
    {
        public Maps(int[,] map, int xsize, int ysize)
        {

            Map = map;
            xSize = xsize;
            ySize = ysize;
        }

        public int[,] Map { get; set; }
        public int xSize { get; set; }
        public int ySize { get; set; }
    }
    

