


Shader "Custom/ShaderTutorial2.2" 
{
	Properties 
	{
		_MainTex ("Splat Map", 2D) = "white" {}
		[NoScaleOffset]_Texture1("Texture 1", 2D) = "white" {} // [NoScaleOffset] no tiling or offset allow
		[NoScaleOffset]_Texture2("Texture 2", 2D) = "white" {}
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

			// Access to MainText property with sampler2D  Extra data of a texture (tiling and offset)
			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _Texture1, _Texture2;


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
				float2 uvSplat : TEXCOORD1;  //UV coordinates in each vertice
			};


			// Vertexprogram with output structure
			// Vertex program (vertices data int display space)
			// vi: UV coordinates in each vertice
			Interpolators VertexProgram(VertexData input)
			{
				Interpolators output;
				//i.position = mul(UNITY_MATRIX_MVP, position); // equivalent to next element
				output.position = UnityObjectToClipPos(input.position);
				// The offset portion mmoves the texture around and is stored in the ZW portion of the variable
				output.uv = TRANSFORM_TEX(input.uv, _MainTex); 

				// Pass to the fragment shader the unmodified UV from teh vertex program
				output.uvSplat = input.uv;

				return output;
			}


			// Fragment Program (Coloring indivdiual pixels inside mesh's triangles)
			// Fragment function with interpolation param 
			float4 FragmentProgram(Interpolators input) : SV_TARGET
			{
				//sample the splat map before sampling the other textures.
				float4 splat = tex2D(_MainTex, input.uvSplat);				

				// Splat texture is monocrome. It will be black or white (1 or 0) so we can select any channel from the splat color (rgb)
				// they will have the same values

				// We want to select 1 of the textures according to splat
				// If splat.r == 0 no select the texture. If splat.r == 1 select the texture
				
				// ( 1-splat.r) means the opposite to the first result. 1 - 0 == 1. 1-1 == 0
				float4 color1 = tex2D(_Texture1, input.uv) * splat.r;
				float4 color2 = tex2D(_Texture2, input.uv) * (1 - splat.r);

				// Add both colors
				float4 color = color1 + color2;

				return color;
			}

		ENDCG
		}
	}

}
