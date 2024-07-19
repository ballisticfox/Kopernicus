﻿// Material wrapper generated by shader translator tool

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Kopernicus.Components.MaterialWrapper
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    public class HapkeCubeMapped : Material
    {
        // Internal property ID tracking object
        protected class Properties
        {
            // Return the shader for this wrapper
            private const String SHADER_NAME = "Terrain/HapkeCubeMapped";


            //This is going to spam the log but I don't know how else to do it
            public static Shader Shader
            {
                get
                {
                    return ShaderLoader.GetShader(SHADER_NAME);
                }
            }

            // Color Tint, default = (1,1,1,1)
            public const String TINT_KEY = "_Tint";
            public Int32 TintId { get; private set; }


            ///
            // Cubemap mainTex, default is "white"
            public const String MAIN_TEX_Xn_KEY = "_MainTex_Xn";
            public Int32 MainTex_XnId { get; private set; }

            public const String MAIN_TEX_Xp_KEY = "_MainTex_Xp";
            public Int32 MainTex_XpId { get; private set; }

            public const String MAIN_TEX_Yn_KEY = "_MainTex_Yn";
            public Int32 MainTex_YnId { get; private set; }

            public const String MAIN_TEX_Yp_KEY = "_MainTex_Yp";
            public Int32 MainTex_YpId { get; private set; }

            public const String MAIN_TEX_Zn_KEY = "_MainTex_Zn";
            public Int32 MainTex_ZnId { get; private set; }

            public const String MAIN_TEX_Zp_KEY = "_MainTex_Zp";
            public Int32 MainTex_ZpId { get; private set; }
            ///


            // Cubemap Normal map, default = "bump" { }
            public const String BUMP_MAP_Xn_KEY = "_BumpMap_Xn";
            public Int32 BumpMap_XnId { get; private set; }

            public const String BUMP_MAP_Xp_KEY = "_BumpMap_Xp";
            public Int32 BumpMap_XpId { get; private set; }

            public const String BUMP_MAP_Yn_KEY = "_BumpMap_Yn";
            public Int32 BumpMap_YnId { get; private set; }

            public const String BUMP_MAP_Yp_KEY = "_BumpMap_Yp";
            public Int32 BumpMap_YpId { get; private set; }

            public const String BUMP_MAP_Zn_KEY = "_BumpMap_Zn";
            public Int32 BumpMap_ZnId { get; private set; }

            public const String BUMP_MAP_Zp_KEY = "_BumpMap_Zp";
            public Int32 BumpMap_ZpId { get; private set; }

            //Bump Map Intensity
            public const String BUMP_SCALE_KEY = "_BumpScale";
            public Int32 BumpScaleId { get; private set; }


            public const String SCATTERING_TEX_KEY = "_ScatteringTex";
            public Int32 ScatteringTexId { get; private set; }


            public const String SURGE_TEX_KEY = "_SurgeTex";
            public Int32 SurgeTexId { get; private set; }


            public const String K_KEY = "_K";
            public Int32 kId { get; private set; }

            public const String Theta_KEY = "_Theta";
            public Int32 ThetaId { get; private set; }

            public const String L_KEY = "_L";
            public Int32 lId { get; private set; }

            public const String G_KEY = "_G";
            public Int32 gId { get; private set; }

            public const String SHADOW_MASK_STRENGTH_KEY = "_shadowMaskStrength";
            public Int32 ShadowMaskStrengthId { get; private set; }

            public const String SHADOW_MASK_OFFSET_KEY = "_shadowMaskOffset";
            public Int32 ShadowMaskOffsetId { get; private set; }

            public const String DEBUG_KEY = "_debug";
            public Int32 debugId { get; private set; }
            //Ocean Radius, radius of the celestial body * Worldscale

            //Planet Origin
            //Depth Pull
            //SunPos
            //SunRadius



            // Singleton instance
            private static Properties _singleton;

            public static Properties Instance
            {
                get
                {
                    // Construct the singleton if it does not exist
                    return _singleton ?? (_singleton = new Properties());
                }
            }

            private Properties()
            {
                TintId       = Shader.PropertyToID(TINT_KEY);
                MainTex_XnId = Shader.PropertyToID(MAIN_TEX_Xn_KEY);
                MainTex_XpId = Shader.PropertyToID(MAIN_TEX_Xp_KEY);
                MainTex_YnId = Shader.PropertyToID(MAIN_TEX_Yn_KEY);
                MainTex_YpId = Shader.PropertyToID(MAIN_TEX_Yp_KEY);
                MainTex_ZnId = Shader.PropertyToID(MAIN_TEX_Zn_KEY);
                MainTex_ZpId = Shader.PropertyToID(MAIN_TEX_Zp_KEY);

                BumpMap_XnId = Shader.PropertyToID(BUMP_MAP_Xn_KEY);
                BumpMap_XpId = Shader.PropertyToID(BUMP_MAP_Xp_KEY);
                BumpMap_YnId = Shader.PropertyToID(BUMP_MAP_Yn_KEY);
                BumpMap_YpId = Shader.PropertyToID(BUMP_MAP_Yp_KEY);
                BumpMap_ZnId = Shader.PropertyToID(BUMP_MAP_Zn_KEY);
                BumpMap_ZpId = Shader.PropertyToID(BUMP_MAP_Zp_KEY);

                BumpScaleId = Shader.PropertyToID(BUMP_SCALE_KEY);

                ScatteringTexId = Shader.PropertyToID(SCATTERING_TEX_KEY);
                SurgeTexId = Shader.PropertyToID(SURGE_TEX_KEY);

                kId = Shader.PropertyToID(K_KEY);
                ThetaId = Shader.PropertyToID(Theta_KEY);
                lId = Shader.PropertyToID(L_KEY);
                gId = Shader.PropertyToID(G_KEY);

                ShadowMaskStrengthId = Shader.PropertyToID(SHADOW_MASK_STRENGTH_KEY);
                ShadowMaskOffsetId = Shader.PropertyToID(SHADOW_MASK_OFFSET_KEY);

                debugId = Shader.PropertyToID(DEBUG_KEY);
            }
        }

        // Is some random material this material 
        public static Boolean UsesSameShader(Material m)
        {
            if (m == null)
            {
                return false;
            }
            bool usesSameShader = m.shader.name == Properties.Shader.name;
            return m.shader.name == Properties.Shader.name;
        }

        // Main Color, default = (1,1,1,1)
        public Color Tint
        {
            get { return GetColor(Properties.Instance.TintId); }
            set { SetColor(Properties.Instance.TintId, value); }
        }
        public Single K
        {
            get { return GetFloat(Properties.Instance.kId); }
            set { SetFloat(Properties.Instance.kId, value); }
        }
        public Single Theta
        {
            get { return GetFloat(Properties.Instance.ThetaId); }
            set { SetFloat(Properties.Instance.ThetaId, value); }
        }
        public Single L
        {
            get { return GetFloat(Properties.Instance.lId); }
            set { SetFloat(Properties.Instance.lId, value); }
        }
        public Single G
        {
            get { return GetFloat(Properties.Instance.gId); }
            set { SetFloat(Properties.Instance.gId, value); }
        }
        public Single ShadowMaskStrength
        {
            get { return GetFloat(Properties.Instance.ShadowMaskStrengthId); }
            set { SetFloat(Properties.Instance.ShadowMaskStrengthId, value); }
        }
        public Single ShadowMaskOffset
        {
            get { return GetFloat(Properties.Instance.ShadowMaskOffsetId); }
            set { SetFloat(Properties.Instance.ShadowMaskOffsetId, value); }
        }
        public int Debug
        {
            get { return GetInt(Properties.Instance.debugId); }
            set { SetInt(Properties.Instance.debugId, value); }
        }
        public Single BumpScale
        {
            get { return GetFloat(Properties.Instance.BumpScaleId); }
            set { SetFloat(Properties.Instance.BumpScaleId, value); }
        }
        public Texture2D ScatterTex
        {
            get { return GetTexture(Properties.Instance.ScatteringTexId) as Texture2D; }
            set { SetTexture(Properties.Instance.ScatteringTexId, value); }
        }
        public Texture2D SurgeTex
        {
            get { return GetTexture(Properties.Instance.SurgeTexId) as Texture2D; }
            set { SetTexture(Properties.Instance.SurgeTexId, value); }
        }

        public Texture2D MainTex_Xn
        {
            get { return GetTexture(Properties.Instance.MainTex_XnId) as Texture2D; }
            set { SetTexture(Properties.Instance.MainTex_XnId, value); }
        }
        public Texture2D MainTex_Xp
        {
            get { return GetTexture(Properties.Instance.MainTex_XpId) as Texture2D; }
            set { SetTexture(Properties.Instance.MainTex_XpId, value); }
        }

        public Texture2D MainTex_Yn
        {
            get { return GetTexture(Properties.Instance.MainTex_YnId) as Texture2D; }
            set { SetTexture(Properties.Instance.MainTex_YnId, value); }
        }
        public Texture2D MainTex_Yp
        {
            get { return GetTexture(Properties.Instance.MainTex_YpId) as Texture2D; }
            set { SetTexture(Properties.Instance.MainTex_YpId, value); }
        }

        public Texture2D MainTex_Zn
        {
            get { return GetTexture(Properties.Instance.MainTex_ZnId) as Texture2D; }
            set { SetTexture(Properties.Instance.MainTex_ZnId, value); }
        }
        public Texture2D MainTex_Zp
        {
            get { return GetTexture(Properties.Instance.MainTex_ZpId) as Texture2D; }
            set { SetTexture(Properties.Instance.MainTex_ZpId, value); }
        }

        public Texture2D BumpMap_Xn
        {
            get { return GetTexture(Properties.Instance.BumpMap_XnId) as Texture2D; }
            set { SetTexture(Properties.Instance.BumpMap_XnId, value); }
        }
        public Texture2D BumpMap_Xp
        {
            get { return GetTexture(Properties.Instance.BumpMap_XpId) as Texture2D; }
            set { SetTexture(Properties.Instance.BumpMap_XpId, value); }
        }

        public Texture2D BumpMap_Yn
        {
            get { return GetTexture(Properties.Instance.BumpMap_YnId) as Texture2D; }
            set { SetTexture(Properties.Instance.BumpMap_YnId, value); }
        }
        public Texture2D BumpMap_Yp
        {
            get { return GetTexture(Properties.Instance.BumpMap_YpId) as Texture2D; }
            set { SetTexture(Properties.Instance.BumpMap_YpId, value); }
        }

        public Texture2D BumpMap_Zn
        {
            get { return GetTexture(Properties.Instance.BumpMap_ZnId) as Texture2D; }
            set { SetTexture(Properties.Instance.BumpMap_ZnId, value); }
        }
        public Texture2D BumpMap_Zp
        {
            get { return GetTexture(Properties.Instance.BumpMap_ZpId) as Texture2D; }
            set { SetTexture(Properties.Instance.BumpMap_ZpId, value); }
        }



        public HapkeCubeMapped() : base(Properties.Shader)
        {
        }

        [Obsolete("Creating materials from shader source String is no longer supported. Use Shader assets instead.")]
        public HapkeCubeMapped(String contents) : base(contents)
        {
            shader = Properties.Shader;
        }

        public HapkeCubeMapped(Material material) : base(material)
        {
            // Throw exception if this material was not the proper material
            if (material.shader.name != Properties.Shader.name)
            {
                throw new InvalidOperationException("Type Mismatch: Terrain/HapkeCubemapped shader required");
            }
        }

    }
}
