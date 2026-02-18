Shader "Custom/FluidDistortion"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (0.1, 0.4, 0.3, 1) // Dark Greenish
        _SecondaryColor ("Secondary Color", Color) = (0.05, 0.2, 0.15, 1) // Darker
        _Speed ("Speed", Range(0.1, 5.0)) = 1.0
        _Scale ("Scale", Range(1.0, 50.0)) = 10.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _MainColor;
            float4 _SecondaryColor;
            float _Speed;
            float _Scale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Simple noise function
            float random (float2 st) {
                return frac(sin(dot(st.xy, float2(12.9898,78.233)))*43758.5453123);
            }

            float noise (float2 st) {
                float2 i = floor(st);
                float2 f = frac(st);
                float a = random(i);
                float b = random(i + float2(1.0, 0.0));
                float c = random(i + float2(0.0, 1.0));
                float d = random(i + float2(1.0, 1.0));
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(a, b, u.x) + (c - a)* u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * _Scale;
                float time = _Time.y * _Speed;
                
                // Fluid-like distortion using noise
                float n1 = noise(uv + float2(time * 0.1, time * 0.2));
                float n2 = noise(uv + float2(n1, n1) - float2(time * 0.2, time * 0.1));
                
                float val = n2;
                
                // Interpolate colors
                return lerp(_SecondaryColor, _MainColor, val);
            }
            ENDCG
        }
    }
}
