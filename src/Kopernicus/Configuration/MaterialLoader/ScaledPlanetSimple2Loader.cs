/**
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
 * which is copyright of TakeTwo Interactive. Your usage of Kerbal Space Program
 * itself is governed by the terms of its EULA, not the license above.
 * 
 * https://kerbalspaceprogram.com
 */

using System;
using System.Diagnostics.CodeAnalysis;
using Kopernicus.Components.MaterialWrapper;
using Kopernicus.ConfigParser.Attributes;
using Kopernicus.ConfigParser.BuiltinTypeParsers;
using Kopernicus.ConfigParser.Enumerations;
using Kopernicus.Configuration.Parsing;
using UnityEngine;

namespace Kopernicus.Configuration.MaterialLoader
{
    [RequireConfigType(ConfigType.Node)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ScaledPlanetSimple2Loader : ScaledPlanetSimple2
    {
        // Main Color, default = (1,1,1,1)
        [ParserTarget("color")]
        public ColorParser ColorSetter
        {
            get { return Color; }
            set { Color = value; }
        }

        // Specular Color, default = (0.5,0.5,0.5,1)
        [ParserTarget("specColor")]
        public ColorParser SpecColorSetter
        {
            get { return SpecColor; }
            set { SpecColor = value; }
        }

        // Shininess, default = 0.078125
        [ParserTarget("shininess")]
        public NumericParser<Single> ShininessSetter
        {
            get { return Shininess; }
            set { Shininess = value; }
        }
        [ParserTarget("dayLightRange")]
        public NumericParser<Single> DaylightSetter
        {
            get { return Daylight; }
            set { Daylight = value; }
        }
        [ParserTarget("emitColor")]
        public ColorParser EmitColorSetter
        {
            get { return EmitColor; }
            set { EmitColor = value; }
        }

        // Base (RGB) Gloss (A), default = "white" { }
        [ParserTarget("mainTexXn")]
        public Texture2DParser MainTexXnSetter
        {
            get { return MainTex_Xn; }
            set 
            {
                MainTex_Xn = value;
                MainTex_Xn.wrapMode = TextureWrapMode.Clamp;
            }
        }
        [ParserTarget("mainTexXp")]
        public Texture2DParser MainTexXpSetter
        {
            get { return MainTex_Xp; }
            set 
            { 
                MainTex_Xp = value;
                MainTex_Xp.wrapMode = TextureWrapMode.Clamp;
            }
        }

        [ParserTarget("mainTexYn")]
        public Texture2DParser MainTexYnSetter
        {
            get { return MainTex_Yn; }
            set 
            {
                MainTex_Yn = value;
                MainTex_Yn.wrapMode = TextureWrapMode.Clamp;
            }
        }
        [ParserTarget("mainTexYp")]
        public Texture2DParser MainTexYpSetter
        {
            get { return MainTex_Yp; }
            set 
            {
                MainTex_Yp = value;
                MainTex_Yp.wrapMode = TextureWrapMode.Clamp;
            }
        }

        [ParserTarget("mainTexZn")]
        public Texture2DParser MainTexZnSetter
        {
            get { return MainTex_Zn; }
            set 
            {
                MainTex_Zn = value;
                MainTex_Zn.wrapMode = TextureWrapMode.Clamp;
            }
        }
        [ParserTarget("mainTexZp")]
        public Texture2DParser MainTexZpSetter
        {
            get { return MainTex_Zp; }
            set 
            {
                MainTex_Zp = value;
                MainTex_Zp.wrapMode = TextureWrapMode.Clamp;
            }
        }

        // Normal map, default = "bump" { }
        [ParserTarget("bumpMapXn")]
        public Texture2DParser BumpMapXnSetter
        {
            get { return BumpMap_Xn; }
            set 
            { 
                BumpMap_Xn = value;
                BumpMap_Xn.wrapMode = TextureWrapMode.Clamp;
            }
        }
        [ParserTarget("bumpMapXp")]
        public Texture2DParser BumpMapXpSetter
        {
            get { return BumpMap_Xp; }
            set 
            { 
                BumpMap_Xp = value;
                BumpMap_Xp.wrapMode = TextureWrapMode.Clamp;
            }
        }
        [ParserTarget("bumpMapYn")]
        public Texture2DParser BumpMapYnSetter
        {
            get { return BumpMap_Yn; }
            set 
            {
                BumpMap_Yn = value;
                BumpMap_Yn.wrapMode = TextureWrapMode.Clamp;
            }
        }
        [ParserTarget("bumpMapYp")]
        public Texture2DParser BumpMapYpSetter
        {
            get { return BumpMap_Yp; }
            set 
            {
                BumpMap_Yp = value;
                BumpMap_Yp.wrapMode = TextureWrapMode.Clamp;
            }
        }
        [ParserTarget("bumpMapZn")]
        public Texture2DParser BumpMapZnSetter
        {
            get { return BumpMap_Zn; }
            set 
            {
                BumpMap_Zn = value;
                BumpMap_Zn.wrapMode = TextureWrapMode.Clamp;
            }
        }
        [ParserTarget("bumpMapZp")]
        public Texture2DParser BumpMapZpSetter
        {
            get { return BumpMap_Zp; }
            set 
            {
                BumpMap_Zp = value;
                BumpMap_Zp.wrapMode = TextureWrapMode.Clamp;
            }
        }

        // Normal Map Scale/Strength
        [ParserTarget("BumpScale")]
        public NumericParser<Single> BumpScaleSetter
        {
            get { return BumpScale; }
            set 
            {
                BumpScale = value;
            }
        }


        // Constructors
        public ScaledPlanetSimple2Loader()
        {
        }

        [Obsolete("Creating materials from shader source String is no longer supported. Use Shader assets instead.")]
        public ScaledPlanetSimple2Loader(String contents) : base(contents)
        {
        }

        public ScaledPlanetSimple2Loader(Material material) : base(material)
        {
        }
    }
}
