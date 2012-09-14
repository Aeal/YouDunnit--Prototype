Shader "Custom/wireFrame" 
{ 
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1) 
	} 
	SubShader 
	{ 
		Pass 
		{
			ZWrite on 
			ZWrite off 
			Blend SrcAlpha OneMinusSrcAlpha
			Colormask RGBA 
			Lighting Off Offset 1, 1 Color[_Color] 
		}
	}
} 
