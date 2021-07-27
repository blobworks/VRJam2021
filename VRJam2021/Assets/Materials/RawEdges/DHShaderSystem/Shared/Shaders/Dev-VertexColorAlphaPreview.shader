Shader "DHShaders/Dev/VertexColor-Alpha Preview"
{
	Properties
	{
		[KeywordEnum(Color, Alpha)] _Mode("Show vertex", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _MODE_COLOR _MODE_ALPHA

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color  = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				#if _MODE_COLOR
					return fixed4(i.color.rgb, 1);
				#endif

				#if _MODE_ALPHA
					return fixed4(i.color.aaa, 1);
				#endif
			}
			ENDCG
		}
	}
}
