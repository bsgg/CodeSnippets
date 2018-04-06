Shader "Custom/MaskedTexture" 
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Mask("Culling Mask", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.1
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" }
		Lighting Off // No Light
		ZWrite Off // No write in zbuffer

		// SrcAlpha: The value of this stage is multiplied by the source alpha value.
		// OneMinusSrcAlpha: The value of this stage is multiplied by (1 - source alpha).
		Blend SrcAlpha OneMinusSrcAlpha // Traditional blend transparency			
		AlphaTest GEqual[_Cutoff]
		Pass
		{
			SetTexture[_Mask] { combine texture } // Set the mask texture in the frame buffer
			SetTexture[_MainTex]{ combine texture, previous } // Combine MainText with the previous
		}
	}
}
