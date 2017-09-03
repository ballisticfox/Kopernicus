﻿/**
 * Kopernicus Planetary System Modifier
 * ------------------------------------------------------------- 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 * MA 02110-1301  USA
 * 
 * This library is intended to be used as a plugin for Kerbal Space Program
 * which is copyright 2011-2017 Squad. Your usage of Kerbal Space Program
 * itself is governed by the terms of its EULA, not the license above.
 * 
 * https://kerbalspaceprogram.com
 */

using System;

namespace Kopernicus
{
    namespace Configuration
    {
        namespace ModLoader
        {
            [RequireConfigType(ConfigType.Node)]
            public class MapDecal : ModLoader<PQSMod_MapDecal>
            {
                // absolute
                [ParserTarget("absolute")]
                public NumericParser<Boolean> absolute
                {
                    get { return mod.absolute; }
                    set { mod.absolute = value; }
                }

                // absoluteOffset
                [ParserTarget("absoluteOffset")]
                public NumericParser<Single> absoluteOffset
                {
                    get { return mod.absoluteOffset; }
                    set { mod.absoluteOffset = value; }
                }

                // angle
                [ParserTarget("angle")]
                public NumericParser<Single> angle
                {
                    get { return mod.angle; }
                    set { mod.angle = value; }
                }

                // colorMap
                [ParserTarget("colorMap")]
                public MapSOParser_RGB<MapSO> colorMap
                {
                    get { return mod.colorMap; }
                    set { mod.colorMap = value; }
                }

                // cullBlack
                [ParserTarget("cullBlack")]
                public NumericParser<Boolean> cullBlack
                {
                    get { return mod.cullBlack; }
                    set { mod.cullBlack = value; }
                }

                // DEBUG_HighlightInclusion
                [ParserTarget("DEBUG_HighlightInclusion")]
                public NumericParser<Boolean> DEBUG_HighlightInclusion
                {
                    get { return mod.DEBUG_HighlightInclusion; }
                    set { mod.DEBUG_HighlightInclusion = value; }
                }

                // heightMap
                [ParserTarget("heightMap")]
                public MapSOParser_GreyScale<MapSO> heightMap
                {
                    get { return mod.heightMap; }
                    set { mod.heightMap = value; }
                }

                // heightMapDeformity
                [ParserTarget("heightMapDeformity")]
                public NumericParser<Double> heightMapDeformity
                {
                    get { return mod.heightMapDeformity; }
                    set { mod.heightMapDeformity = value; }
                }

                // position
                [ParserTarget("position")]
                public Vector3Parser position
                {
                    get { return mod.position; }
                    set { mod.position = value; }
                }

                // position v2
                [ParserTarget("Position")]
                public PositionParser Position
                {
                    set { mod.position = value; }
                }

                // removeScatter
                [ParserTarget("removeScatter")]
                public NumericParser<Boolean> removeScatter
                {
                    get { return mod.removeScatter; }
                    set { mod.removeScatter = value; }
                }

                // radius
                [ParserTarget("radius")]
                public NumericParser<Double> radius
                {
                    get { return mod.radius; }
                    set { mod.radius = value; }
                }

                // smoothColor
                [ParserTarget("smoothColor")]
                public NumericParser<Single> smoothColor
                {
                    get { return mod.smoothColor; }
                    set { mod.smoothColor = value; }
                }

                // smoothHeight
                [ParserTarget("smoothHeight")]
                public NumericParser<Single> smoothHeight
                {
                    get { return mod.smoothHeight; }
                    set { mod.smoothHeight = value; }
                }

                // useAlphaHeightSmoothing
                [ParserTarget("useAlphaHeightSmoothing")]
                public NumericParser<Boolean> useAlphaHeightSmoothing
                {
                    get { return mod.useAlphaHeightSmoothing; }
                    set { mod.useAlphaHeightSmoothing = value; }
                }
            }
        }
    }
}

