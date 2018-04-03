Shader "Holistic/BasicLightLambert" 
{
	Properties
	{
		_Colour("Colour", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Geometry"
		}

		CGPROGRAM
		#pragma surface surf BasicLambert
		
		half4 LightingBasicLambert(SurfaceOutput s, half3 lightDir, half atten)  // Name LightingBasicLambert has to be the same as #pragma surface surf BasicLambert with Lighting
		{
			half NdotL = dot(s.Normal, lightDir); // Dot product between surface normal and vector light director
			
			// Calculate final color
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten); // Albedo color * light color * dot product light * light intensity
			//c.rgb = s.Albedo * (NdotL * atten);
			c.a = s.Alpha;
			return c;
		}

		float4 _Colour;

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = _Colour.rgb;
		}
		ENDCG
	}

	FallBack "Diffuse"
}