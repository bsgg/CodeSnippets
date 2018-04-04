Shader "Unlit/DrawTracks"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Coordinate("_Coordinate", Vector) = (0,0,0,0)
		_Color("Draw Color", Color) = (1,0,0,0)
		_Size("Size", Range(1,500)) = 1
		_Strength("Strength", Range(0,1)) = 1
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
			
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Coordinate;
			fixed4 _Color;
			half _Size;
			half _Strength;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// We want to spawn a brush where we have clicked on our mesh once we click the mesh
				// this shader is being drawn onto the render texture
				// The uv coordinates are always between 0 and 1
				// Anything done in the frag program will be calcluated on each pixel 
				// If we do a distance check between teh current pixel and the coordinates we know which pixel is at 
				// the same place as teh coordinates
				// As we need the color we need to invert the outcome of that operation
				// Saturate is the same as clamp to 1 to 0 in c#

				// _Size Is for the brush size, we have to invert the value, to get the less size the smaller the brush
				float draw = pow (saturate(1 - distance(i.uv, _Coordinate.xy)),(500 / _Size));
				// Strenght of the brush
				fixed4 drawcol = _Color * (draw * _Strength);
				return saturate(col + drawcol);

			}
			ENDCG
		}
	}
}
