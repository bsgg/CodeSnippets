Shader "Holistic/BumpDiffuse" 
{
	Properties {
		_Diffuse ("Diffuse Texture", 2D) = "white" {}
		_Bump("Normal Texture", 2D) = "bump" {}
		_BumpSlider("Bumpiness", Range(0,10)) = 1
		_BrightSlider("Brightness", Range(0,10)) = 1
	}
	SubShader {		

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _Diffuse;
		sampler2D _Bump;
		half _BumpSlider;
		half _BrightSlider;

		struct Input {
			float2 uv_Diffuse;
			float2 uv_Bump;
		};
		
		void surf (Input IN, inout SurfaceOutput o) 
		{		
			
			//Performs a texture lookup in sampler samp using coordinate, retrieves texels from a texture
			// Converts the values from texture into albedo
			o.Albedo = tex2D(_Diffuse, IN.uv_Diffuse).rgb;

			// Set the normals texels from the normal and add brightiness
			o.Normal = UnpackNormal(tex2D(_Bump, IN.uv_Bump)) * _BrightSlider;
			// Apply intensity through the slider only in xy
			o.Normal *= float3(_BumpSlider, _BumpSlider, 1);

		}
		ENDCG
	}
	FallBack "Diffuse"
}
