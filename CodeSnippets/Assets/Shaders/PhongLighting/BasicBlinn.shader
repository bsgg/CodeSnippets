Shader "Holistic/BasicBlinn" {
	Properties {
		_Colour("Colour", Color) = (1,1,1,1)
		_SpecColor("SpecularColour", Color) = (1,1,1,1)
		_Spec("Specular", Range(0,1)) = 0.5
		_Gloss("Gloss", Range(0,1)) = 0.5

	}
	SubShader {
		Tags { "Queue"="Geometry" }

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf BlinnPhong

		float4 _Colour;
		half _Spec;
		fixed _Gloss;

		struct Input {
			float2 uv_MainTex;
		};


		void surf (Input IN, inout SurfaceOutput o) {
			
			o.Albedo = _Colour.rgb;
			o.Specular = _Spec;
			o.Gloss = _Gloss;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
