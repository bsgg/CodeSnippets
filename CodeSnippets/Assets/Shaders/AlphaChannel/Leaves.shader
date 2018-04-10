Shader "Holistic/Leaves" {
	Properties {
		_MainTex ("MainTex", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
 
		CGPROGRAM
		// Physically based Standard lighting Lambert, include alpha:fade the alpha is enabled. Alpha doesn't write into zbuffer, so sometimes you can have z-fighting
		#pragma surface surf Lambert alpha:fade

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};
			

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;

			// Include alpha
			o.Alpha = c.a;
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}
