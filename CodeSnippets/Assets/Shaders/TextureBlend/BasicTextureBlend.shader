Shader "Holistic/BasicTextureBlend" {
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	    _DecalTex("Decal", 2D) = "white" {}
	    [Toggle] _ShowDecal("Show Decal?", Float) = 0 // Toggle for the decal
	}
	SubShader{
		Tags
		{
			"RenderType" = "Geometry"
		}

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _DecalTex;
		float _ShowDecal;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 a = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 b = tex2D(_DecalTex, IN.uv_MainTex) * _ShowDecal; // Multipling the variable show decal will toggle the texture

			// Check if the current pixel of b is almost white, we will use the color from the second image
			// Otherwise we will use the first one
			o.Albedo = (b.r > 0.9) ? b.rgb : a.rgb;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
