// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


// Reference: http://catlikecoding.com/unity/tutorials/rendering/part-2/

Shader "Custom/ShaderTutorial1"
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
			struct Interpolators 
			{
				float4 position : SV_POSITION;
				float3 localPosition : TEXCOORD0;
			};


			// Vertex program (vertices data int display space)
			// Returns a final coordinates of a vertex (4 coordinates since is a 4x4 transformation matrix to transform 
			// all the vertices from the mesh into display space
			// SV_POSITION indicates that is a position information what we are returning in float (output position of the vertex)
			// The parameter position, is the objcet-space position of the vertex. It will be automatic vertex position given from the mesh

			// localPosition: Localposition for the fragment, it has to be TEXCOORD0 as well (important!)
			/*float4 VertexProgram(
				float4 position : POSITION,
				out float3 localPosition : TEXCOORD0
				) : SV_POSITION
			{
				
				// Gets position from each vertex
				localPosition = position.xyz;
				
				// Returns this same position. At this point this function will convert each position of each vertex in display space 
				// (from object space into display space)
				// If we return the position as it is from the mesh (the source) it will be distorted, because we are using boject-space positions as
				// if they were display positions. Rotate, moving or scale won't have any effect.
				// return position;

				// To get the correct position information we need to multiply each vertex position by the camera transformation and projection
				// UNITY_MATRIX_MVP is a 4x4 matrix
				// return mul(UNITY_MATRIX_MVP, position); UNITY WILL CONVERT THIS INTO UnityObjectToClipPos(position);
				return  UnityObjectToClipPos(position);
			}*/

			// Vertexprogram with output structure
			Interpolators VertexProgram(float4 position : POSITION) 
			{
				Interpolators output;
				output.localPosition = position.xyz;
				//i.position = mul(UNITY_MATRIX_MVP, position);
				output.position = UnityObjectToClipPos(position);
				return output;
			}



			// Fragment Program (Coloring indivdiual pixels inside mesh's triangles)
			// Returns a RGBA color for on pixel (SV_TARGET, output default shader target) This is the frame buffer which contains the final
			// image that we are generating
			// The input parameter is the output of the VertexProgram. They have to match

			// Param localPosition: The processed vertex data isn't used directly from the fragmentProgram. Before that, 
			// it is interpolated against mesh triangles. It takes three processed vertices and interpolates between them
			// For every pixel covered by th trinagle, it invokes the gragment program, passing along the interpolated data.
			// This localPosition param it will be the interpolated local position from the vertexprogram, and it will be used as a return
			// for the final color. The type of data is TEXCOORD0 used in interpolated elements (apart from textures)
			/*float4 FragmentProgram(
				float4 position: SV_POSITION,
				float3 localPosition: TEXCOORD0) : SV_TARGET
			{
				// Giving a fixed color
				//return float4(0, 1, 0, 1);
				//return _Tint;

				// Returns the position as a color (XYZ, the fourth param is just 1)
				// This will produce a rainbow RGB colour according to the interpolated position between triangles
				return float4(localPosition,1);
			}*/


			// Fragment function with interpolation param 
			float4 FragmentProgram(Interpolators input) : SV_TARGET
			{				
				//return float4(input.localPosition,1);
				// Move the values between 0-1 range and tint with property _Tint
				// Without 0.5 some positions will end up between -1/2 or +1/2
				return float4(input.localPosition + 0.5, 1) * _Tint;
			}

			ENDCG

		}
	}
}
