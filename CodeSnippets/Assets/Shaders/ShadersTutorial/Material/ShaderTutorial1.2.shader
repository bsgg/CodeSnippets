// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


// Reference: http://catlikecoding.com/unity/tutorials/rendering/part-2/

Shader "Custom/ShaderTutorial1.2"
{
	// Shader properties
	Properties
	{
		// Tint or color property for the shader
		_Tint("Tint", Color) = (1, 1, 1, 1)		
	}

	//  SubShader: group multiple shader variants 
	//  It could have one sub-shader for desktops and another for mobiles. 
	SubShader
	{
		// A shader pass is where an object actually gets renderer
		// It's possible to have more than 1
		// More than 1 pass, the object gets rendered multiple times (required for a lot of effects)
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
			float4 _Tint;

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
			};
			

			// Vertexprogram with output structure
			// Vertex program (vertices data int display space)
			// vi: UV coordinates in each vertice
			Interpolators VertexProgram(VertexData input)
			{
				Interpolators output;				
				//i.position = mul(UNITY_MATRIX_MVP, position); // equivalent to next element
				output.position = UnityObjectToClipPos(input.position);

				output.uv = input.uv;
				return output;
			}


			// Fragment Program (Coloring indivdiual pixels inside mesh's triangles)
			// Fragment function with interpolation param 
			float4 FragmentProgram(Interpolators input) : SV_TARGET
			{				
				// Return uv coordinates as colors
				// For example, U becomes red, V becomes green, while blue is always 1.
				return float4(input.uv, 1, 1);
			}

			ENDCG

		}
	}
}
