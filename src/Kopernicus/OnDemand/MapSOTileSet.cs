using System;

using Kopernicus.ConfigParser.Attributes;
using Kopernicus.ConfigParser.Enumerations;
using Kopernicus.ConfigParser.Interfaces;
using Kopernicus.Components;
using UnityEngine;
using Kopernicus.Configuration.Parsing;
using Kopernicus.ConfigParser.BuiltinTypeParsers;

namespace Kopernicus.OnDemand

{
    public class MapSOTileSet
    {
        public string tileSetPath { get; private set; }
        public int tileSetSize { get; private set; }
        private string tileSetExtension;
        private int tileSetLength;


        MapSODemandLarge[] tileSet;
        
        public MapSOTileSet(string path, int size)
        {
            Debug.Log("Making a tileset with a path of " + path);
            tileSetPath = path;
            tileSetSize = size;
            tileSetLength = (size * size * 6);
            tileSetExtension = ".dds";//extension;
            tileSet = new MapSODemandLarge[tileSetLength];
            for (int index = 0; index < tileSetLength; index++)
            {
                int X = index % size;
                int Y = (int)(Math.Floor((double)index / size) % size);
                int F = (int)(Math.Floor((double)index / (size * size)));

                tileSet[index] = ScriptableObject.CreateInstance<MapSODemandLarge>();
                Debug.Log(tileSetPath + "_" + F + "-" + Y + "-" + X + tileSetExtension);
                tileSet[index].Path = tileSetPath + "_" + F + "-" + Y + "-" + X + tileSetExtension;
                tileSet[index].AutoLoad = OnDemandStorage.OnDemandLoadOnMissing;
            }
        }


        public MapSODemandLarge GetTile(int face, double x, double y)
        {
            int a = face * tileSetSize * tileSetSize;
            int b = (int)(Math.Floor(tileSetSize * y) * 16);
            int c = (int)(Math.Floor(tileSetSize * x));
            int tileIndex = a + b + c;
            return tileSet[tileIndex];
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
}
