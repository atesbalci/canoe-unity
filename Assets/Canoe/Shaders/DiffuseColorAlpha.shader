Shader "Mobile/Diffuse Color Alpha" {
Properties {
    _Color ("Color", COLOR) = (1, 1, 1, 1)
}
SubShader {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
    Blend SrcAlpha OneMinusSrcAlpha
    ZWrite Off
    LOD 200

CGPROGRAM
#pragma surface surf Lambert noforwardadd alpha:auto

fixed4 _Color;

struct Input {
    float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
    o.Albedo = _Color.rgb;
    o.Alpha = _Color.a;
}
ENDCG
}

Fallback "Mobile/VertexLit"
}
