// Material wrapper generated by shader translator tool
using System;
using UnityEngine;

namespace Kopernicus
{
    namespace MaterialWrapper
    {
        public class NormalDiffuseDetail : Material
        {
            // Internal property ID tracking object
            protected class Properties
            {
                // Return the shader for this wrapper
                private const String shaderName = "Legacy Shaders/Diffuse Detail";
                public static Shader shader
                {
                    get { return Shader.Find (shaderName); }
                }

                // Main Color, default = (1,1,1,1)
                public const String colorKey = "_Color";
                public Int32 colorID { get; private set; }

                // Base (RGB), default = "white" { }
                public const String mainTexKey = "_MainTex";
                public Int32 mainTexID { get; private set; }

                // Detail (RGB), default = "gray" { }
                public const String detailKey = "_Detail";
                public Int32 detailID { get; private set; }

                // Singleton instance
                private static Properties singleton = null;
                public static Properties Instance
                {
                    get
                    {
                        // Construct the singleton if it does not exist
                        if(singleton == null)
                            singleton = new Properties();
            
                        return singleton;
                    }
                }

                private Properties()
                {
                    colorID = Shader.PropertyToID(colorKey);
                    mainTexID = Shader.PropertyToID(mainTexKey);
                    detailID = Shader.PropertyToID(detailKey);
                }
            }

            // Is some random material this material 
            public static Boolean UsesSameShader(Material m)
            {
                return m.shader.name == Properties.shader.name;
            }

            // Main Color, default = (1,1,1,1)
            public new Color color
            {
                get { return GetColor (Properties.Instance.colorID); }
                set { SetColor (Properties.Instance.colorID, value); }
            }

            // Base (RGB), default = "white" { }
            public Texture2D mainTex
            {
                get { return GetTexture (Properties.Instance.mainTexID) as Texture2D; }
                set { SetTexture (Properties.Instance.mainTexID, value); }
            }

            public Vector2 mainTexScale
            {
                get { return GetTextureScale (Properties.mainTexKey); }
                set { SetTextureScale (Properties.mainTexKey, value); }
            }

            public Vector2 mainTexOffset
            {
                get { return GetTextureOffset (Properties.mainTexKey); }
                set { SetTextureOffset (Properties.mainTexKey, value); }
            }

            // Detail (RGB), default = "gray" { }
            public Texture2D detail
            {
                get { return GetTexture (Properties.Instance.detailID) as Texture2D; }
                set { SetTexture (Properties.Instance.detailID, value); }
            }

            public Vector2 detailScale
            {
                get { return GetTextureScale (Properties.detailKey); }
                set { SetTextureScale (Properties.detailKey, value); }
            }

            public Vector2 detailOffset
            {
                get { return GetTextureOffset (Properties.detailKey); }
                set { SetTextureOffset (Properties.detailKey, value); }
            }

            public NormalDiffuseDetail() : base(Properties.shader)
            {
            }

            [Obsolete("Creating materials from shader source String is no longer supported. Use Shader assets instead.")]
            public NormalDiffuseDetail(String contents) : base(contents)
            {
                base.shader = Properties.shader;
            }

            public NormalDiffuseDetail(Material material) : base(material)
            {
                // Throw exception if this material was not the proper material
                if (material.shader.name != Properties.shader.name)
                    throw new InvalidOperationException("Type Mismatch: Legacy Shaders/Diffuse Detail shader required");
            }

        }
    }
}
