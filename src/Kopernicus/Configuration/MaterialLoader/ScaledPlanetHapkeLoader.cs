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
using static KSP.UI.Screens.Settings.SettingsSetup;

namespace Kopernicus.Configuration.MaterialLoader
{
    [RequireConfigType(ConfigType.Node)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ScaledPlanetHapkeLoader : ScaledPlanetHapke
    {
        // Main Color, default = (1,1,1,1)
        [ParserTarget("color")]
        public ColorParser ColorSetter
        {
            get { return Tint; }
            set { Tint = value; }
        }

        // Shininess, default = 0.078125
        [ParserTarget("porosityCoeffient")]
        public NumericParser<Single> kSetter
        {
            get { return K; }
            set { K = value; }
        }
        [ParserTarget("roughnessAngle")]
        public NumericParser<Single> ThetaSetter
        {
            get { return Theta; }
            set { Theta = value; }
        }
        [ParserTarget("shininess")]
        public NumericParser<Single> mThetaSetter
        {
            get { return mTheta; }
            set { mTheta = value; }
        }
        [ParserTarget("brightness")]
        public NumericParser<Single> LSetter
        {
            get { return L; }
            set { L = value; }
        }
        [ParserTarget("gamma")]
        public NumericParser<Single> GSetter
        {
            get { return G; }
            set { G = value; }
        }
        [ParserTarget("specColor")]
        public ColorParser SpecColorSetter
        {
            get { return specColor; }
            set { specColor = value; }
        }
        [ParserTarget("refractiveIndex")]
        public NumericParser<Single> NSetter
        {
            get { return N; }
            set { N = value; }
        }

        [ParserTarget("shadowMaskStrength")]
        public NumericParser<Single> ShadowMaskStrengthSetter
        {
            get { return ShadowMaskStrength; }
            set { ShadowMaskStrength = value; }
        }
        [ParserTarget("shadowMaskOffset")]
        public NumericParser<Single> ShadowMaskOffsetSetter
        {
            get { return ShadowMaskOffset; }
            set { ShadowMaskOffset = value; }
        }
        [ParserTarget("debugTex")]
        public NumericParser<Int32> DebugSetter
        {
            get { return Debug; }
            set { Debug = value; }
        }
        [ParserTarget("scatterTex")]
        public Texture2DParser ScatterTexSetter
        {
            get { return ScatterTex; }
            set
            {
                ScatterTex = value;
            }
        }
        [ParserTarget("surgeTex")]
        public Texture2DParser SurgeTexSetter
        {
            get { return SurgeTex; }
            set
            {
                SurgeTex = value;
            }
        }

        // Base (RGB) Gloss (A), default = "white" { }
        [ParserTarget("mainTex")]
        public Texture2DParser MainTexSetter
        {
            get { return MainTex; }
            set
            {
                MainTex = value;
            }
        }

        // Normal map, default = "bump" { }
        [ParserTarget("bumpMap")]
        public Texture2DParser BumpMapSetter
        {
            get { return BumpMap; }
            set
            {
                BumpMap = value;
            }
        }

        // Normal Map Scale/Strength
        [ParserTarget("bumpScale")]
        public NumericParser<Single> BumpScaleSetter
        {
            get { return BumpScale; }
            set
            {
                BumpScale = value;
            }
        }


        // Constructors
        public ScaledPlanetHapkeLoader()
        {
        }

        [Obsolete("Creating materials from shader source String is no longer supported. Use Shader assets instead.")]
        public ScaledPlanetHapkeLoader(String contents) : base(contents)
        {
        }

        public ScaledPlanetHapkeLoader(Material material) : base(material)
        {
        }
    }
}
