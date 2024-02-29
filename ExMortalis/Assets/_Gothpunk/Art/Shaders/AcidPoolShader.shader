Shader "Custom/AcidPoolBubblesAndSplashWithGlow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _DistortionStrength ("Distortion Strength", Range(0, 1)) = 0.5
        _TintColor ("Tint Color", Color) = (1,1,1,1)
        _BubbleSpeed ("Bubble Speed", Range(0, 1)) = 0.1
        _BubbleTexture ("Bubble Texture", 2D) = "white" {}
        _SplashTexture ("Splash Texture", 2D) = "white" {}
        _GlowIntensity ("Glow Intensity", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NormalMap;
            float _DistortionStrength;
            fixed4 _TintColor;
            float _BubbleSpeed;
            sampler2D _BubbleTexture;
            sampler2D _SplashTexture;
            float _GlowIntensity;

            float3 GetPerlinNoise(float2 uv, float time)
            {
                float bubbleDistortion = tex2D(_BubbleTexture, uv + time).r;
                float splashStrength = tex2D(_SplashTexture, uv + time).r; // Use the same time for splash texture
                float splashDistortion = lerp(0, bubbleDistortion, splashStrength);
                return float3(bubbleDistortion, bubbleDistortion, splashDistortion) * _DistortionStrength;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float time = _Time.y * _BubbleSpeed;
                float2 distortionUV = i.uv + GetPerlinNoise(i.uv, time).xy;
                float3 normal = UnpackNormal(tex2D(_NormalMap, distortionUV));
                float3 worldNormal = normalize(mul(normal, unity_ObjectToWorld));

                fixed4 texColor = tex2D(_MainTex, distortionUV);
                float3 distortedNormal = normalize(worldNormal + GetPerlinNoise(i.uv, time).xyz);
                float3 viewDir = normalize(i.vertex.xyz - _WorldSpaceCameraPos);
                float3 reflDir = reflect(-viewDir, distortedNormal);
                float3 normalColor = (reflDir * 0.5 + 0.5);

                float glowIntensity = max(texColor.r, max(texColor.g, texColor.b)) * _GlowIntensity;
                fixed4 finalColor = texColor * _TintColor * float4(normalColor, 1) * glowIntensity;
                return finalColor;
            }
            ENDCG
        }
    }
}
