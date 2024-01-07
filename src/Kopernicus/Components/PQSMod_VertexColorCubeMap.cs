﻿/* 
 * This code is adapted from KopernicusExpansion-Continued
 * Available from https://github.com/StollD/KopernicusExpansion-Continued
 */

using System;
using UnityEngine;

namespace RealSolarSystem
{
    /// <summary>
    /// A colormap PQSMod that can parse cubemapped textures.
    /// </summary>
    public class PQSMod_VertexColorCubeMap : PQSMod
    {

        public MapSO vertexColorMapXn;
        public MapSO vertexColorMapXp;
        public MapSO vertexColorMapYn;
        public MapSO vertexColorMapYp;
        public MapSO vertexColorMapZn;
        public MapSO vertexColorMapZp;
        public double edgeClampRange;

        public Vector3d UVtoXYZ(double u, double v)
        {
            Vector3d coords = new Vector3d();

            double theta = 2.0 * Math.PI * u;
            double phi = Math.PI * v;

            coords.x = (float)(Math.Cos(theta) * Math.Sin(phi));
            coords.y = (float)(-Math.Cos(phi));
            coords.z = (float)(Math.Sin(theta) * Math.Sin(phi));

            return coords;
        }
        public Vector3d XYZtoFaceUVI(Vector3d coords)
        {
            //X = U, Y = V, Z = FaceIndex 
            Vector3d uvIndex = new Vector3d();
            Vector3d absCoords = new Vector3d(Math.Abs(coords.x), Math.Abs(coords.y), Math.Abs(coords.z));

            Boolean isXPositive = coords.x > 0 ? true : false;
            Boolean isYPositive = coords.y > 0 ? true : false;
            Boolean isZPositive = coords.z > 0 ? true : false;

            double maxAxis = 1;

            //Negative X
            if (!isXPositive && absCoords.x >= absCoords.y && absCoords.x >= absCoords.z)
            {
                maxAxis = absCoords.x;
                uvIndex.x = coords.z;
                uvIndex.y = coords.y;
                uvIndex.z = 0;
            }
            //Positive X
            if (isXPositive && absCoords.x >= absCoords.y && absCoords.x >= absCoords.z)
            {
                maxAxis = absCoords.x;
                uvIndex.x = -coords.z;
                uvIndex.y = coords.y;
                uvIndex.z = 1;
            }
            //Negative Y
            if (!isYPositive && absCoords.y >= absCoords.x && absCoords.y >= absCoords.z)
            {
                maxAxis = absCoords.y;
                uvIndex.x = coords.x;
                uvIndex.y = coords.z;
                uvIndex.z = 2;
            }
            //Positive Y
            if (isYPositive && absCoords.y >= absCoords.x && absCoords.y >= absCoords.z)
            {
                maxAxis = absCoords.y;
                uvIndex.x = coords.x;
                uvIndex.y = -coords.z;
                uvIndex.z = 3;
            }
            //Negative Z
            if (!isZPositive && absCoords.z >= absCoords.x && absCoords.z >= absCoords.y)
            {
                maxAxis = absCoords.z;
                uvIndex.x = -coords.x;
                uvIndex.y = coords.y;
                uvIndex.z = 4;
            }
            if (isZPositive && absCoords.z >= absCoords.x && absCoords.z >= absCoords.y)
            {
                maxAxis = absCoords.z;
                uvIndex.x = coords.x;
                uvIndex.y = coords.y;
                uvIndex.z = 5;
            }

            uvIndex.x = 0.5f * (uvIndex.x / maxAxis + 1.0f);
            uvIndex.y = 0.5f * (uvIndex.y / maxAxis + 1.0f);


            return uvIndex;
        }

        public Color GetCubeMapColor(MapSO texXn, MapSO texXp, MapSO texYn, MapSO texYp, MapSO texZn, MapSO texZp, double u, double v)
        {
            Color col = new Color(255, 0, 255);
            Vector3d coords = UVtoXYZ(u, v);
            Vector3d uvIndex = XYZtoFaceUVI(coords);

            double pixelClamp = 1.0d / (texXn.Width / edgeClampRange);
            //Clamp values near edges to prevent wrapping
            uvIndex.x = Math.Max(uvIndex.x, pixelClamp);
            uvIndex.x = Math.Min(uvIndex.x, 1 - pixelClamp);
            uvIndex.y = Math.Max(uvIndex.y, pixelClamp);
            uvIndex.y = Math.Min(uvIndex.y, 1 - pixelClamp);

            //Xn -> Zn
            if (uvIndex.z == 0)
                return texZn.GetPixelColor((1 - uvIndex.x), (1 - uvIndex.y));

            if (uvIndex.z == 1)
                return texZp.GetPixelColor((1 - uvIndex.x), (1 - uvIndex.y));

            if (uvIndex.z == 2)
                return texYn.GetPixelColor((uvIndex.y), (1 - uvIndex.x));

            if (uvIndex.z == 3)
                return texYp.GetPixelColor((1 - uvIndex.y), (uvIndex.x));

            if (uvIndex.z == 4)
                return texXn.GetPixelColor((1 - uvIndex.x), (1 - uvIndex.y));

            if (uvIndex.z == 5)
                return texXp.GetPixelColor((1 - uvIndex.x), (1 - uvIndex.y));


            return col;
        }



        public override void OnSetup()
        {
            base.requirements = PQS.ModiferRequirements.MeshColorChannel;
        }
        public override void OnVertexBuildHeight(PQS.VertexBuildData data)
        {
            data.vertColor = GetCubeMapColor(vertexColorMapXn, vertexColorMapXp, vertexColorMapYn, vertexColorMapYp, vertexColorMapZn, vertexColorMapZp, (data.u), (data.v));
        }
    }
}
