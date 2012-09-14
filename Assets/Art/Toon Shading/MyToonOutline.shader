Shader "Custom/MyToonOutline" 
{
    Properties 
    { 
    	// ...shader properties... 
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
    }
SubShader {


	
		
    Pass {
    		name "OUTLINE"
    		
    		Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

struct v2f {
    float4 pos : SV_POSITION;
    float3 color : COLOR0;
};

v2f vert (appdata_base v)
{
	
    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    
    float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
    float2 offset = TransformViewToProjection(norm.xy);
    o.pos.xy += offset * 0.015;
    //o.pos.xyz += o.pos.xyz*(0.1,0.1,0.1);
    o.color = (norm.x,norm.y,norm.z,0);
    return o;
}

half4 frag (v2f i) : COLOR
{
    return half4 (i.color, 1);
    //return ((0,0,0,1), 1);
}
ENDCG

    }
    

}
} 