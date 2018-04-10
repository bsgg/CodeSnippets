Shader "Holistic/TransparentBlend" {
	Properties{
		_MainTex("Texture", 2D) = "black" {}
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" } // Transparent queu

		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off // Controls which sides of polygons should be culled (not drawn) Off Disables culling - all faces are drawn. Used for special effects.
		// 1 Pass
		Pass
		{
			// Replace the pixel in the frame buffer with whatever is in our texture
			SetTexture[_MainTex]{ combine texture }
		}
	}
}
