Shader "Mobile/Diffuse Color" {
Properties {
    _MainColor ("Color", COLOR) = (1, 1, 1, 1)
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 150

CGPROGRAM
#pragma surface surf Lambert noforwardadd

fixed4 _MainColor;

struct Input {
    float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
    o.Albedo = _MainColor.rgb;
    o.Alpha = _MainColor.a;
}
ENDCG
}

Fallback "Mobile/VertexLit"
}
