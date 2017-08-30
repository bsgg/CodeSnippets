


Shader "Custom/ShaderTutorial2.1" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_DetailTex("Detail Texture", 2D) = "gray" {}
	}

	SubShader 
	{
		Pass
		{
			// Shader unity language variant of HLSL and CG
			CGPROGRAM

			// Vertex program: Responsible for processing the vertex data of a mesh.
			// This includes the conversion from object space to display space
			// Fragment program: Responsible for coloring individual pixels that lie inside the mesh's triangles.
			#pragma vertex VertexProgram
			#pragma fragment FragmentProgram

			#include "UnityCG.cginc"

			// Property Tint (defined in properties section)
			float4 _Color;

			// Access to MainText property with sampler2D
			sampler2D _MainTex;

			// Extra data of a texture (tiling and offset)
			float4 _MainTex_ST;

			// Detailed texture
			sampler2D _DetailTex;
			float4 _DetailTex_ST;

			// Structure for vertexprogram
			struct VertexData
			{
				float4 position : POSITION; // 3D Position
				float2 uv : TEXCOORD0;  //UV coordinates in each vertice
				
			};


			struct Interpolators
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvDetail : TEXCOORD1;  //UV coordinates in each vertice
			};


			// Vertexprogram with output structure
			// Vertex program (vertices data int display space)
			// vi: UV coordinates in each vertice
			Interpolators VertexProgram(VertexData input)
			{
				Interpolators output;
				//i.position = mul(UNITY_MATRIX_MVP, position); // equivalent to next element
				output.position = UnityObjectToClipPos(input.position);

				// The tiling vector is used to scale the texture, it's (1,1) by default. It's stored in the XY
				// To change the uv of each vertex with tiling (scale uv vertices). We need to multiply that for the tiling info

				// The offset portion mmoves the texture around and is stored in the ZW portion of the variable
				output.uv = TRANSFORM_TEX(input.uv, _MainTex); 
				
				// Interpolate uv for detailed texture
				output.uvDetail = TRANSFORM_TEX(input.uv, _DetailTex);

				return output;
			}


			// Fragment Program (Coloring indivdiual pixels inside mesh's triangles)
			// Fragment function with interpolation param 
			float4 FragmentProgram(Interpolators input) : SV_TARGET
			{
				// Sampling the texture with the UV coordinates with tex2D function
				// Tint the final color with _Tint property
				float4 color = tex2D(_MainTex, input.uv) * _Color;
				//color *= tex2D(_MainTex, input.uv * 10) * 2;

				// Include detailed texture
				color *= tex2D(_DetailTex, input.uvDetail) * unity_ColorSpaceDouble;
				return color;
			}

		ENDCG
		}
	}

}
