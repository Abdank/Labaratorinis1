using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public class MapsContainer
    {
        public const int MaxMaps = 50;
        private Maps[] Maps;
        public int Count { get; private set; }
        public MapsContainer(int size)
        {
            Maps = new Maps[size];
        }
        public void AddMap(Maps map)
        {
            Maps[Count++] = map;
        }
        public Maps GetMap(int index)
        {
            return Maps[index];
        }

    }

