Shader "TapTap/SpriteOutline2D"
{
    Properties
    {
        [Header(Sprite)]
        [Space(5)]
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)
        [Space(10)]
        [Header(Outline)]
        [Space(5)]
        _OutlineThickness ("Espessura (pixels)", Range(0, 16)) = 3
        _AlphaCutoff ("Limite da Borda (alpha)", Range(0.01, 0.99)) = 0.3
        [KeywordEnum(Solido, Gradiente Vertical, Cores Animadas)] _OutlineMode ("Modo de Cor", Float) = 0
        [HDR] _OutlineColor ("Cor do Outline (HDR)", Color) = (1, 1, 1, 1)
        [HDR] _OutlineColorB ("Cor B (gradiente vertical)", Color) = (1, 1, 1, 1)
        [Space(5)]
        [Header(Cores Animadas escolha ate 4)]
        [Space(5)]
        [HDR] _AnimColor1 ("Cor 1", Color) = (1, 0.2, 0.2, 1)
        [HDR] _AnimColor2 ("Cor 2", Color) = (0.2, 0.5, 1, 1)
        [HDR] _AnimColor3 ("Cor 3", Color) = (0.3, 1, 0.4, 1)
        [HDR] _AnimColor4 ("Cor 4", Color) = (1, 0.9, 0.2, 1)
        _AnimColorCount ("Quantas Cores Usar (2 a 4)", Range(2, 4)) = 3
        _AnimSpeed ("Velocidade da Animacao", Range(0, 5)) = 1.0
        _AnimSpatial ("Variacao pela Tela (fita de LED)", Range(0, 10)) = 2.0
        _AnimSaturation ("Saturacao", Range(0, 2)) = 1.0
        _AnimBrightness ("Brilho (HDR)", Range(0, 4)) = 1.0

        [Space(10)]
        [Header(Pisca opcional)]
        [Space(5)]
        _PulseSpeed ("Velocidade do Pisca (0 desliga)", Range(0, 10)) = 0
        _PulseMin ("Brilho Minimo do Pisca", Range(0, 1)) = 0.4
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            Tags { "LightMode" = "Universal2D" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature_local _OUTLINEMODE_SOLIDO _OUTLINEMODE_GRADIENTE_VERTICAL _OUTLINEMODE_CORES_ANIMADAS
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);  SAMPLER(sampler_MainTex);
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _MainTex_TexelSize;
                half4  _Color;
                half   _OutlineThickness;
                half   _AlphaCutoff;
                half4  _OutlineColor;
                half4  _OutlineColorB;
                half4  _AnimColor1;
                half4  _AnimColor2;
                half4  _AnimColor3;
                half4  _AnimColor4;
                half   _AnimColorCount;
                half   _AnimSpeed;
                half   _AnimSpatial;
                half   _AnimSaturation;
                half   _AnimBrightness;
                half   _PulseSpeed;
                half   _PulseMin;
            CBUFFER_END
            half3 RGBtoHSV(half3 c)
            {
                half4 K = half4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                half4 p = lerp(half4(c.bg, K.wz), half4(c.gb, K.xy), step(c.b, c.g));
                half4 q = lerp(half4(p.xyw, c.r), half4(c.r, p.yzx), step(p.x, c.r));
                half d = q.x - min(q.w, q.y);
                half e = 1.0e-5;
                return half3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }
            half3 HSVtoRGB(half3 c)
            {
                half4 K = half4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                half3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }
            half3 AnimatedColor(half phase)
            {
                half count = clamp(floor(_AnimColorCount), 2.0, 4.0);
                half t = frac(phase) * count;
                half idx = floor(t);
                half f = smoothstep(0.0, 1.0, frac(t));
                half3 c0 = _AnimColor1.rgb;
                half3 c1 = (count > 1.5) ? _AnimColor2.rgb : c0;
                half3 c2 = (count > 2.5) ? _AnimColor3.rgb : c0;
                half3 c3 = (count > 3.5) ? _AnimColor4.rgb : c0;
                half3 cols[4] = { c0, c1, c2, c3 };
                int i = (int)idx % (int)count;
                int j = (i + 1) % (int)count;
                half3 outCol = lerp(cols[i], cols[j], f);
                half3 hsv = RGBtoHSV(outCol);
                hsv.y = saturate(hsv.y * _AnimSaturation);
                return HSVtoRGB(hsv) * _AnimBrightness;
            }
            struct Attributes
            {
                float3 positionOS : POSITION;
                float4 color      : COLOR;
                float2 uv         : TEXCOORD0;
            };
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                float4 screenPos  : TEXCOORD1;
                half4  color      : COLOR;
            };
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.screenPos = ComputeScreenPos(OUT.positionCS);
                OUT.color = IN.color * _Color;
                return OUT;
            }
            half MaxAlphaAround(float2 uv, float2 texel, int radius)
            {
                half a = 0;
                [loop]
                for (int y = -radius; y <= radius; y++)
                {
                    [loop]
                    for (int x = -radius; x <= radius; x++)
                    {
                        if (x * x + y * y > radius * radius) continue;
                        float2 o = float2(x, y) * texel;
                        a = max(a, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + o).a);
                    }
                }
                return a;
            }
            half4 frag(Varyings IN) : SV_Target
            {
                half4 src = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * IN.color;
                int radius = (int)round(_OutlineThickness);
                float2 texel = _MainTex_TexelSize.xy;
                if (src.a >= _AlphaCutoff || radius < 1)
                    return src;
                half around = MaxAlphaAround(IN.uv, texel, radius);
                half edge = step(_AlphaCutoff, around);
                if (edge < 0.5)
                    return half4(0, 0, 0, 0); // fora do contorno -> transparente

                // ----- Cor do contorno conforme o modo -----
                half3 oc = _OutlineColor.rgb;
                #if defined(_OUTLINEMODE_GRADIENTE_VERTICAL)
                    oc = lerp(_OutlineColor.rgb, _OutlineColorB.rgb, saturate(IN.uv.y));
                #elif defined(_OUTLINEMODE_CORES_ANIMADAS)
                    float2 sp = IN.screenPos.xy / max(IN.screenPos.w, 1e-4);
                    half spatial = (sp.x + sp.y) * _AnimSpatial;
                    half phase = _Time.y * _AnimSpeed * 0.15 + spatial;
                    oc = AnimatedColor(phase);
                #endif
                // ----- Pisca opcional -----
                half pulse = 1.0;
                if (_PulseSpeed > 0.001)
                    pulse = lerp(_PulseMin, 1.0, 0.5 + 0.5 * sin(_Time.y * _PulseSpeed));
                return half4(oc * pulse, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Sprites/Default"
}
