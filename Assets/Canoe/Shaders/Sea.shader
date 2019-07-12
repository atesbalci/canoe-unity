Shader "Canoe/Sea" {
Properties {
    _MainColor ("Color", COLOR) = (1, 1, 1, 1)
    _Speed ("Speed", Float) = 1
    _Period ("Period", Float) = 1
    _WaveSize ("Wave Size", Float) = 1
    _Gap ("Gap", Float) = 1
    _WrinkleFrequency ("Wrinkle Frequency", Float) = 1
    _WrinkleSize ("Wrinkle Size", Float) = 1
}

SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 80
    
    Pass
    {
        Tags{ "RenderType" = "Opaque" }

        CGPROGRAM

        #pragma vertex vert
        #pragma fragment frag
        #pragma target 2.0
        #include "UnityCG.cginc"
        #include "noiseSimplex.cginc"

        fixed4 _MainColor;
        float _Speed;
        float _Period;
        float _WaveSize;
        float _Gap;
        float _WrinkleFrequency;
        float _WrinkleSize;

        struct appdata
        {
            float3 pos : POSITION;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct v2f
        {
            float4 pos : SV_POSITION;

            UNITY_VERTEX_OUTPUT_STEREO
        };

        v2f vert(appdata IN)
        {
            v2f o;
            UNITY_SETUP_INSTANCE_ID(IN);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
            
            float3 pos;
            
            pos.x = IN.pos.x;
            pos.y = (sin(IN.pos.z * _Period + _Time.y * _Speed) + sin(IN.pos.z * _Period * 0.4f + _Time.y * _Speed) + snoise(float2(IN.pos.x + _Time.y, IN.pos.z + _Time.y))) * _WaveSize;
            pos.z = IN.pos.z + sin((IN.pos.x + _Time.y * _Gap) * _WrinkleFrequency) * _WrinkleSize + sin((IN.pos.z * sin(_Time.y) + IN.pos.x * cos(_Time.y)) * _Period * 0.4f + _Time.y * _Speed);

            o.pos = UnityObjectToClipPos(pos);
            
            return o;
        }

        fixed4 frag(v2f IN) : SV_Target
        {
            fixed4 col;
            
            col.rgb = _MainColor.rgb;
            col.a = 1.0f;

            return col;
        }

        ENDCG
    }
}
}
