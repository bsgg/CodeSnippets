Shader "Holistic/DotProduct" 
{
	
	SubShader {
		CGPROGRAM
		#pragma surface surf Lambert
		struct Input {
			float3 viewDir; // Get view direction from the world
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			half dotp = dot(IN.viewDir, o.Normal); // Use view direction to calculate  the dot product between normals and viewDir

			//half dotp = 1 - dot(IN.viewDir, o.Normal); // The other effect

			
			// Use that as a Albedo						   
			// The white areas are the ones with the dot == 1 (the albedo will be 0.9,1,1 which is white)
			// When the dot product is close to 0, (or the dot product is smaller towards the edges)   (The albedo will be (0.2,1,1))
			// The blue element of albedo will be , and the other 2 channels will predominate over this one (blending between green and blue) 
			o.Albedo = float3(dotp, 1,1); 
			
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}
