Shader "Terrain/HapkeSpecular"
{

    Properties
    {
        _Tint("Tint", Color) = (1,1,1,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _NormalTex("BumpMap", 2D) = "bump" {}
        _BumpScale("Normal scale", Float) = 1
        _ScatteringTex("ScatterTex", 2D) = "white" {}
        _SurgeTex("SurgeTex", 2D) = "white" {}
        _K ("Porosity Coeffient", Float) = 2
        _Theta ("Mean Roughness Angle", float) = 25
        _mTheta ("Shininess", float) = 1
        _L ("Brightness", Float) = 1
        _G ("Gamma", Float) = 2.2
        _specColor("Specular Color", Color) = (1,1,1,1)
        _n("Refractive Index", Float) = 1.333 
        _shadowMaskStrength("Shadow Mask Strength", FLoat) = 1
        _shadowMaskOffset("Shadow Mask Offset", Float) = 0

        _debug("Debug Textures", Int) = 1

        // w - single Scattering albedo | 0 - 1
        // b - phase function shape | 0 - 1
        // c - phase function intensity | -1 - 1
        // Psi - Used in porosity function | 0 - 0.792
        // B_s0 - Amplitude of SHOE | 0 - 2
        // h_s - Angular width of SHOE | 0 - 1
        // B_C0 - Amplitude of CBOE | 0 - 2
        // h_c - Mean slope roughness | 0 - pi/2
    }
    Subshader
    {
        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            
            
			#pragma vertex vert
			#pragma fragment frag
            
            #define M_PI 3.1415926535897932384626433832795

			#include "Lighting.cginc"
			#include "UnityCG.cginc"
            #include "AutoLight.cginc"

            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NormalTex;
            float4 _NormalTex_ST;
            float _BumpScale;

            sampler2D _ScatteringTex;
            float4 _ScatteringTex_ST;
            sampler2D _SurgeTex;
            float4 _SurgeTex_ST;
            float _K;
            float _Theta;
            float _mTheta;
            float4 _specColor;
            float _n;
            float _L;
            float _G;

            float _shadowMaskStrength;
            float _shadowMaskOffset;

            int _debug;

            uniform float4x4 _MainRotation;

            // Theory of Reflectance and Emittance Spectroscopy, Bruce Hapke, 2012
            // Chapter 6, 7, 8, 9, and 12 will be used



            // Multiple Scattering Approximations

            // Albedo Factor: (8.22b)
            float gamma(float w)
            {
                return sqrt(1.0-w);
            }

            // Diffusive Reflectance: (8.25)
            float r_0(float w)
            {
                return 2.0 / (1.0 +  gamma(w));
            }

            // Ambartsumian–Chandrasekhar H function: (8.56)
            float H(float x, float w)
            {
                float value = 1.0/(1.0 - w * x * (r_0(w) + (1.0-2.0*r_0(w)*x)/2.0 * log((1.0+x)/x)));
                return value;
            }

            // Double-lobed Henyey-Greenstein function (6.7a)
            float PI_HG2(float g, float b, float c)
            {
                float backwardLobe = ((1.0+c)/2.0) * ((1-b*b) /  pow(1.0-2.0*b*cos(g)+b*b, 1.5));
                float forwardLobe =  ((1.0-c)/2.0) * ((1-b*b) /  pow(1.0+2.0*b*cos(g)+b*b, 1.5));
                return backwardLobe + forwardLobe;
            }

            // Opposition Surge

            // Shadow Hiding Opposition function (9.22)
            float B_s(float g, float h_s)
            {
                return 1.0/(1.0+(1.0/h_s)*tan(g/2.0));
            }

            // (9.43)
            float B_c(float g, float h_c, float K)
            {
                return 1.0 / (1.0 + (1.3 + K)*(1.0/h_c * tan(g/2.0) + pow(1.0/h_c * tan(g/2.0), 2.0)));
            }



            
            // Roughness
            // (12.45a)
            // Verified
            float X(float theta)
            {
                return 1.0 /sqrt(1.0 +M_PI*pow(tan(theta), 2.0));
            }

            // (12.45b)
            // Verified
            float E_1(float x, float theta)
            {
                return exp(-1.0*(2.0/M_PI)*(1.0/tan(theta))*(1.0/tan(x)));
                
            }

            // (12.45c)
            // Verified
            float E_2(float x, float theta)
            {
                return exp( -1.0 * (1.0/M_PI) * pow(1.0/tan(theta), 2.0) * pow(1.0/tan(x), 2.0)); 
            }

            // (12.48)
            // Verified
            float n_e(float x, float theta)
            {
                return X(theta)*(cos(x)+sin(x)*tan(theta) * (E_2(x, theta) / (2.0 - E_1(x, theta))));
            }

            // (12.51)
            // Verified
            float f(float psi)
            {
                return exp(-2.0 * tan(psi/2.0));
            }

            // Section (12.2.2 - 12.2.3)
            float S(float mu_0, float mu, float psi, float theta)
            {
                float i = acos(mu_0);
                float e = acos(mu);
                if (e > i)
                {
                    // (12.47)
                    // Verified
                    float mu_e = X(theta) * (cos(e) + sin(e)*tan(theta) * (E_2(e, theta) - pow(sin(psi/2.0), 2.0) * E_2(i, theta)) / (2.0 - E_1(e, theta) - (psi / M_PI) * E_1(i, theta)));
                    
                    // (12.50)
                    // Verified        
                    return ((mu_e/n_e(e, theta))  * (mu_0 / n_e(i, theta)) * (X(theta) / (1.0 - f(psi) + f(psi)*X(theta) * (mu_0 / n_e(i, theta)))));
                }
                else
                {
                    // (12.53)
                    // Verified
                    float mu_e = X(theta) * (cos(e) + sin(e)*tan(theta) * (cos(psi) * E_2(i, theta) + pow(sin(psi/2.0), 2.0) * E_2(e, theta)) / (2.0 - E_1(i, theta) - (psi / M_PI) * E_1(e, theta)));

                    // (12.54)
                    // Verified
                    return ((mu_e/n_e(e, theta))  * (mu_0 / n_e(i, theta)) * (X(theta) / (1.0 - f(psi) + f(psi)*X(theta) * (mu / n_e(e, theta)))));
                }
            }

            ///--------- Specular Data ------------///
            // (4.21)
            float SnellsLaw(float theta)
            {
                return asin(sin(theta)/_n);
            }
            float Delta(float x)
            {
                if (x == 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            // (4.37)
            float R_perpendicular(float theta)
            {
                float theta_prime = SnellsLaw(theta);
                return pow(sin(theta-theta_prime)/sin(theta+theta_prime),2);
            }
            // (4.38)
            float R_parallel(float theta)
            {
                float theta_prime = SnellsLaw(theta);
                return pow(tan(theta-theta_prime)/tan(theta+theta_prime),2);
            }
            // (10.10 Addendum)
            float FresnelReflectance(float theta)
            {
                return (R_parallel(theta)+R_perpendicular(theta))/2;
            }
            float BeckmannDistribution(float alpha, float m)
            {
                return 1 / (m*pow(alpha,4))*exp((alpha*alpha-1)/(m*alpha*alpha));
            }
            float3 GGXMicrofacetCoeff(float3 surfNormal, float3 halfDir, float m)
            {
                return (m)/(M_PI*pow((pow(dot(surfNormal,halfDir),2)*(m-1)+1),2));
            }
            float3 ComputeSpecular(float3 specColor, float3 viewDir, float3 lightDir, float3 normalDir, float theta)
            {
                specColor = specColor/length(specColor);
                float mu_0 = clamp(dot(lightDir,normalDir),0.0001,1);
                float mu = clamp(dot(viewDir,normalDir),0.0001,1);

                float g = acos(dot(lightDir,viewDir));
                float h = acos(dot(normalize(viewDir+lightDir),viewDir));
                float alpha = dot(normalize(viewDir+lightDir),normalDir);

                float G = S(mu_0, mu, g, theta);
                // float D = GGXMicrofacetCoeff(normalDir,normalize(viewDir+lightDir),theta);
                float D = BeckmannDistribution(alpha, theta);

                float3 reflectance = (FresnelReflectance(h)*specColor) * ((G*D)/(M_PI*dot(normalDir,lightDir)*(dot(normalDir,viewDir))));
                return reflectance;
            }
            float3 CompressColor(float3 value)
            {
                return -1/(value+1)+1;
            }










            struct VertexData
            {
                float4 vertex : POSITION;
                float3 normal: NORMAL;
                float4 tangent: TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct FragData
            {
                float4 position: SV_POSITION;
                float2 uv: TEXCOORD0;
                float3 normal: TEXCOORD1;
                float3 viewDir: TEXCOORD2;
                float4 objMain : TEXCOORD3;
                half3 tspace0 : TEXCOORD8; // tangent.x, bitangent.x, normal.x
				half3 tspace1 : TEXCOORD9; // tangent.y, bitangent.y, normal.y
				half3 tspace2 : TEXCOORD10; // tangent.z, bitangent.z, normal.z
            };
            
            FragData vert(VertexData vertexData)
            {
                FragData fragData;

                float4 vertexPos = mul(unity_ObjectToWorld, vertexData.vertex);
                float3 origin = mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz;
                fragData.position = UnityObjectToClipPos(vertexData.vertex);
                fragData.viewDir = normalize(WorldSpaceViewDir(vertexData.vertex));
                fragData.normal = UnityObjectToWorldNormal(vertexData.normal);
                fragData.objMain = mul(_MainRotation, vertexData.vertex);


                // Normal Data
                float3 worldNormal = normalize(vertexPos - origin);
				// half4 tangent =  float4(UnityObjectToWorldDir(vertexData.tangent.xyz), vertexData.tangent.w);
                half4 tangent = _BumpScale*half4(normalize(half3(worldNormal.z, 0.000001, -worldNormal.x)), vertexData.tangent.w*unity_WorldTransformParams.w);
                half3 wTangent = tangent.xyz;
                half3 wBitangent = cross(worldNormal, wTangent) * tangent.w;
				fragData.tspace0 = half3(wTangent.x, wBitangent.x, worldNormal.x);
				fragData.tspace1 = half3(wTangent.y, wBitangent.y, worldNormal.y);
				fragData.tspace2 = half3(wTangent.z, wBitangent.z, worldNormal.z);
                fragData.uv = TRANSFORM_TEX(vertexData.uv, _ScatteringTex);

                return fragData;
            }


            float4 frag(FragData fragData) : SV_TARGET
            {


                /// Albedo, Normals, Lighting Data ///
                // Normals
                float4 normalTex = tex2D(_NormalTex, fragData.uv);

				half3 worldNormal;
                half3 tnormal = UnpackScaleNormal(normalTex, 1);
				worldNormal.x = dot(fragData.tspace0, tnormal);
				worldNormal.y = dot(fragData.tspace1, tnormal);
				worldNormal.z = dot(fragData.tspace2, tnormal);

                half3 maskNormal;
                half3 meshNormal = UnpackScaleNormal(half4(0.5,0.5,0.5,1), 1);
                maskNormal.x = dot(fragData.tspace0, meshNormal);
                maskNormal.y = dot(fragData.tspace1, meshNormal);
                maskNormal.z = dot(fragData.tspace2, meshNormal);
                


                // // Angle Data | (8.1)
                // float i = acos(dot(_WorldSpaceLightPos0,worldNormal));
                float mu_0 = clamp(dot(_WorldSpaceLightPos0,worldNormal),0.00001,1);

                // float e = acos(dot(fragData.viewDir,worldNormal) / (length(_WorldSpaceLightPos0)*length(worldNormal)));
                float mu = clamp(dot(fragData.viewDir,worldNormal),0.00001,1);

                float g = clamp(acos(dot(_WorldSpaceLightPos0,fragData.viewDir)), 0, 3.14159);

                float3 albedo = tex2D(_MainTex, fragData.uv) * _Tint;

                // Linear -> sRGB
                // albedo = GammaToLinearSpace(albedo);

                // Lighting - Lommel–Seeliger law | (8.14)
                float lighting = 1 / (mu_0+mu);
                float mask = acos(dot(_WorldSpaceLightPos0,maskNormal) / (length(_WorldSpaceLightPos0)*length(maskNormal)));
                mask = cos(mask+(_shadowMaskOffset*(M_PI)/2));
                mask = 1-pow(1-pow(mask, _shadowMaskStrength), 1/mask);

                /// Porosity and Roughness ///
                // float phi = 0.48;
                // K = -1 * log(1 - 1.209*pow(phi, 0.666666667))/1.209*pow(phi, 0.666666667)
                float K = _K;
                float theta = (_Theta * M_PI)/180;
                // float mtheta = sqrt((floor(_mTheta)/255*90)*M_PI/180);
                float3 specColor = _specColor;




                /// Phase and Multiple Scattering Functions ///

                // Single Scattering Albedo
                float w = tex2D(_ScatteringTex, fragData.uv).r;
                // w = LinearToGammaSpace(w);

                // Multiple Scattering Shape
                float b = tex2D(_ScatteringTex, fragData.uv).g;
                // b = LinearToGammaSpace(b);

                // Multiple Scattering Intensity
                float c = (tex2D(_ScatteringTex, fragData.uv).b);
                // c = LinearToGammaSpace(c);
                c *= 2;
                c -= 1;

                // Microfacet Angle
                float mtheta = (tex2D(_MainTex, fragData.uv).a);
                mtheta = (mtheta);
                mtheta = (mtheta*90)*M_PI/180;
                
                
                float4 multipleScattering = H(mu_0/K, w)*H(mu/K, w)-1;

                /// Opposition Surge ///

                //  Amplitude of Shadow Hiding Opposition Effect (SHOE)
                float B_s0 = tex2D(_SurgeTex, fragData.uv).r;
                // B_s0 = LinearToGammaSpace(B_s0);
                B_s0 *= 2;
                // B_s0 = GammaToLinearSpace(B_s0);

                // Angular width of SHOE
                float h_s = tex2D(_SurgeTex, fragData.uv).g;
                // h_s = LinearToGammaSpace(h_s);

                // Amplitude of Coherent Backscatter Opposition Effect (CBOE) - fixed at 0.0
                float b_c0 = tex2D(_SurgeTex, fragData.uv).b;
                // b_c0 = LinearToGammaSpace(b_c0);
                b_c0 *= 2;
                // b_c0 = GammaToLinearSpace(b_c0);
                
                // Angular width of CBOE - fixed at 1.0
                float4 h_c = tex2D(_SurgeTex, fragData.uv).a;
                // b_c0 = LinearToGammaSpace(b_c0);


                // Bidirectional Reflectance (12.55)
                // K * albedo/(4) * lighting * (PI_HG2(g, b, c)*(1+B_s0*B_s(g, h_s))+multipleScattering)
                float3 diffuseFactor = K * albedo/4 * (lighting) * (PI_HG2(g, b, c)*(1+B_s0*B_s(g, h_s))+multipleScattering) * (1+b_c0*B_c(g, h_c, K)) * S(mu_0, mu, g, theta);
                float3 specularFactor = ComputeSpecular(specColor, fragData.viewDir,_WorldSpaceLightPos0,worldNormal, clamp(mtheta*(1/_mTheta),0.0001,M_PI/2-0.0001));
                // float3 reflectance = pow(sin(mtheta*mtheta),2)*diffuseFactor + (pow(cos(mtheta*mtheta),2)*specularFactor);
                float3 reflectance = CompressColor(diffuseFactor) + CompressColor(specularFactor);
                // float3 reflectance = diffuseFactor + specularFactor;
                // float3 reflectance = ;
                // // reflectance = tex2D(_SurgeTex, fragData.uv).r;
                reflectance = _LightColor0*mu_0*reflectance;
                reflectance = pow(reflectance, _G);
                reflectance *= _L;
                reflectance *= mask;

                // Undo the gamma correction
                reflectance = LinearToGammaSpace(reflectance);
                
                if (_debug == 1)
                {
                    return float4(reflectance, 1);
                }
                else if (_debug == 2)
                {
                    return tex2D(_ScatteringTex, fragData.uv).r;
                }
                else if (_debug == 3)
                {
                    return tex2D(_ScatteringTex, fragData.uv).g;
                }
                else if (_debug == 4)
                {
                    return tex2D(_ScatteringTex, fragData.uv).b;
                }
                else if (_debug == 5)
                {
                    return tex2D(_SurgeTex, fragData.uv).r;
                }
                else if (_debug == 6)
                {
                    return tex2D(_SurgeTex, fragData.uv).g;
                }
                else if (_debug == 7)
                {
                    return tex2D(_SurgeTex, fragData.uv).b;
                }
                else if (_debug == 8)
                {
                    return tex2D(_SurgeTex, fragData.uv).a;
                }
                
                return float4(reflectance, 1);
            }


			ENDCG
        }
    }
}