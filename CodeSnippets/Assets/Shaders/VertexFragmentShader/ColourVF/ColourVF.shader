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
			
			v2f vert (appdata v)// This runs in every vertex
			{
				v2f o;
				
				// Transforms a point from object space to the camera’s clip space in homogeneous coordinates. 
				// This is the equivalent of mul(UNITY_MATRIX_MVP, float4(pos, 1.0)), and should be used in its place.
				o.vertex = UnityObjectToClipPos(v.vertex);   // Convert world data structure into clippling space structure
				//o.color.r = (v.vertex.x + 10)/10; // Change red chanel and get the x
				//o.color.g = (v.vertex.z + 10)/10; // Change g chanel and get the z
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target // This runs in every pixel
			{
				fixed4 col = i.color;

				// In this case x and z from vertex is the values accross the screens no the vertex data
				// In this step the vertex are converted
				// Screen space goes from 0 to 1 
				// Adding 10 and divided by 10 it's not doing much 
				//col.r = (i.vertex.x + 10) / 10; 
				//col.g = (i.vertex.z + 10) / 10;

				col.r = (i.vertex.x) / 1000; // What we are changing is the pixels based on screen position not the vertex
				col.g = (i.vertex.y) / 1000;

				return col;
			}
			ENDCG
		}
	}
}
