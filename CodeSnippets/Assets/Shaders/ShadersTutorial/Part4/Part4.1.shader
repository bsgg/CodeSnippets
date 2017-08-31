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

				// Transform normales from object space into world space
				// transpose the world-to-object matrix and multiply that with the vertex normal.
				output.normal = mul(transpose((float3x3)unity_WorldToObject),
					input.normal);
				// Normalize normals (uniform scale)
				output.normal = normalize(output.normal);
					
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
