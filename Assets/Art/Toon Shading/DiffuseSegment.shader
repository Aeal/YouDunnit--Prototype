Shader "Custom/DiffuseSegment" 
{
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
CGPROGRAM
#pragma surface surf ToonRamp

sampler2D _Ramp;

// custom lighting function that uses a texture ramp based
// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
{
	#ifndef USING_DIRECTIONAL_LIGHT
	lightDir = normalize(lightDir);
	#endif
	
	half d = dot (s.Normal, lightDir)*0.5 + 0.5;
	half4 c;
	c.rgb = s.Albedo;
	if(d >0.65)
	{

		c.rgb = s.Albedo *_LightColor0.rgb * (0.85,0.85,0.85) * (atten );
	}
	else if(d >0.35)
	{
		c.rgb *= 0.35;

		c.rgb = s.Albedo *_LightColor0.rgb * (0.50,0.50,0.5) * (atten );
	}
	else
	{
		c.rgb *= 0.15;
		c.rgb = s.Albedo *_LightColor0.rgb * (.25,.25,.25) * (atten );
	}

	c.a = 0;
	return c;

}


sampler2D _MainTex;
float4 _Color;

struct Input {
	float2 uv_MainTex : TEXCOORD0;
};

void surf (Input IN, inout SurfaceOutput o) {
	half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG

	} 

	Fallback "Diffuse"
}
