Shader "WaterZ"
{
    Properties
    {
        Vector1_9DA1BF7E("Wave Speed", Float) = 0.2
        Color_6AA3AF81("Water Color", Color) = (0.6235294, 0.8313726, 0.8, 0)
        Color_11F89965("Wave Color", Color) = (1, 1, 0.9215686, 0)
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent+0"
        }
        
        Pass
        {
            Name "Pass"
            Tags 
            { 
                // LightMode: <None>
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
        
            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma shader_feature _ _SAMPLE_GI
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS_UNLIT
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float Vector1_9DA1BF7E;
            float4 Color_6AA3AF81;
            float4 Color_11F89965;
            CBUFFER_END
        
            // Graph Functions
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }
            
            
            float2 Unity_GradientNoise_Dir_float(float2 p)
            {
                // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }
            
            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            { 
                float2 p = UV * Scale;
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
                float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
            }
            
            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }
            
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
            
            void Unity_Clamp_float(float In, float Min, float Max, out float Out)
            {
                Out = clamp(In, Min, Max);
            }
            
            void Unity_Posterize_float(float In, float Steps, out float Out)
            {
                Out = floor(In / (1 / Steps)) * (1 / Steps);
            }
            
            void Unity_Blend_Multiply_float4(float4 Base, float4 Blend, out float4 Out, float Opacity)
            {
                Out = Base * Blend;
                Out = lerp(Base, Out, Opacity);
            }
            
            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }
            
            void Unity_Add_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A + B;
            }
        
            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 ObjectSpacePosition;
                float4 uv0;
                float3 TimeParameters;
            };
            
            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Split_7D6DE16B_R_1 = IN.ObjectSpacePosition[0];
                float _Split_7D6DE16B_G_2 = IN.ObjectSpacePosition[1];
                float _Split_7D6DE16B_B_3 = IN.ObjectSpacePosition[2];
                float _Split_7D6DE16B_A_4 = 0;
                float _Multiply_47EEEF4B_Out_2;
                Unity_Multiply_float(_Split_7D6DE16B_G_2, 0.5, _Multiply_47EEEF4B_Out_2);
                float _Property_D4C79AC3_Out_0 = Vector1_9DA1BF7E;
                float _Multiply_93011312_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_D4C79AC3_Out_0, _Multiply_93011312_Out_2);
                float2 _TilingAndOffset_B5B477F3_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_93011312_Out_2.xx), _TilingAndOffset_B5B477F3_Out_3);
                float _GradientNoise_FB8BDEBB_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_B5B477F3_Out_3, 200, _GradientNoise_FB8BDEBB_Out_2);
                float2 _Vector2_624A42B9_Out_0 = float2(_Multiply_93011312_Out_2, 0);
                float2 _TilingAndOffset_7A107C66_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 0.3), _Vector2_624A42B9_Out_0, _TilingAndOffset_7A107C66_Out_3);
                float _GradientNoise_FFE47C59_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_7A107C66_Out_3, 120, _GradientNoise_FFE47C59_Out_2);
                float _Add_4184AFFE_Out_2;
                Unity_Add_float(0, _GradientNoise_FFE47C59_Out_2, _Add_4184AFFE_Out_2);
                float _Add_9F03C0C4_Out_2;
                Unity_Add_float(_Add_4184AFFE_Out_2, 0.3, _Add_9F03C0C4_Out_2);
                float _Add_780D41B_Out_2;
                Unity_Add_float(_GradientNoise_FB8BDEBB_Out_2, _Add_9F03C0C4_Out_2, _Add_780D41B_Out_2);
                float _Add_EE74AB12_Out_2;
                Unity_Add_float(_Add_780D41B_Out_2, 0.2, _Add_EE74AB12_Out_2);
                float _Multiply_858F95BD_Out_2;
                Unity_Multiply_float(_Add_EE74AB12_Out_2, 0.6, _Multiply_858F95BD_Out_2);
                float _Add_71089766_Out_2;
                Unity_Add_float(_Multiply_47EEEF4B_Out_2, _Multiply_858F95BD_Out_2, _Add_71089766_Out_2);
                float4 _Combine_BA89149E_RGBA_4;
                float3 _Combine_BA89149E_RGB_5;
                float2 _Combine_BA89149E_RG_6;
                Unity_Combine_float(_Split_7D6DE16B_R_1, _Add_71089766_Out_2, _Split_7D6DE16B_B_3, _Split_7D6DE16B_A_4, _Combine_BA89149E_RGBA_4, _Combine_BA89149E_RGB_5, _Combine_BA89149E_RG_6);
                description.VertexPosition = _Combine_BA89149E_RGB_5;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
                float3 TimeParameters;
            };
            
            struct SurfaceDescription
            {
                float3 Color;
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _Property_D4C79AC3_Out_0 = Vector1_9DA1BF7E;
                float _Multiply_93011312_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_D4C79AC3_Out_0, _Multiply_93011312_Out_2);
                float2 _TilingAndOffset_B5B477F3_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_93011312_Out_2.xx), _TilingAndOffset_B5B477F3_Out_3);
                float _GradientNoise_FB8BDEBB_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_B5B477F3_Out_3, 200, _GradientNoise_FB8BDEBB_Out_2);
                float2 _Vector2_624A42B9_Out_0 = float2(_Multiply_93011312_Out_2, 0);
                float2 _TilingAndOffset_7A107C66_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 0.3), _Vector2_624A42B9_Out_0, _TilingAndOffset_7A107C66_Out_3);
                float _GradientNoise_FFE47C59_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_7A107C66_Out_3, 120, _GradientNoise_FFE47C59_Out_2);
                float _Add_4184AFFE_Out_2;
                Unity_Add_float(0, _GradientNoise_FFE47C59_Out_2, _Add_4184AFFE_Out_2);
                float _Add_9F03C0C4_Out_2;
                Unity_Add_float(_Add_4184AFFE_Out_2, 0.3, _Add_9F03C0C4_Out_2);
                float _Add_780D41B_Out_2;
                Unity_Add_float(_GradientNoise_FB8BDEBB_Out_2, _Add_9F03C0C4_Out_2, _Add_780D41B_Out_2);
                float _Add_EE74AB12_Out_2;
                Unity_Add_float(_Add_780D41B_Out_2, 0.2, _Add_EE74AB12_Out_2);
                float _Clamp_DBC80250_Out_3;
                Unity_Clamp_float(_Add_EE74AB12_Out_2, 0, 1, _Clamp_DBC80250_Out_3);
                float _Clamp_333D6FA7_Out_3;
                Unity_Clamp_float(_Clamp_DBC80250_Out_3, 0, 1, _Clamp_333D6FA7_Out_3);
                float _Posterize_B04BAFF7_Out_2;
                Unity_Posterize_float(_Clamp_333D6FA7_Out_3, 1, _Posterize_B04BAFF7_Out_2);
                float4 _Property_952C595E_Out_0 = Color_6AA3AF81;
                float4 _Blend_BA8E212A_Out_2;
                Unity_Blend_Multiply_float4((_Posterize_B04BAFF7_Out_2.xxxx), _Property_952C595E_Out_0, _Blend_BA8E212A_Out_2, 1);
                float _Subtract_269278FA_Out_2;
                Unity_Subtract_float(1, _Posterize_B04BAFF7_Out_2, _Subtract_269278FA_Out_2);
                float4 _Property_395DA778_Out_0 = Color_11F89965;
                float4 _Blend_8A5E450F_Out_2;
                Unity_Blend_Multiply_float4((_Subtract_269278FA_Out_2.xxxx), _Property_395DA778_Out_0, _Blend_8A5E450F_Out_2, 1);
                float4 _Add_19AA831D_Out_2;
                Unity_Add_float4(_Blend_BA8E212A_Out_2, _Blend_8A5E450F_Out_2, _Add_19AA831D_Out_2);
                float4 _Add_340130A4_Out_2;
                Unity_Add_float4(float4(0, 0, 0, 0), _Add_19AA831D_Out_2, _Add_340130A4_Out_2);
                surface.Color = (_Add_340130A4_Out_2.xyz);
                surface.Alpha = 1;
                surface.AlphaClipThreshold = 0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float4 interp00 : TEXCOORD0;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.ObjectSpaceTangent =          input.tangentOS;
                output.ObjectSpacePosition =         input.positionOS;
                output.uv0 =                         input.uv0;
                output.TimeParameters =              _TimeParameters.xyz;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
            
            
                output.uv0 =                         input.texCoord0;
                output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags 
            { 
                "LightMode" = "ShadowCaster"
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing
        
            // Keywords
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS_SHADOWCASTER
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float Vector1_9DA1BF7E;
            float4 Color_6AA3AF81;
            float4 Color_11F89965;
            CBUFFER_END
        
            // Graph Functions
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }
            
            
            float2 Unity_GradientNoise_Dir_float(float2 p)
            {
                // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }
            
            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            { 
                float2 p = UV * Scale;
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
                float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
            }
            
            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }
            
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
        
            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 ObjectSpacePosition;
                float4 uv0;
                float3 TimeParameters;
            };
            
            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Split_7D6DE16B_R_1 = IN.ObjectSpacePosition[0];
                float _Split_7D6DE16B_G_2 = IN.ObjectSpacePosition[1];
                float _Split_7D6DE16B_B_3 = IN.ObjectSpacePosition[2];
                float _Split_7D6DE16B_A_4 = 0;
                float _Multiply_47EEEF4B_Out_2;
                Unity_Multiply_float(_Split_7D6DE16B_G_2, 0.5, _Multiply_47EEEF4B_Out_2);
                float _Property_D4C79AC3_Out_0 = Vector1_9DA1BF7E;
                float _Multiply_93011312_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_D4C79AC3_Out_0, _Multiply_93011312_Out_2);
                float2 _TilingAndOffset_B5B477F3_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_93011312_Out_2.xx), _TilingAndOffset_B5B477F3_Out_3);
                float _GradientNoise_FB8BDEBB_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_B5B477F3_Out_3, 200, _GradientNoise_FB8BDEBB_Out_2);
                float2 _Vector2_624A42B9_Out_0 = float2(_Multiply_93011312_Out_2, 0);
                float2 _TilingAndOffset_7A107C66_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 0.3), _Vector2_624A42B9_Out_0, _TilingAndOffset_7A107C66_Out_3);
                float _GradientNoise_FFE47C59_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_7A107C66_Out_3, 120, _GradientNoise_FFE47C59_Out_2);
                float _Add_4184AFFE_Out_2;
                Unity_Add_float(0, _GradientNoise_FFE47C59_Out_2, _Add_4184AFFE_Out_2);
                float _Add_9F03C0C4_Out_2;
                Unity_Add_float(_Add_4184AFFE_Out_2, 0.3, _Add_9F03C0C4_Out_2);
                float _Add_780D41B_Out_2;
                Unity_Add_float(_GradientNoise_FB8BDEBB_Out_2, _Add_9F03C0C4_Out_2, _Add_780D41B_Out_2);
                float _Add_EE74AB12_Out_2;
                Unity_Add_float(_Add_780D41B_Out_2, 0.2, _Add_EE74AB12_Out_2);
                float _Multiply_858F95BD_Out_2;
                Unity_Multiply_float(_Add_EE74AB12_Out_2, 0.6, _Multiply_858F95BD_Out_2);
                float _Add_71089766_Out_2;
                Unity_Add_float(_Multiply_47EEEF4B_Out_2, _Multiply_858F95BD_Out_2, _Add_71089766_Out_2);
                float4 _Combine_BA89149E_RGBA_4;
                float3 _Combine_BA89149E_RGB_5;
                float2 _Combine_BA89149E_RG_6;
                Unity_Combine_float(_Split_7D6DE16B_R_1, _Add_71089766_Out_2, _Split_7D6DE16B_B_3, _Split_7D6DE16B_A_4, _Combine_BA89149E_RGBA_4, _Combine_BA89149E_RGB_5, _Combine_BA89149E_RG_6);
                description.VertexPosition = _Combine_BA89149E_RGB_5;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
            };
            
            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                surface.Alpha = 1;
                surface.AlphaClipThreshold = 0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.ObjectSpaceTangent =          input.tangentOS;
                output.ObjectSpacePosition =         input.positionOS;
                output.uv0 =                         input.uv0;
                output.TimeParameters =              _TimeParameters.xyz;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
            
            
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
            ENDHLSL
        }
        
        Pass
        {
            Name "DepthOnly"
            Tags 
            { 
                "LightMode" = "DepthOnly"
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite On
            ColorMask 0
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS_DEPTHONLY
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float Vector1_9DA1BF7E;
            float4 Color_6AA3AF81;
            float4 Color_11F89965;
            CBUFFER_END
        
            // Graph Functions
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }
            
            
            float2 Unity_GradientNoise_Dir_float(float2 p)
            {
                // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }
            
            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            { 
                float2 p = UV * Scale;
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
                float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
            }
            
            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }
            
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
        
            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 ObjectSpacePosition;
                float4 uv0;
                float3 TimeParameters;
            };
            
            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Split_7D6DE16B_R_1 = IN.ObjectSpacePosition[0];
                float _Split_7D6DE16B_G_2 = IN.ObjectSpacePosition[1];
                float _Split_7D6DE16B_B_3 = IN.ObjectSpacePosition[2];
                float _Split_7D6DE16B_A_4 = 0;
                float _Multiply_47EEEF4B_Out_2;
                Unity_Multiply_float(_Split_7D6DE16B_G_2, 0.5, _Multiply_47EEEF4B_Out_2);
                float _Property_D4C79AC3_Out_0 = Vector1_9DA1BF7E;
                float _Multiply_93011312_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_D4C79AC3_Out_0, _Multiply_93011312_Out_2);
                float2 _TilingAndOffset_B5B477F3_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), (_Multiply_93011312_Out_2.xx), _TilingAndOffset_B5B477F3_Out_3);
                float _GradientNoise_FB8BDEBB_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_B5B477F3_Out_3, 200, _GradientNoise_FB8BDEBB_Out_2);
                float2 _Vector2_624A42B9_Out_0 = float2(_Multiply_93011312_Out_2, 0);
                float2 _TilingAndOffset_7A107C66_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 0.3), _Vector2_624A42B9_Out_0, _TilingAndOffset_7A107C66_Out_3);
                float _GradientNoise_FFE47C59_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_7A107C66_Out_3, 120, _GradientNoise_FFE47C59_Out_2);
                float _Add_4184AFFE_Out_2;
                Unity_Add_float(0, _GradientNoise_FFE47C59_Out_2, _Add_4184AFFE_Out_2);
                float _Add_9F03C0C4_Out_2;
                Unity_Add_float(_Add_4184AFFE_Out_2, 0.3, _Add_9F03C0C4_Out_2);
                float _Add_780D41B_Out_2;
                Unity_Add_float(_GradientNoise_FB8BDEBB_Out_2, _Add_9F03C0C4_Out_2, _Add_780D41B_Out_2);
                float _Add_EE74AB12_Out_2;
                Unity_Add_float(_Add_780D41B_Out_2, 0.2, _Add_EE74AB12_Out_2);
                float _Multiply_858F95BD_Out_2;
                Unity_Multiply_float(_Add_EE74AB12_Out_2, 0.6, _Multiply_858F95BD_Out_2);
                float _Add_71089766_Out_2;
                Unity_Add_float(_Multiply_47EEEF4B_Out_2, _Multiply_858F95BD_Out_2, _Add_71089766_Out_2);
                float4 _Combine_BA89149E_RGBA_4;
                float3 _Combine_BA89149E_RGB_5;
                float2 _Combine_BA89149E_RG_6;
                Unity_Combine_float(_Split_7D6DE16B_R_1, _Add_71089766_Out_2, _Split_7D6DE16B_B_3, _Split_7D6DE16B_A_4, _Combine_BA89149E_RGBA_4, _Combine_BA89149E_RGB_5, _Combine_BA89149E_RG_6);
                description.VertexPosition = _Combine_BA89149E_RGB_5;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
            };
            
            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                surface.Alpha = 1;
                surface.AlphaClipThreshold = 0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.ObjectSpaceTangent =          input.tangentOS;
                output.ObjectSpacePosition =         input.positionOS;
                output.uv0 =                         input.uv0;
                output.TimeParameters =              _TimeParameters.xyz;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
            
            
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
            ENDHLSL
        }
        
    }
    FallBack "Hidden/Shader Graph/FallbackError"
}
