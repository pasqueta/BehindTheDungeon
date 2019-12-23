// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Custom/DiffuseDisparitionSwap3Colors" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}

	_Mask("Mask", 2D) = "white"{}
	_Progression("Progression", Range(0,1)) = 0

	_Metallic("Metallic", 2D) = "white"{}
	_Smoothness("Smoothess", Range(0,1)) = 0
	
	_ColorMask("",2D) = "" {}
	_ColorR("Color R", Vector) = (0,0,0,1)
	_ColorG("Color G", Vector) = (0,0,0,1)
	_ColorB("Color B", Vector) = (0,0,0,1)

}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 150

CGPROGRAM
#pragma surface surf Standard noforwardadd

	sampler2D _MainTex;

	sampler2D _Mask;
	float _Progression;

	sampler2D _Metallic;
	float _Smoothness;

	sampler2D _ColorMask;
	float4 _ColorR;
	float4 _ColorG;
	float4 _ColorB;

struct Input 
{
    float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutputStandard o) 
{
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
    o.Albedo = c.rgb;
    o.Alpha = c.a;

	float4 blend = tex2D(_Mask, IN.uv_MainTex);
	clip(blend.r - _Progression);


	fixed4 metal = tex2D(_Metallic, IN.uv_MainTex);
	o.Metallic = metal.r;
	o.Smoothness = metal.a * _Smoothness;

	if (tex2D(_ColorMask,IN.uv_MainTex).r > 0.5)
	{
		o.Albedo = c.rgb * _ColorR;
	}
	if (tex2D(_ColorMask, IN.uv_MainTex).g > 0.5)
	{
		o.Albedo = c.rgb * _ColorG;
	}
	if (tex2D(_ColorMask, IN.uv_MainTex).b > 0.5)
	{
		o.Albedo = c.rgb * _ColorB;
	}

}
ENDCG
}

Fallback "Mobile/VertexLit"
}
