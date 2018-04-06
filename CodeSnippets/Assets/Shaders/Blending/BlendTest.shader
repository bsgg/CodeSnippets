Shader "Holistic/BlendTest" {
	Properties {
		_MainTex ("Texture", 2D) = "black" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" } // Transparent queu

		// Doc https://docs.unity3d.com/Manual/SL-Blend.html
		// Test Blend 1
		//Blend One One // Additive Blend multiply 1 by the pixel color and multiply by 1. Adds 1 color to the other color that comes later
		
		// Test Blend 2
		// Whatever the alpha of the source image and the current color of the frame buffer 
		// Anything that is not alpha on the source, will stay with no alpha. Anything with alpha on the source will be blend with 
		// the frame buffer
		// This transparency blend
		//Blend SrcAlpha OneMinusSrcAlpha 

		// Test Blend 3: (Soft additive) Takes incoming color or color that is already in frame buffer and it will be multiply by 0 (so its discarded)
		// DstColor: The value of this stage is multiplied by frame buffer source color value.	
		// Zero: The value zero - use this to remove either the source or the destination values.
		// For example if we have a black texture with some white elements:
		// it will multiply the white elements wich has value 1, will multiply by what it's already in the frame buffer (something behind), so both together will  blend and we will have "transparency or color depending what is behind")
		// When the pixel is black will multiply by the incoming color (whatever is behind) and will give us 0 or black in this case
		//Blend DstColor Zero

		// Test Blend 4: Reverse the effect from previous with a texture black with painted elements in white
		// Elements in black will be a blending (or transparent) and elements in white will be white
		Blend One One

		// 1 Pass
		Pass
		{
			// Replace the pixel in the frame buffer with whatever is in our texture
			SetTexture [_MainTex] { combine texture}
		}
	}
}
