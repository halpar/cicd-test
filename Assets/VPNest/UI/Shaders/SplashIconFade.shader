Shader "Unlit/SplashIcon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask", 2D) = "white" {}
        _Progression ("Progression",float) = 0.0
        _Intensity ("Intensity", float) = 0.3
        
        }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
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

            sampler2D _MaskTex;
            float4 _MaskTex_ST;

            float _Progression;
            float _Intensity;
            float _Radius;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 maskCol = tex2D(_MaskTex, i.uv);
                col.a = (1 - smoothstep(_Progression - (2 + _Intensity) - _Intensity, _Progression - (2 + _Intensity), i.uv.x - i.uv.y)) * 0.4;
                col.a += (smoothstep(_Progression * (2 + _Intensity) - _Intensity, _Progression * (2 + _Intensity), i.uv.x - i.uv.y)) * 0.4;
                // apply fog
               // col.a = (1 - step(_Progression, i.uv.x - i.uv.y))*0.4;
                col.a += 0.6;
                UNITY_APPLY_FOG(i.fogCoord, col);
                col.a *= maskCol.a;
                return col;
            }
            ENDCG
        }
    }
}