Shader "Holistic/ColourVF"
{

	SubShader
	{

		Pass // When you use vertex and pixel shader the shader need a Pass
		{
			CGPROGRAM
			#pragma vertex vert // vert name of the vertex shader
			#pragma fragment frag // Frag is the name of the fragment shader

			#include "UnityCG.cginc" // Builting funcitions https://docs.unity3d.com/Manual/SL-BuiltinFunctions.html

			struct appdata
			{
				float4 vertex : POSITION; // Position of each vertex in the 3DWorld
			};

			struct v2f // Vertex to fragmen structure
			{
				float4 vertex : SV_POSITION; // Vertices processes from world space into clipping space
				float4 color: COLOR; 
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				
				// Transforms a point from object space to the camera’s clip space in homogeneous coordinates. 
				// This is the equivalent of mul(UNITY_MATRIX_MVP, float4(pos, 1.0)), and should be used in its place.
				o.vertex = UnityObjectToClipPos(v.vertex);   // Convert world data structure into clippling space structure
				o.color.r = (v.vertex.x + 10)/20; // Change red chanel and get the x
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = i.color;
				return col;
			}
			ENDCG
		}
	}
}
