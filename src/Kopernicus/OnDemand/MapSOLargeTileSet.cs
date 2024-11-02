using System;
using UnityEngine;


namespace Kopernicus.OnDemand

{
    public class MapSOLargeTileSet
    {
        public string tileSetPath { get; private set; }
        public int tileSetSize { get; private set; }
        private string tileSetExtension;
        private int tileSetLength;


        MapSODemandLarge[] tileSet;
        
        public MapSOLargeTileSet(string path, int size)
        {

            /* TILE ORDER
             * 0 - Zn
             * 1 - Zp
             * 2 - Yn
             * 3 - Yp
             * 4 - Xn
             * 5 - Xp
             */
            Debug.Log(OnDemandStorage.UseOnDemand);

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

            int b = (int)(Math.Floor(tileSetSize * y) * tileSetSize);

            if (b == tileSetSize * tileSetSize)
                b -= tileSetSize;

            int c = (int)(Math.Floor(tileSetSize * x));
            if (c == tileSetSize)
                c -= 1;

            int tileIndex = a + b + c;

            return tileSet[tileIndex];
        }

        public MapSO.HeightAlpha GetPixelHeightAlpha(int face, double x, double y)
        {
            MapSODemandLarge tile = GetTile(face, x, y);

            double newX = (tileSetSize * x) - Math.Floor(tileSetSize * x);
            double newY = (tileSetSize * y) - Math.Floor(tileSetSize * y);

            tile.Load();
            return tile.GetPixelHeightAlpha(newX, newY);
        }
    }
}
