// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Part4.2" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)		
		_MainTex("Albedo", 2D) = "white" {}
	}
	SubShader 
	{		
		Pass
		{
			
			Tags
			{
				// Type of light mode, in this case is forward rendering (default one)
				"LightMode" = "ForwardBase"
			}

			CGPROGRAM

			#pragma vertex VertexProgram
			#pragma fragment FragmentProgram	

			//#include "UnityCG.cginc"
			#include "UnityStandardBRDF.cginc"

			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

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

				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
					
				return output;
			}

			float4 FragmentProgram(OutputVertexData input) : SV_TARGET
			{
				float4 color = _Color;

				// Visualizing the normals
				input.normal = normalize(input.normal);

				// Get the position of the current light and return with the dot product of the input
				//float3 lightDir = _WorldSpaceLightPos0.xyz;
				//return DotClamped(lightDir, input.normal);

				// Ge lightDir and light color of current light
				float3 lightDir = _WorldSpaceLightPos0.xyz;
				float3 lightColor = _LightColor0.rgb;

				// Calculate the intensity of that light and get the final diffuse color
				float intensityLight = DotClamped(lightDir, input.normal);
				//float3 diffuseColor = lightColor * intensityLight;
				//return float4(diffuseColor, 1);

				// Include albedo = Albedo is Latin for whiteness. So it describes how much of the red, green, and blue color channels are diffusely reflected
				float3 albedo = tex2D(_MainTex, input.uv).rgb * _Color.rgb;
				float3 diffuseColor = albedo * lightColor * intensityLight;

				return float4(diffuseColor, 1);
				
			}

			ENDCG
		}
	}
}
