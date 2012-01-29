
Shader "RenderFX/SkyboxBlender" {
	Properties {
		_Tint ("Tint Color", Color) = (.5, .5, .5, .5)
		_TexMat1 ("Base (RGB)", 2D) = "white" {}
		_TexMat2 ("Base (RGB)", 2D) = "white" {}
		_Blend ("Blend", Range(0.0,1.0)) = 0.0
	}
	
	Category {
		ZWrite On
		Alphatest Greater 0
		Tags {Queue=Transparent}
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
	
		
		
		SubShader {
			Tags { "Queue"="Background" "RenderType"="Background" }
			Cull Off ZWrite Off Fog { Mode Off }
			Color [_Tint]
			Pass {
				SetTexture [_TexMat1] { combine texture }
				SetTexture [_TexMat2] { constantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }
				SetTexture [_TexMat2] { combine previous +- primary, previous * primary }
			}
		}
	}
}