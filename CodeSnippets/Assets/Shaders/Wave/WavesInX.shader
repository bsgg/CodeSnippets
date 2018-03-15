Shader "Holistic/WavesInX" {
    Properties {
      _MainTex("Diffuse", 2D) = "white" {}
      _Tint("Colour Tint", Color) = (1,1,1,1)
      _Freq("Frequency", Range(0,5)) = 3 // How close the waves are together
      _Speed("Speed",Range(0,100)) = 10 // How fast
      _Amp("Amplitude",Range(0,1)) = 0.5 // How height are the waves
    }
    SubShader {
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert 
      
      struct Input {
          float2 uv_MainTex;
          float3 vertColor; // To add the tint
      };
      
	  // Properties
      float4 _Tint;
      float _Freq;
      float _Speed;
      float _Amp;

      struct appdata {
          float4 vertex: POSITION;
          float3 normal: NORMAL;
          float4 texcoord: TEXCOORD0;
          float4 texcoord1: TEXCOORD1;
          float4 texcoord2: TEXCOORD2;
      };
      
      void vert (inout appdata v, out Input o) 
	  {
          UNITY_INITIALIZE_OUTPUT(Input,o);

          float t = _Time * _Speed; // Time and speed _Time variable is unity, similar to Time in Update

		  // Gets the hight of the weight, it works across x direction, it calculates a new y value for each vertex according to the x in time
		  float w0 = sin(t + v.vertex.x * _Freq) * _Amp;
		  float w1 = sin(t*2 + v.vertex.x * _Freq*2) * _Amp;
		  float w2 = cos(t + v.vertex.x * _Freq) * _Amp;
		  float waveHeight = w0 + w1 + w2;

          v.vertex.y = v.vertex.y + waveHeight; // Modify each y coordinate of the vertex for that height (offset)

          v.normal = normalize(float3(v.normal.x + waveHeight, v.normal.y, v.normal.z));

          o.vertColor = waveHeight + 2; // Vertex color, using the waveHeight to match the color

      }

      sampler2D _MainTex;
      void surf (Input IN, inout SurfaceOutput o) {
          float4 c = tex2D(_MainTex, IN.uv_MainTex);
          o.Albedo = c * IN.vertColor.rgb;
      }
      ENDCG

    } 
    Fallback "Diffuse"
  }