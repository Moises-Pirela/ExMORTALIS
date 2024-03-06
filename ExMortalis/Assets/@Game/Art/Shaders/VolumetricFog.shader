Shader "Universal Render Pipeline/VolumetricFog"
{
    Properties
    {
        _FogColor("Fog Color", Color) = (0.5, 0.5, 0.5, 1.0)
        _FogDensity("Fog Density", Range(0.0, 1.0)) = 0.1
        // Add more properties as needed
    }

    SubShader
    {
        Tags 
        {
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0; 
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0; // Pass UV coordinates to fragment shader
            };

            float _FogDensity;
            sampler2D _FogColorTexture;
            float4 _FogColor;


            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex); 
                o.uv = v.uv;
                return o;
            }
 
            half4 frag(v2f i) : SV_Target
            {
                half fogFactor = exp2(-i.pos.w * _FogDensity);
                half4 sampledFogColor = tex2D(_FogColorTexture, i.uv);
                half4 fogColor = lerp(sampledFogColor, _FogColor, fogFactor);

                return fogColor;
            }
            ENDHLSL
        }
    }
}
