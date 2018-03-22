Shader "Holistic/Rim" 
{
	Properties
	{
		_Diffuse("Diffuse Texture", 2D) = "white" {}
		_RimColor("Rim Color", Color) =( 0,0.5,0.5,0.0)
		_RimPower("Rim Power", Range(0.5,8.0)) = 3.0
		_StripeWidth("Stripe Width", Range(1,20)) = 10
	}
	
	SubShader {
		CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
			float2 uv_MainTex;
			float3 viewDir; // Get view direction from the world
			float3 worldPos; // Get world position of the pixel that you are about to draw

		};

		// Hold properties
		sampler2D _MainTex;
		float4 _RimColor;
		float _RimPower;
		float _StripeWidth;

		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;

			// Use view direction to calculate  the dot product between normals and viewDir (normalize viewDire first)
			// Reversing the dot, we get the opposite efect. Around the edges (far points from the viewer) we get a bright color
			// In the center of the object we get a darker color.
			// This is what you want in a rim effect, bright in the edges
			half dotValue = dot(normalize(IN.viewDir), o.Normal);
			half rim = 1 - dotValue;
			
			// For the rim effect what we want is moving between 1 to 0,  not between -1 and 1, since -1 is no color. To do this we use saturate
			// Color saturation refers to the intensity of color in an image
			half saturateDotValue = saturate(dotValue);
			rim = 1 - saturateDotValue; // Rim value will be between 1 and 0

			// Include pow we increase the intesity of the rim
			//o.Emission = _RimColor.rgb * pow(rim, _RimPower);

			// Example with stripes
			// frac function gives you anything inside the brackets and gives you the fractional part, or the decimal part
			o.Emission = frac(IN.worldPos.y * (20 - _StripeWidth) * 0.5) > 0.4 ?
							float3(0.2,0,0.8) * rim: float3(1,1,0) * rim;

			
			
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}
