// Adapated From EVE

Shader "Terrain/ScaledMesh2"
{
	Properties
	{
		_Color("Color Tint", Color) = (1,1,1,1)
		_MainTex_Xn("MainXn", 2D) = "white" {}
		_MainTex_Xp("MainXp", 2D) = "white" {}
		_MainTex_Yn("MainYn", 2D) = "white" {}
		_MainTex_Yp("MainYp", 2D) = "white" {}
		_MainTex_Zn("MainZn", 2D) = "white" {}
		_MainTex_Zp("MainZp", 2D) = "white" {}


		_BumpMap_Xn("BumpXn", 2D) = "bump" {}
		_BumpMap_Xp("BumpXp", 2D) = "bump" {}
		_BumpMap_Yn("BumpYn", 2D) = "bump" {}
		_BumpMap_Yp("BumpYp", 2D) = "bump" {}
		_BumpMap_Zn("BumpZn", 2D) = "bump" {}
		_BumpMap_Zp("BumpZp", 2D) = "bump" {}

		_BumpScale("Normal scale", Float) = 1
		_SpecColor("Specular Tint", Color) = (0,0,0,1)
		_Shininess("Shininess", Float) = 1

		_DayLight("Daylight Multiplier", Float) = 0
		_EmitColor("Emission Tint", Color) = (0,0,0,1)

		_PlanetOrigin("Sphere Center", Vector) = (0,0,0,1)
		_SunPos("_SunPos", Vector) = (0,0,0)
		_SunRadius("_SunRadius", Float) = 1
	}


	SubShader
	{
		Pass
		{
			name "Terrain/ScaledMesh2"
			Cull Back
			ZWrite On
			Tags
			{
				"Queue" = "Geometry"
				"IgnoreProjector" = "True"
				"RenderType" = "Opaque"
				"LightMode" = "ForwardBase"
			}

			CGPROGRAM

			#include "Lighting.cginc"
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			#pragma target 3.0
			#pragma glsl
			#pragma vertex vert
			#pragma fragment frag




			#define CUBEMAP_DEF(TexXn, TexXp, TexYn, TexYp, TexZn, TexZp)\
				sampler2D TexXn, TexXp; \
				sampler2D TexYn, TexYp; \
				sampler2D TexZn, TexZp;


			#define GetCubeCubeUV(cubeVect) \
				float3 cubeVectNorm = normalize(cubeVect); \
				float3 cubeVectNormAbs = abs(cubeVectNorm);\
				half zxlerp = step(cubeVectNormAbs.x, cubeVectNormAbs.z);\
				half nylerp = step(cubeVectNormAbs.y, max(cubeVectNormAbs.x, cubeVectNormAbs.z));\
				half s = lerp(cubeVectNorm.x, cubeVectNorm.z, zxlerp);\
				s = sign(lerp(cubeVectNorm.y, s, nylerp));\
				half3 detailCoords = lerp(half3(1, -s, -1)*cubeVectNorm.xzy, half3(1, s, -1)*cubeVectNorm.zxy, zxlerp);\
				detailCoords = lerp(half3(1, 1, s)*cubeVectNorm.yxz, detailCoords, nylerp);\
				half2 uv = ((.5*detailCoords.yz) / abs(detailCoords.x)) + .5;


			CUBEMAP_DEF(_MainTex_Xn, _MainTex_Xp, _MainTex_Yn, _MainTex_Yp, _MainTex_Zn, _MainTex_Zp)
			CUBEMAP_DEF(_BumpMap_Xn, _BumpMap_Xp, _BumpMap_Yn, _BumpMap_Yp, _BumpMap_Zn, _BumpMap_Zp)

			float _DayLight;
			float _BumpScale;
			float _Shininess;
			fixed4 _Color;
			fixed4 _EmitColor;

			float _SunRadius = 1;
			float3 _SunPos;
			float3 _PlanetOrigin;
			sampler2D _CameraDepthTexture;
			uniform float4x4 _MainRotation;
			uniform float4x4 _ShadowBodies = float4x4
			(	0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0, 0, 0,
				0, 0, 0, 0);

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float3 worldVert : TEXCOORD0;
				float3 L : TEXCOORD1;
				float4 objMain : TEXCOORD3;
				float3 viewDir : TEXCOORD4;
				LIGHTING_COORDS(5,6)
				float4 projPos : TEXCOORD7;
				half3 tspace0 : TEXCOORD8; // tangent.x, bitangent.x, normal.x
				half3 tspace1 : TEXCOORD9; // tangent.y, bitangent.y, normal.y
				half3 tspace2 : TEXCOORD10; // tangent.z, bitangent.z, normal.z
			};


			//Cubemaps
			inline float4 CubeDerivatives(float2 uv, float scale)
			{

				//Make the UV continuous.
				float2 uvS = abs(uv - (.5*scale));

				float2 uvCont;
				uvCont.x = max(uvS.x, uvS.y);
				uvCont.y = min(uvS.x, uvS.y);

				return float4(ddx(uvCont), ddy(uvCont));
			}

			inline half4 GetCubeMap(sampler2D texXn, sampler2D texXp, sampler2D texYn, sampler2D texYp, sampler2D texZn, sampler2D texZp, float3 cubeVect)
			{
				GetCubeCubeUV(cubeVect);

				//this fixes UV discontinuity on Y-X seam by swapping uv coords in derivative calcs when in the X quadrants.
				float4 uvdd = CubeDerivatives(uv, 1);


				half4 sampxn = tex2D(texXn, uv, uvdd.xy, uvdd.zw);
				half4 sampxp = tex2D(texXp, uv, uvdd.xy, uvdd.zw);
				half4 sampyn = tex2D(texYn, uv, uvdd.xy, uvdd.zw);
				half4 sampyp = tex2D(texYp, uv, uvdd.xy, uvdd.zw);
				half4 sampzn = tex2D(texZn, uv, uvdd.xy, uvdd.zw);
				half4 sampzp = tex2D(texZp, uv, uvdd.xy, uvdd.zw);

				half4 sampx = lerp(sampxn, sampxp, step(0, s));
				half4 sampy = lerp(sampyn, sampyp, step(0, s));
				half4 sampz = lerp(sampzn, sampzp, step(0, s));

				half4 samp = lerp(sampx, sampz, zxlerp);
				samp = lerp(sampy, samp, nylerp);
				return samp;
			}


			//Lighting
			inline half4 SpecularColorLight( half3 lightDir, half3 viewDir, half3 normal, half4 color, half4 specColor, float specK, half atten )
			{

				lightDir = normalize(lightDir);
				viewDir = normalize(viewDir);
				normal = normalize(normal);
				half3 h = normalize( lightDir + viewDir );

				half diffuse = dot( normal, lightDir );

				float nh = saturate( dot( h, normal ) );
				float spec = pow( nh, specK ) * specColor.a * (1-color.a);

				half4 c;
				c.rgb = _LightColor0.rgb*((color.rgb * diffuse )+ (specColor.rgb * spec)) * (atten * 2);
				c.a = diffuse*(atten * 2);//_LightColor0.a * specColor.a * spec * atten; // specular passes by default put highlights to overbright
				return c;
			}

			inline half4 DiffuseColorLight( half3 lightDir, half3 normal, half4 color, half atten )
			{
				half diffuse = dot( normal, lightDir );

				half4 c;
				c.rgb = _LightColor0.a*(color.rgb * diffuse ) * (atten * 2);
				c.a = diffuse*(atten * 2);
				return c;
			}
			//Shadows
			//Same eclipse function from scatterer
			half EclipseShadow(float3 worldPos, float lightSourceRadius, float occluderSphereRadius, float3 worldLightPos,float3 occluderSpherePosition)
			{
				float3 lightDirection = float3(worldLightPos - worldPos);
				float3 lightDistance = length(lightDirection);
				lightDirection = lightDirection / lightDistance;

				// computation of level of shadowing w
				float3 sphereDirection = float3(occluderSpherePosition - worldPos);  //occluder planet
				float sphereDistance = length(sphereDirection);
				sphereDirection = sphereDirection / sphereDistance;

				float dd = lightDistance * (asin(min(1.0, length(cross(lightDirection, sphereDirection))))
					- asin(min(1.0, occluderSphereRadius / sphereDistance)));

				float w = smoothstep(-1.0, 1.0, -dd / lightSourceRadius);
				w = w * smoothstep(0.0, 0.2, dot(lightDirection, sphereDirection));

				return (1-w);
			}
			inline half MultiBodyShadow(float3 worldPos, float sunRadius, float3 sunPos, float4x4 m)
			{
				half shadowTerm = 1.0;

				for (int i=0; i<4;i++)
				{
					if (m[i].w == 0.0) break;

					shadowTerm*= EclipseShadow(worldPos, sunRadius, m[i].w, sunPos, m[i].xyz);
				}

				return shadowTerm;
			}


			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.pos = UnityObjectToClipPos(v.vertex);

				float4 vertexPos = mul(unity_ObjectToWorld, v.vertex);
				float3 origin = mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz;
				o.worldVert = vertexPos;
				float3 worldNormal = normalize(vertexPos - origin);
				o.objMain = mul(_MainRotation, v.vertex);
				o.viewDir = normalize(WorldSpaceViewDir(v.vertex));

				half4 tangent = half4(normalize(half3(-worldNormal.z, 0, worldNormal.x)), 1);
				half3 wTangent = tangent.xyz;
				// compute bitangent from cross product of normal and tangent
				half tangentSign = tangent.w * unity_WorldTransformParams.w;
				half3 wBitangent = cross(worldNormal, wTangent) * tangentSign;
				// output the tangent space matrix
				o.tspace0 = half3(wTangent.x, wBitangent.x, worldNormal.x);
				o.tspace1 = half3(wTangent.y, wBitangent.y, worldNormal.y);
				o.tspace2 = half3(wTangent.z, wBitangent.z, worldNormal.z);

				o.projPos = ComputeScreenPos(o.pos);
				COMPUTE_EYEDEPTH(o.projPos.z);
				TRANSFER_VERTEX_TO_FRAGMENT(o);

				o.L = _PlanetOrigin - _WorldSpaceCameraPos.xyz;

				return o;
			}

			struct fout
			{
				half4 color : COLOR;
			};

			fout frag(v2f IN)
			{
				fout OUT;
				half4 color;
				half4 main;
				half4 emit;
				half4 emitTint = _EmitColor;
				half4 specColor = _SpecColor;

				main = GetCubeMap(_MainTex_Xn, _MainTex_Xp, _MainTex_Yn, _MainTex_Yp, _MainTex_Zn, _MainTex_Zp, IN.objMain.xyz);
				emit = GetCubeMap(_BumpMap_Xn, _BumpMap_Xp, _BumpMap_Yn, _BumpMap_Yp, _BumpMap_Zn, _BumpMap_Zp, IN.objMain.xyz);

				half3 tnormal = UnpackScaleNormal(GetCubeMap(_BumpMap_Xn, _BumpMap_Xp, _BumpMap_Yn, _BumpMap_Yp, _BumpMap_Zn, _BumpMap_Zp, IN.objMain.xyz), _BumpScale);
				half3 worldNormal;
				worldNormal.x = dot(IN.tspace0, tnormal);
				worldNormal.y = dot(IN.tspace1, tnormal);
				worldNormal.z = dot(IN.tspace2, tnormal);

				color = _Color * main.rgba;

				//lighting
				half4 scolor = SpecularColorLight(_WorldSpaceLightPos0, IN.viewDir, worldNormal, color, specColor, _Shininess, LIGHT_ATTENUATION(IN));
				emit.rgba = (1-emit.b) * emitTint;
				half4 emitColor = DiffuseColorLight(-_WorldSpaceLightPos0, worldNormal, emit, LIGHT_ATTENUATION(IN));
				scolor.a = 1;

				scolor.rgb *= MultiBodyShadow(IN.worldVert, _SunRadius, _SunPos, _ShadowBodies);
				emitColor = lerp(emitColor, emit, _DayLight);
				OUT.color = max(scolor, emitColor/2);// + emitColor.b;

				OUT.color.a *= step(0, dot(IN.viewDir, worldNormal));

				return OUT;
			}
			ENDCG

		}

	}

}
