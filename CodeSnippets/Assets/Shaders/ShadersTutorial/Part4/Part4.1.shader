// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Part4.1" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)		
	}
	SubShader 
	{		
		Pass
		{
			CGPROGRAM

			#pragma vertex VertexProgram
			#pragma fragment FragmentProgram	

			#include "UnityCG.cginc"

			float4 _Color;

			struct VertexData
			{
				float4 position : POSITION; // 3D Position
				float3 normal : NORMAL; // vertex normal coords
				float2 uv : TEXCOORD0; // uv coords

			};
			struct OutputVertexData
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
			};

			OutputVertexData VertexProgram(VertexData input)
			{
				OutputVertexData output;
				//i.position = mul(UNITY_MATRIX_MVP, position); // equivalent to next element
				output.position = UnityObjectToClipPos(input.position);

				// All the normals are in object  space (mesh's local space ), we need to know the surface orientation in world space.
				// The normals need to be transformed from object space into world space
				// We need the object's transformation for this. Unity collapses an object's entire transformation (all the transformations, rotation scale and position)
				// hierarchy into a single transformation matrix: float4x4 unity_ObjectToWorld
				// output.normal = mul((float3x3)unity_ObjectToWorld, input.normal)); The fourth row will be ignored since is position
				// After transform into world space, they need to be normalized: output.normal = normalize(output.normal);
				// This normalization, makes some normals look weird in objects with not uniform scale. For this objects, the normals need to be
				// inverted, but the rotation should be the same.

				// transpose the world-to-object matrix and multiply that with the vertex normal.
				//output.normal = mul(transpose((float3x3)unity_WorldToObject),	input.normal);
				// Normalize normals (uniform scale)
				//output.normal = normalize(output.normal);

				// With UnityObjectToWorldNormal unity does the transpose of the matrix transformation in worldspace
				output.normal = UnityObjectToWorldNormal(input.normal);
					
				return output;
			}

			float4 FragmentProgram(OutputVertexData input) : SV_TARGET
			{
				float4 color = _Color;

				// Visualizing the normals
				input.normal = normalize(input.normal);
				color = float4(input.normal * 0.5 + 0.5, 1);

				return color;
			}

			ENDCG
		}
	}
}
