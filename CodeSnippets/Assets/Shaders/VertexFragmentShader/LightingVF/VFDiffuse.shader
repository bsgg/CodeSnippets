Shader "Holistic/VFDiffuse" {
	Properties 
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader 
	{
		Pass
		{
			// This setups forward renderer so the lights are calculated by model, not at the end like in deferred lighting
			Tags{ "LightMode" = "ForwardBase" } 

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc" 

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 textcoord : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 diff : COLOR0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;

				// Convert vertices from the model into clip space position
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.textcoord; // Get texture coordinates
				
				// Calculate world normal, from object to world normal (real coords in real world)
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);

				// Get the dot product between the normal and world space light position it gets from UnityLightingCommon
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));

				// The diffuse color is the normal and the light color
				o.diff = nl * _LightColor0;

				return o;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// Get the color from the texture and multiply by the diffuse of the light
				col *= i.diff;

				return col;
			}
			ENDCG
		}	
	}
	FallBack "Diffuse"
}
