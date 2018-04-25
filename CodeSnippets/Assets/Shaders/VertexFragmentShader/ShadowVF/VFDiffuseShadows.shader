Shader "Holistic/VFDiffuseShadows" {
	Properties 
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader 
	{
		Pass // First pass with the light (1 Draw Call)
		{
			// This setups forward renderer so the lights are calculated by model, not at the end like in deferred lighting
			Tags{ "LightMode" = "ForwardBase" } 

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc" 
			#include "Lighting.cginc" 
			#include "AutoLight.cginc"

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
				float4 pos : SV_POSITION;
				SHADOW_COORDS(1)
			};

			v2f vert(appdata v)
			{
				v2f o;

				// Convert vertices from the model into clip space position
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.textcoord; // Get texture coordinates
				
				// Calculate world normal, from object to world normal (real coords in real world)
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);

				// Get the dot product between the normal and world space light position it gets from UnityLightingCommon
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));

				// The diffuse color is the normal and the light color
				o.diff = nl * _LightColor0;

				TRANSFER_SHADOW(o)

				return o;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed shadow = SHADOW_ATTENUATION(i); // Get the shadow
				col.rgb *= i.diff * shadow;

				return col;
			}
			ENDCG
		}	

		// Second pass for the shadows (2 draw call) This pass only takes the geometry from the model and  cast a shadow.
				// The final pass is a shadow
		Pass 
			{
				Tags{ "LightMode" = "ShadowCaster" } // Cast shadows

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_shadowcaster // Include shadows
				#include "UnityCG.cginc"

				struct appdata 
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 texcoord : TEXCOORD0;
				};

				struct v2f {
					V2F_SHADOW_CASTER; // v2f only has shadows. 
				};

				v2f vert(appdata v)
				{
					v2f o;
					TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
					return o;
				}

				float4 frag(v2f i) : SV_Target
				{
					SHADOW_CASTER_FRAGMENT(i)
				}
				ENDCG
			}

	}
	FallBack "Diffuse"
}
