Shader "Hidden/TapFeedback"
{
    Properties
    {
        [HideInInspector]
        _MainTex ("Texture", 2D) = "white" {}
        [HideInInspector]
        _Size ("Size", float) = 0.2
        _Color ("Size", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert alpha
            #pragma fragment frag alpha
            // make fog work
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            uniform float _Size = 0.2;
            float _R;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float size = _Size * _R;
                float outerGlowR = size * 0.6;
                float intensity = size * 1.6;
                float innerIntensity = size * 0.5;
                float innerCircle = size * 0.6 * (1.2 - _R);
                float innerCircleIntensity = size * 1.7;
                i.uv = i.uv - 0.5;
                float dist = sqrt(i.uv.x * i.uv.x + i.uv.y * i.uv.y);
                float col = 1 - smoothstep(outerGlowR, outerGlowR + intensity, dist);
                float4 finalOuterGlow = col;
                finalOuterGlow.xyz = 1;
                float4 innerGlow = smoothstep(outerGlowR - innerIntensity, outerGlowR + intensity  / 4, dist);
                innerGlow *= 1 - step(outerGlowR + intensity / 4, dist);

                finalOuterGlow += innerGlow;
                float4 innerCircleCol = (1 - smoothstep(innerCircle, innerCircle + innerCircleIntensity, dist)) * (1 - 2 * smoothstep(0, innerCircle + innerCircleIntensity, dist));
                finalOuterGlow += innerCircleCol;
                half4 colRes = finalOuterGlow;
                colRes.a *= 1 - _R * _R;
                colRes.a = clamp(colRes.a, 0, 1);

                return colRes * _Color;
            }
            ENDCG
        }
    }
}