Shader "Holistic/PropChallenge4" 
{
    Properties {
        _diffuse ("Diffuse", 2D) = "white" {}
		_emissive("Emissive", 2D) = "white" {}
    }
    SubShader {

      CGPROGRAM
        #pragma surface surf Lambert
        
        sampler2D _diffuse;
		sampler2D _emissive;

        struct Input {
            float2 uv_main;

        };
        
        void surf (Input IN, inout SurfaceOutput o) {

            o.Albedo = tex2D(_diffuse, uv_main).rgb;
			//o.Emission = tex2D(_emissive, IN.uv).rgb;
        }
      
      ENDCG
    }
    Fallback "Diffuse"
  }
