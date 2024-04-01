using System;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using Kopernicus.ConfigParser.Attributes;
using Kopernicus.ConfigParser.BuiltinTypeParsers;
using Kopernicus.ConfigParser.Enumerations;
using Kopernicus.Configuration.Parsing;
using Kopernicus.Components;
using Kopernicus.OnDemand;

namespace Kopernicus.Configuration.ModLoader
{
    [RequireConfigType(ConfigType.Node)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class VertexHeightCubeMap : ModLoader<PQSMod_VertexHeightCubeMap>
    {
        // The map textures for the planet
        [ParserTarget("tilePath")]
        public StringCollectionParser tilepath
        {
            get { return Mod.path; }
            set { Mod.path = value; }
        }

        [ParserTarget("tileSize")]
        public NumericParser<int> tilesize
        {
            get { return Mod.size; }
            set { Mod.size = value; }
        }

        // Height map offset
        [ParserTarget("offset")]
        public NumericParser<Double> heightMapOffset
        {
            get { return Mod.heightMapOffset; }
            set 
            {
                Debug.Log("Setting Heightmap offset " + value.Value);
                Mod.heightMapOffset = value; 
            }
        }

        // Height map offset
        [ParserTarget("deformity")]
        public NumericParser<Double> heightMapDeformity
        {
            get { return Mod.heightMapDeformity; }
            set { Mod.heightMapDeformity = value; }
        }
        [ParserTarget("clampRange")]
        public NumericParser<Double> clampRange
        {
            get { return Mod.edgeClampRange; }
            set { Mod.edgeClampRange = value; }
        }
        // Height map offset
        [ParserTarget("scaleDeformityByRadius")]
        public NumericParser<Boolean> scaleDeformityByRadius
        {
            get { return Mod.scaleDeformityByRadius; }
            set { Mod.scaleDeformityByRadius = value; }
        }
    }
}
