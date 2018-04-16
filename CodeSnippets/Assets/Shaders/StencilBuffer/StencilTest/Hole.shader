Shader "Holistic/Hole" {
	Properties
	{
		_MainTex("Diffuse", 2D) = "white" {}
	}
	SubShader{
		Tags
		{
			"RenderType" = "Geometry"
		}

		ColorMask 0
		ZWrite off

		// A stencil buffer contains per-pixel integer data which is used to add more control over which pixels are rendered. 
		// A stencil buffer operates similarly to a depth buffe
		// Enable Stencil buffer to create the mask		
		// Will be the same extension as the frame buffer		
		Stencil 
		{
			Ref 1 // Each pixel in frame buffer will have the same coords in the stencil buffer
			Comp always // Comparision between frame buffer and stencil buffer always
			Pass replace   // Include what this shader has to do with the pixel. Pass == draw call, in this case we will replace anything is in
							// Framebuffer with what we have in this shader
		}

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c= tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
