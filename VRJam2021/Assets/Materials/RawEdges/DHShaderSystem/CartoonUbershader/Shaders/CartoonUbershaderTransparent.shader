Shader "DHShaders/Cartoon/CartoonUbershader Transparent"
{
	Properties
	{
		[Space()]
		[Header(Main)]
		[HDR]
		_Color("Diffuse Color", Color) = (0.25, 0.25, 0.25, 1)
		_MainTex("Texture (RGB)", 2D) = "white" {}

		_NormalTex("Normalmap", 2D) = "bump" {}
		_NormalPower("Normalmap Power", Range(0, 4)) = 1
		_OmniLightPower("Omni Light Power", Range(0, 2)) = 1
		_SpotLightPower("Spot Light Power", Range(0, 2)) = 1

		[Space(12)]
		[Header(Shading)]
		_Shading("Shading", Float) = 1
		[HDR]
		_ShadeColor("Shade Color", Color) = (0, 0, 0, 1)
		_ShadeSize("Shade Size", Range(0.0, 1.0)) = 0
		[HDR]
		_ShadeOutlineColor("Shadow Outline Color", Color) = (0, 0, 0, 1)
		_ShadeOutlineSize("Shade Outline Size", Range(0, 0.99)) = 0


		[Space(12)]
		[ToggleHeaderDrawer(Specular, SPECULAR, 202)]
		_Specular("Specular", Float) = 0
		[CheckToggle(SPECULAR)]
		_SpecularTex("Specular Texture (RGB)", 2D) = "white" {}
		[HDR]
		[CheckToggle(SPECULAR)]
		_SpecularColor("Specular Color", Color) = (1, 1, 1, 1)
		[CheckToggle(SPECULAR)]
		_SpecularSize("Specular Size", Range(0.0, 1.0)) = 0.1
		[CheckerToggleKeywordEnum(SPECULAR, Standart, Anisotropy, Subtractive)]
		_SpecularMode("Specular Mode", Float) = 0
		[CheckerToggleKeywordEnum(SPECULAR, Standart, Disabled)]
		_SpecularTransparency("Specular Transparency", Float) = 0
		[CheckToggle(SPECULAR)]
		_AnisotropyPower("Anisotropic Power", Range(-1, 1)) = 0
		[CheckToggle(SPECULAR)]
		_AnisotropyDirection("Anisotropic Direction", Range(-1, 1)) = 0



		[Space(12)]
		[ToggleHeaderDrawer(Emission, EMISSION, 108)]
		_Emission("Emission", Float) = 0
		[HDR]
		[CheckToggle(EMISSION)]
		_EmissionColor("Emission Color", Color) = (0, 0, 0, 1)
		[CheckToggle(EMISSION)]
		_EmissionTex("Emission Texture (RGB)", 2D) = "black" {}


		[Space(12)]
		[ToggleHeaderDrawer(Rim, RIM, 60)]
		_Rim("Rim", Float) = 0
		[HDR]
		[CheckToggle(RIM)]
		_RimColor("Rim Color", Color) = (0.26, 0.19, 0.16, 1)
		[CheckToggle(RIM)]
		_RimSize("Rim Size", Range(0.01, 1.0)) = 0.2
	}
		SubShader
		{
			Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}

			CGPROGRAM
			#pragma surface surf Cartoon nolightmap fullforwardshadows alpha:fade
			#pragma target 3.0

			#pragma multi_compile _ SPECULAR
			#pragma multi_compile _SPECULARMODE_STANDART _SPECULARMODE_ANISOTROPY _SPECULARMODE_SUBTRACTIVE
			#pragma multi_compile _SPECULARTRANSPARENCY_STANDART _SPECULARTRANSPARENCY_DISABLED

			#pragma multi_compile _ RIM
			#pragma multi_compile _ EMISSION

			struct CartoonSurfaceOutput
			{
				fixed3 Albedo;
				fixed3 Normal;

			#ifdef SPECULAR
				fixed3 Specular;
			#endif

				fixed3 Emission;
				fixed  Alpha;
			};

			struct Input
			{
				float2 uv_MainTex;

			#ifdef SPECULAR
				float2 uv_SpecularTex;
			#endif

				float2 uv_EmissionTex;
				float2 uv_NormalTex;
				float3 viewDir;

				float4 color : COLOR;
			};

			float4    _Color;
			sampler2D _MainTex;
			sampler2D _NormalTex;
			float _NormalPower;
			float _OmniLightPower;
			float _SpotLightPower;


			float4 _ShadeColor;

		#ifdef SPECULAR
			sampler2D _SpecularTex;
			float4    _SpecularColor;
			float     _SpecularSize;

			#ifdef _SPECULARMODE_ANISOTROPY
				float _AnisotropyPower;
				float _AnisotropyDirection;
			#endif

		#endif

			sampler2D _EmissionTex;
			float4    _EmissionColor;

			float _ShadeSize;

			float4 _RimColor;
			float _RimSize;

			float4 _ShadeOutlineColor;
			float  _ShadeOutlineSize;

			half4 LightingCartoon(CartoonSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half4 c;
				half3 h = normalize(lightDir + viewDir);
				half diff = saturate(dot(s.Normal, lightDir));

	#if !defined(POINT) && !defined(SPOT)

				bool isShade = (bool)(_ShadeSize < diff);

				if (isShade == 0)
				{
					atten = 0;
				}
				else
				{
					atten -= pow(1 - diff, 3);
				}

		#ifdef SPECULAR
				#ifdef _SPECULARMODE_ANISOTROPY
					fixed HdotA = dot(s.Normal, h);
					float aniso = max(0, sin(radians((HdotA + _AnisotropyDirection) * 180) - _AnisotropyPower));

					bool isSpecular = (bool)((1.0 - _SpecularSize) < diff && atten > 0.5) * aniso;
				#else
					bool isSpecular = (bool)((1.0 - _SpecularSize) < diff && atten > 0.5);
				#endif
		#endif

				float shadowOutline = 0;

				if (diff == 0)
					atten = 0;

				if (atten > 0.1 && atten < _ShadeOutlineSize && diff > 0)
					shadowOutline = 1;

				atten = (atten > 0.25);

				float atten3 = float3(1, 1, 1);

				if (atten < 1)
				{
					atten3 = _ShadeColor.rgb;
				}

				c.rgb = s.Albedo * _LightColor0.rgb;

		#ifdef SPECULAR
				#if defined(_SPECULARMODE_STANDART) || defined(_SPECULARMODE_ANISOTROPY)
					#ifdef _SPECULARTRANSPARENCY_STANDART
						c.rgb += s.Specular * _SpecularColor.rgb * isSpecular;
					#endif
					#ifdef _SPECULARTRANSPARENCY_DISABLED
						c.rgb += s.Specular * _SpecularColor.rgb * isSpecular * (1 / (_Color.a + (0.00001)));
					#endif
				#endif

				#ifdef _SPECULARMODE_SUBTRACTIVE
					c.rgb -= s.Specular * _SpecularColor.rgb * isSpecular;
				#endif
		#endif
				c.rgb = lerp((c.rgb) * atten3, _ShadeOutlineColor, shadowOutline);

		#if defined(RIM) || defined(EMISSION)
				c.rgb += s.Emission.rgb;
		#endif

				c.a = s.Alpha;

				return c;
	#else
				half NdotL = saturate(dot(s.Normal, lightDir));

		#ifdef POINT
				atten = length(_LightColor0.rgb) * (atten > 0);
		#elif SPOT
				atten = length(_LightColor0.rgb) * (atten > 0.01);
		#endif

		#ifdef SPECULAR
			#ifdef _SPECULARMODE_ANISOTROPY
						fixed HdotA = dot(s.Normal, h);
						float aniso = max(0, sin(radians((HdotA + _AnisotropyDirection) * 180) - _AnisotropyPower));

						bool isSpecular = (bool)((1.0 - _SpecularSize) < diff && atten > 0.5) * aniso;
			#else
						bool isSpecular = (bool)((1.0 - _SpecularSize) < diff && atten > 0.5);
			#endif
		#endif

				c.rgb = normalize(_LightColor0.rgb);

		#ifdef SPECULAR
				#ifdef _SPECULARTRANSPARENCY_STANDART
					c.rgb += s.Specular * _SpecularColor.rgb * isSpecular;
				#endif
				#ifdef _SPECULARTRANSPARENCY_DISABLED
					c.rgb += s.Specular * _SpecularColor.rgb * isSpecular * (1 / (_Color.a + (0.00001)));
				#endif
		#endif

				c.rgb = c.rgb * ((NdotL >= 0) * atten);

		#ifdef POINT
				c.rgb *= _OmniLightPower;
		#elif SPOT
				c.rgb *= _SpotLightPower;
		#endif

				c.a = s.Alpha;
				return c;
	#endif
			}

			void surf(Input IN, inout CartoonSurfaceOutput o)
			{
				float4 col = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = col.rgb * _Color * IN.color.rgb * unity_ColorSpaceDouble.x;

		#ifdef SPECULAR
				o.Specular = tex2D(_SpecularTex, IN.uv_SpecularTex).rgb;
		#endif

				o.Normal = UnpackNormalWithScale(tex2D(_NormalTex, IN.uv_NormalTex), _NormalPower);

		#ifdef EMISSION
				o.Emission = tex2D(_EmissionTex, IN.uv_EmissionTex).rgb + _EmissionColor.rgb;
		#endif

		#ifdef RIM
				half rim = saturate(dot(normalize(IN.viewDir), o.Normal));
				o.Emission += (_RimColor.rgb * (rim < _RimSize));
		#endif

				o.Alpha = col.a * _Color.a;
			}
			ENDCG
		}
}
