using System;

using Kopernicus.ConfigParser.Attributes;
using Kopernicus.ConfigParser.Enumerations;
using Kopernicus.ConfigParser.Interfaces;
using Kopernicus.Components;
using UnityEngine;
using Kopernicus.Configuration.Parsing;

namespace Kopernicus.OnDemand

{
    public class MapSOTileSet
    {
        private string tileSetPath;
        private string tileSetExtension;
        private int tileSetLength;
        private int tileSetSize;

        MapSODemandLarge[] tileSet;
        


        public MapSOTileSet(string path, int size)
        {
            tileSetPath = path;
            tileSetSize = size;
            tileSetLength = (size * size * 6)-1;
            tileSetExtension = ".dds";//extension;
            tileSet = new MapSODemandLarge[tileSetLength];
            for (int index = 0; index < tileSetLength; index++)
            {
                int X = index % 16;
                int Y = (int)(Math.Floor((double)index / size) % size);
                int F = (int)(Math.Floor((double)index / size * size) % size * size);

                tileSet[index] = ScriptableObject.CreateInstance<MapSODemandLarge>();
                tileSet[index].Path = tileSetPath + "_" + F + "-" + Y + "-" + X + tileSetExtension;
                tileSet[index].AutoLoad = OnDemandStorage.OnDemandLoadOnMissing;
            }
        }




        public MapSO.HeightAlpha GetPixelHeightAlpha(int face, double x, double y)
        {
            int a = face * tileSetSize * tileSetSize;
            int b = (int)(Math.Floor(tileSetSize * y) * 16);
            int c = (int)(Math.Floor(tileSetSize * x));
            int tileIndex = a + b + c;

            double newX = (tileSetSize * x) - Math.Floor(tileSetSize * x);
            double newY = (tileSetSize * y) - Math.Floor(tileSetSize * y);

            tileSet[tileIndex].Load();
            return tileSet[tileIndex].GetPixelHeightAlpha(newX, newY);
        }
    }

    [RequireConfigType(ConfigType.Value)]
    public class MapSOTileSet<T> : BaseLoader, IParsable, ITypeParser<T> where T : MapSO
    {
        /// <summary>
        /// The value that is being parsed
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Parse the Value from a string
        /// </summary>
        public void SetFromString(String s)
        {
            Char[] split = {'.'};
            String[] splitString = s.Split(split);
            String path = splitString[0];
            int size = Convert.ToInt32(splitString[1]);
            MapSOTileSet tileSet = new MapSOTileSet(path, size);
        }

        public String ValueToString()
        {
            return Value.name;
        }
    }
}
