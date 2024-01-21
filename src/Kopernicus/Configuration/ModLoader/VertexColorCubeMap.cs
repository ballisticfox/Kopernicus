/* 
 * This code is adapted from KopernicusExpansion-Continued
 * Available from https://github.com/StollD/KopernicusExpansion-Continued
 */

using System;
using System.Diagnostics.CodeAnalysis;
using Kopernicus.ConfigParser.Attributes;
using Kopernicus.ConfigParser.BuiltinTypeParsers;
using Kopernicus.ConfigParser.Enumerations;
using Kopernicus.Configuration.Parsing;
using Kopernicus.Components;

namespace Kopernicus.Configuration.ModLoader
{
    [RequireConfigType(ConfigType.Node)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class VertexColorCubeMap : ModLoader<PQSMod_VertexColorCubeMap>
    {
        // The map textures for the planet
        [ParserTarget("mapXn")]
        public MapSOParserRGB<MapSO> vertexColorMapXn
        {
            get { return Mod.vertexColorMapXn; }
            set { Mod.vertexColorMapXn = value; }
        }
        [ParserTarget("mapXp")]
        public MapSOParserRGB<MapSO> vertexColorMapXp
        {
            get { return Mod.vertexColorMapXp; }
            set { Mod.vertexColorMapXp = value; }
        }
        [ParserTarget("mapYn")]
        public MapSOParserRGB<MapSO> vertexColorMapYn
        {
            get { return Mod.vertexColorMapYn; }
            set { Mod.vertexColorMapYn = value; }
        }
        [ParserTarget("mapYp")]
        public MapSOParserRGB<MapSO> vertexColorMapYp
        {
            get { return Mod.vertexColorMapYp; }
            set { Mod.vertexColorMapYp = value; }
        }
        [ParserTarget("mapZn")]
        public MapSOParserRGB<MapSO> vertexColorMapZn
        {
            get { return Mod.vertexColorMapZn; }
            set { Mod.vertexColorMapZn = value; }
        }
        [ParserTarget("mapZp")]
        public MapSOParserRGB<MapSO> vertexColorMapZp
        {
            get { return Mod.vertexColorMapZp; }
            set { Mod.vertexColorMapZp = value; }
        }
        [ParserTarget("clampRange")]
        public NumericParser<Single> clampRange
        {
            get { return Mod.edgeClampRange; }
            set { Mod.edgeClampRange = value; }
        }
    }
}
