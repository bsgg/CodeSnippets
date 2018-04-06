Shader "Holistic/Hologram" {
	Properties {
		_RimColor ("Rim Color", Color) =  (0,0.5,0.5,0.0)
		_RimPower("Rim Power", Range(0.5,8.0)) = 3.0
	}
	SubShader 
	{
		Tags { "RenderType"="Transparent" }

		// Add pass to write data zbuffer
		Pass
		{
			ZWrite On
			ColorMask 0 // No alpha in this pass
		}

		// First Pass
		CGPROGRAM
		// Physically based Standard lighting Lambert. With alpha there is no data written in zbuffer
		#pragma surface surf Lambert alpha:fade

		sampler2D _MainTex;

		struct Input {
			float3 viewDir;
		};
			
		float4 _RimColor;
		float _RimPower;

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * pow(rim, _RimPower) * 10;
			o.Alpha = pow(rim, _RimPower);

			// Write data in zbuffer to avoid seen geometry inside objects as in the zombiebunny
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}
