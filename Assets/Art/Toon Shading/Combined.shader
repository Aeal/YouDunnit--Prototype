Shader "Custom/Combined" 
{
    Properties 
    { 
    	// ...shader properties... 
    	_Color ("Main Color", Color) = (1,1,1,0.5)
        _SpecColor ("Spec Color", Color) = (1,1,1,1)
        _Emission ("Emmisive Color", Color) = (0,0,0,0)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.7
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
    }
    SubShader 
    {
    	
    	UsePass "Custom/MyToonOutline/OUTLINE"
    	//Tags { "RenderType"="Opaque" }
		//UsePass "Custom/MyVertexLit/DSEG"
		Tags { "RenderType"="Opaque" }
		//UsePass "Toon/Lighted/FORWARD"
		//UsePass "Custom/MyVertexLit/DSEG"
		UsePass "Custom/DiffuseSegment/FORWARD"
    }
    
    // might take this out, but if our hardware fails
    // we can fall back to vertex lit
} 