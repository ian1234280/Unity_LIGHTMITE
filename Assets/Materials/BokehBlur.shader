Shader "Custom/BokehBlur"
{
    Properties
    {
        [HideinInspector] _MainTex("Main Tex", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalRenderPipeline"
            "RenderType" = "Transparent"
        }
        Cull Off 
        ZWrite OFF
        ZTest Always
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        #pragma vertex VS
        #pragma fragment PS

        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        half _FocusDistance;	
        half _FarBlurIntensity;	
        half _NearBlurIntensity;	
        half _BlurLoop;		   
        half _BlurRadius;	  
        TEXTURE2D(_MainTex);   
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_SourTex);   
        SAMPLER(sampler_SourTex);
        SAMPLER(_CameraDepthTexture);	

        struct VSInput
        {
            float4 positionL : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct PSInput
        {
            float4 positionH : SV_POSITION;
            float2 uv : TEXCOORD0;
        };
        
        ENDHLSL

        Pass
        {
            NAME "Boken Blur"
            
            HLSLPROGRAM
            
            PSInput VS(VSInput vsInput)
            {
                PSInput vsOutput;

                vsOutput.positionH = TransformObjectToHClip(vsInput.positionL);

                #ifdef UNITY_UV_STARTS_AT_TOP
                if(_MainTex_TexelSize.y < 0)
                    vsInput.uv.y = 1 - vsInput.uv.y;
                #endif
                vsOutput.uv = vsInput.uv;

                return vsOutput;
            }

            float4 PS(PSInput psInput) : SV_TARGET
            {
                float4 outputColor;

                float angle = 2.3398;   
                float2x2 rotation = float2x2(cos(angle), -sin(angle), sin(angle), cos(angle));  
                float2 offsetUV = float2(_BlurRadius, 0);	
                float2 targetUV;
                float r;
                for(int i = 1; i < _BlurLoop; ++i)
                {
                    
                    r = sqrt(i);
                    offsetUV = mul(rotation, offsetUV);
                    targetUV = psInput.uv + _MainTex_TexelSize.xy * offsetUV * r;
                    
                    outputColor += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, targetUV);
                }

                return outputColor / (_BlurLoop - 1);	
            }
            
            ENDHLSL
        }

        Pass
        {
            NAME "depth of field"
            HLSLPROGRAM
            PSInput VS(VSInput vsInput)
            {
                PSInput vsOutput;

                vsOutput.positionH = TransformObjectToHClip(vsInput.positionL);

                #ifdef UNITY_UV_STARTS_AT_TOP
                if(_MainTex_TexelSize.y < 0)
                    vsInput.uv.y = 1 - vsInput.uv.y;
                #endif
                vsOutput.uv = vsInput.uv;

                return vsOutput;
            }

            float4 PS(PSInput psInput) : SV_TARGET
            {
                
                float4 blurTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, psInput.uv);
                float4 sourceTex = SAMPLE_TEXTURE2D(_SourTex, sampler_SourTex, psInput.uv); 


                float depth = Linear01Depth(tex2D(_CameraDepthTexture, psInput.uv).r , _ZBufferParams);
                float distance;
          
                if(depth > _FocusDistance)
                {
                   
                    distance = saturate((depth - _FocusDistance) * (depth - _FocusDistance) * _FarBlurIntensity);
                }
                else
                {
                    distance = saturate((depth - _FocusDistance) * (depth - _FocusDistance) * _NearBlurIntensity);
                }
                
            
                return lerp(sourceTex, blurTex, distance);
            }
            ENDHLSL
        }
    }
}