Shader "Holistic/RimDiffuse" 
{
	Properties
	{
		_Diffuse("Diffuse Texture", 2D) = "white" {}
		_Bump("Normal Texture", 2D) = "bump" {}
		_BumpSlider("Bumpiness", Range(0,10)) = 1
		_RimColor("Rim Color", Color) =( 0,0.5,0.5,0.0)
		_RimPower("Rim Power", Range(0.5,8.0)) = 3.0
	}
	
	SubShader {
		CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
			float2 uv_Diffuse;
			float2 uv_Bump;
			float3 viewDir; // Get view direction from the world
		};

		// Hold properties
		sampler2D _Diffuse;
		sampler2D _Bump;
		half _BumpSlider;
		float4 _RimColor;
		float _RimPower;

		void surf(Input IN, inout SurfaceOutput o)
		{
			// Albedo color
			o.Albedo = tex2D(_Diffuse, IN.uv_Diffuse).rgb;

			// Normals
			o.Normal = UnpackNormal(tex2D(_Bump, IN.uv_Bump));
			
			o.Normal *= float3(_BumpSlider, _BumpSlider, 1);


			// Use view direction to calculate  the dot product between normals and viewDir (normalize viewDire first)
			// Reversing the dot, we get the opposite efect. Around the edges (far points from the viewer) we get a bright color
			// In the center of the object we get a darker color.
			// This is what you want in a rim effect, bright in the edges
			half dotValue = dot(normalize(IN.viewDir), o.Normal);
			half rim = 1 - dotValue;
			
			// For the rim effect what we want is moving between 1 to 0,  not between -1 and 1, since -1 is no color. To do this we use saturate
			// Color saturation refers to the intensity of color in an image
			half saturateDotValue = saturate(dotValue);
			rim = 1 - saturateDotValue;

			// Include pow we increase the intesity of the rim
			o.Emission = _RimColor.rgb * pow(rim, _RimPower);
			
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}
