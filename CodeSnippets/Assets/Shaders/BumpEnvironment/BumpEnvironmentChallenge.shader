Shader "Holistic/BumpEnvironmentChallenge" 
{
	Properties {
		_myBump("Bump Texture", 2D) = "bump" {}
		_myCube("Cube Map", CUBE) = "white" {}
	}
	SubShader {
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _myBump;
		samplerCUBE _myCube;

		struct Input {
			float2 uv_myBump;
			float3 worldRefl; INTERNAL_DATA
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = texCUBE(_myCube, WorldReflectionVector(IN, o.Normal)).rgb;
			o.Normal = UnpackNormal(tex2D(_myBump, IN.uv_myBump)) * 0.3;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
