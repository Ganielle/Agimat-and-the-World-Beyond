Shader "Agimat and the World Beyond/Noise Animation"
{
    Properties
    {
        [NoScaleOffset]_MainTex("MainTexture", 2D) = "white" {}
        [HDR]_Tint("Color", Color) = (1.835294, 0.1333333, 0.1333333, 1)
        Vector2_288FEC1D("Direction", Vector) = (0, 0.1, 0, 0)
        Vector1_ACBBFEA9("AngleOffsetSpeed", Float) = 2
        Vector1_7A60B76E("CellDensity", Float) = 2
        _Stencil("Stencil ID", Float) = 0
        _StencilComp("StencilComp", Float) = 8
        _StencilOp("StencilOp", Float) = 0
        _StencilReadMask("StencilReadMask", Float) = 255
        _StencilWriteMask("StencilWriteMask", Float) = 255
        _ColorMask("ColorMask", Float) = 15
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
        ZTest [unity_GUIZTestMode]
        ZWrite Off
        // ColorMask: <None>

        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }
        ColorMask[_ColorMask]
        

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
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
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
    float4 _Tint;
    float2 Vector2_288FEC1D;
    float Vector1_ACBBFEA9;
    float Vector1_7A60B76E;
    CBUFFER_END
    TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
    SAMPLER(_SampleTexture2D_B4797219_Sampler_3_Linear_Repeat);

        // Graph Functions
        
    void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
    {
        Out = A * B;
    }

    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
    {
        Out = UV * Tiling + Offset;
    }

    void Unity_Multiply_float(float A, float B, out float Out)
    {
        Out = A * B;
    }


    inline float2 Unity_Voronoi_RandomVector_float (float2 UV, float offset)
    {
        float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
        UV = frac(sin(mul(UV, m)) * 46839.32);
        return float2(sin(UV.y*+offset)*0.5+0.5, cos(UV.x*offset)*0.5+0.5);
    }

    void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
    {
        float2 g = floor(UV * CellDensity);
        float2 f = frac(UV * CellDensity);
        float t = 8.0;
        float3 res = float3(8.0, 0.0, 0.0);

        for(int y=-1; y<=1; y++)
        {
            for(int x=-1; x<=1; x++)
            {
                float2 lattice = float2(x,y);
                float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
                float d = distance(lattice + offset, f);

                if(d < res.x)
                {
                    res = float3(d, offset.x, offset.y);
                    Out = res.x;
                    Cells = res.y;
                }
            }
        }
    }

    void Unity_Power_float(float A, float B, out float Out)
    {
        Out = pow(A, B);
    }

    void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
    {
        Out = A * B;
    }

        // Graph Vertex
        // GraphVertex: <None>
        
        // Graph Pixel
        struct SurfaceDescriptionInputs
    {
        float4 uv0;
        float4 VertexColor;
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
        float4 _SampleTexture2D_B4797219_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
        float _SampleTexture2D_B4797219_R_4 = _SampleTexture2D_B4797219_RGBA_0.r;
        float _SampleTexture2D_B4797219_G_5 = _SampleTexture2D_B4797219_RGBA_0.g;
        float _SampleTexture2D_B4797219_B_6 = _SampleTexture2D_B4797219_RGBA_0.b;
        float _SampleTexture2D_B4797219_A_7 = _SampleTexture2D_B4797219_RGBA_0.a;
        float2 _Property_6459D444_Out_0 = Vector2_288FEC1D;
        float2 _Multiply_B7513082_Out_2;
        Unity_Multiply_float(_Property_6459D444_Out_0, (IN.TimeParameters.x.xx), _Multiply_B7513082_Out_2);
        float2 _TilingAndOffset_20F18B0F_Out_3;
        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_B7513082_Out_2, _TilingAndOffset_20F18B0F_Out_3);
        float _Property_9E48D93B_Out_0 = Vector1_ACBBFEA9;
        float _Multiply_C3781331_Out_2;
        Unity_Multiply_float(_Property_9E48D93B_Out_0, IN.TimeParameters.x, _Multiply_C3781331_Out_2);
        float _Property_86EFC281_Out_0 = Vector1_7A60B76E;
        float _Voronoi_47BC6ACC_Out_3;
        float _Voronoi_47BC6ACC_Cells_4;
        Unity_Voronoi_float(_TilingAndOffset_20F18B0F_Out_3, _Multiply_C3781331_Out_2, _Property_86EFC281_Out_0, _Voronoi_47BC6ACC_Out_3, _Voronoi_47BC6ACC_Cells_4);
        float _Power_B3755451_Out_2;
        Unity_Power_float(_Voronoi_47BC6ACC_Out_3, 1.5, _Power_B3755451_Out_2);
        float4 _Multiply_2B883C98_Out_2;
        Unity_Multiply_float(_SampleTexture2D_B4797219_RGBA_0, (_Power_B3755451_Out_2.xxxx), _Multiply_2B883C98_Out_2);
        float4 _Multiply_6A355D3A_Out_2;
        Unity_Multiply_float(IN.VertexColor, _Multiply_2B883C98_Out_2, _Multiply_6A355D3A_Out_2);
        float4 _Property_B06C3993_Out_0 = _Tint;
        float4 _Multiply_434B9292_Out_2;
        Unity_Multiply_float(_Multiply_6A355D3A_Out_2, _Property_B06C3993_Out_0, _Multiply_434B9292_Out_2);
        float _Split_C8A9A842_R_1 = _Multiply_434B9292_Out_2[0];
        float _Split_C8A9A842_G_2 = _Multiply_434B9292_Out_2[1];
        float _Split_C8A9A842_B_3 = _Multiply_434B9292_Out_2[2];
        float _Split_C8A9A842_A_4 = _Multiply_434B9292_Out_2[3];
        surface.Color = (_Multiply_434B9292_Out_2.xyz);
        surface.Alpha = _Split_C8A9A842_A_4;
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
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };

        // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 texCoord0;
                float4 color;
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
                float4 interp01 : TEXCOORD1;
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
                output.interp01.xyzw = input.color;
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
                output.color = input.interp01.xyzw;
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

        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
    {
        SurfaceDescriptionInputs output;
        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





        output.uv0 =                         input.texCoord0;
        output.VertexColor =                 input.color;
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
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
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
    float4 _Tint;
    float2 Vector2_288FEC1D;
    float Vector1_ACBBFEA9;
    float Vector1_7A60B76E;
    CBUFFER_END
    TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
    SAMPLER(_SampleTexture2D_B4797219_Sampler_3_Linear_Repeat);

        // Graph Functions
        
    void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
    {
        Out = A * B;
    }

    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
    {
        Out = UV * Tiling + Offset;
    }

    void Unity_Multiply_float(float A, float B, out float Out)
    {
        Out = A * B;
    }


    inline float2 Unity_Voronoi_RandomVector_float (float2 UV, float offset)
    {
        float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
        UV = frac(sin(mul(UV, m)) * 46839.32);
        return float2(sin(UV.y*+offset)*0.5+0.5, cos(UV.x*offset)*0.5+0.5);
    }

    void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
    {
        float2 g = floor(UV * CellDensity);
        float2 f = frac(UV * CellDensity);
        float t = 8.0;
        float3 res = float3(8.0, 0.0, 0.0);

        for(int y=-1; y<=1; y++)
        {
            for(int x=-1; x<=1; x++)
            {
                float2 lattice = float2(x,y);
                float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
                float d = distance(lattice + offset, f);

                if(d < res.x)
                {
                    res = float3(d, offset.x, offset.y);
                    Out = res.x;
                    Cells = res.y;
                }
            }
        }
    }

    void Unity_Power_float(float A, float B, out float Out)
    {
        Out = pow(A, B);
    }

    void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
    {
        Out = A * B;
    }

        // Graph Vertex
        // GraphVertex: <None>
        
        // Graph Pixel
        struct SurfaceDescriptionInputs
    {
        float4 uv0;
        float4 VertexColor;
        float3 TimeParameters;
    };

    struct SurfaceDescription
    {
        float Alpha;
        float AlphaClipThreshold;
    };

    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
    {
        SurfaceDescription surface = (SurfaceDescription)0;
        float4 _SampleTexture2D_B4797219_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
        float _SampleTexture2D_B4797219_R_4 = _SampleTexture2D_B4797219_RGBA_0.r;
        float _SampleTexture2D_B4797219_G_5 = _SampleTexture2D_B4797219_RGBA_0.g;
        float _SampleTexture2D_B4797219_B_6 = _SampleTexture2D_B4797219_RGBA_0.b;
        float _SampleTexture2D_B4797219_A_7 = _SampleTexture2D_B4797219_RGBA_0.a;
        float2 _Property_6459D444_Out_0 = Vector2_288FEC1D;
        float2 _Multiply_B7513082_Out_2;
        Unity_Multiply_float(_Property_6459D444_Out_0, (IN.TimeParameters.x.xx), _Multiply_B7513082_Out_2);
        float2 _TilingAndOffset_20F18B0F_Out_3;
        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_B7513082_Out_2, _TilingAndOffset_20F18B0F_Out_3);
        float _Property_9E48D93B_Out_0 = Vector1_ACBBFEA9;
        float _Multiply_C3781331_Out_2;
        Unity_Multiply_float(_Property_9E48D93B_Out_0, IN.TimeParameters.x, _Multiply_C3781331_Out_2);
        float _Property_86EFC281_Out_0 = Vector1_7A60B76E;
        float _Voronoi_47BC6ACC_Out_3;
        float _Voronoi_47BC6ACC_Cells_4;
        Unity_Voronoi_float(_TilingAndOffset_20F18B0F_Out_3, _Multiply_C3781331_Out_2, _Property_86EFC281_Out_0, _Voronoi_47BC6ACC_Out_3, _Voronoi_47BC6ACC_Cells_4);
        float _Power_B3755451_Out_2;
        Unity_Power_float(_Voronoi_47BC6ACC_Out_3, 1.5, _Power_B3755451_Out_2);
        float4 _Multiply_2B883C98_Out_2;
        Unity_Multiply_float(_SampleTexture2D_B4797219_RGBA_0, (_Power_B3755451_Out_2.xxxx), _Multiply_2B883C98_Out_2);
        float4 _Multiply_6A355D3A_Out_2;
        Unity_Multiply_float(IN.VertexColor, _Multiply_2B883C98_Out_2, _Multiply_6A355D3A_Out_2);
        float4 _Property_B06C3993_Out_0 = _Tint;
        float4 _Multiply_434B9292_Out_2;
        Unity_Multiply_float(_Multiply_6A355D3A_Out_2, _Property_B06C3993_Out_0, _Multiply_434B9292_Out_2);
        float _Split_C8A9A842_R_1 = _Multiply_434B9292_Out_2[0];
        float _Split_C8A9A842_G_2 = _Multiply_434B9292_Out_2[1];
        float _Split_C8A9A842_B_3 = _Multiply_434B9292_Out_2[2];
        float _Split_C8A9A842_A_4 = _Multiply_434B9292_Out_2[3];
        surface.Alpha = _Split_C8A9A842_A_4;
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
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };

        // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 texCoord0;
                float4 color;
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
                float4 interp01 : TEXCOORD1;
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
                output.interp01.xyzw = input.color;
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
                output.color = input.interp01.xyzw;
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

        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
    {
        SurfaceDescriptionInputs output;
        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





        output.uv0 =                         input.texCoord0;
        output.VertexColor =                 input.color;
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
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
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
    float4 _Tint;
    float2 Vector2_288FEC1D;
    float Vector1_ACBBFEA9;
    float Vector1_7A60B76E;
    CBUFFER_END
    TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
    SAMPLER(_SampleTexture2D_B4797219_Sampler_3_Linear_Repeat);

        // Graph Functions
        
    void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
    {
        Out = A * B;
    }

    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
    {
        Out = UV * Tiling + Offset;
    }

    void Unity_Multiply_float(float A, float B, out float Out)
    {
        Out = A * B;
    }


    inline float2 Unity_Voronoi_RandomVector_float (float2 UV, float offset)
    {
        float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
        UV = frac(sin(mul(UV, m)) * 46839.32);
        return float2(sin(UV.y*+offset)*0.5+0.5, cos(UV.x*offset)*0.5+0.5);
    }

    void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
    {
        float2 g = floor(UV * CellDensity);
        float2 f = frac(UV * CellDensity);
        float t = 8.0;
        float3 res = float3(8.0, 0.0, 0.0);

        for(int y=-1; y<=1; y++)
        {
            for(int x=-1; x<=1; x++)
            {
                float2 lattice = float2(x,y);
                float2 offset = Unity_Voronoi_RandomVector_float(lattice + g, AngleOffset);
                float d = distance(lattice + offset, f);

                if(d < res.x)
                {
                    res = float3(d, offset.x, offset.y);
                    Out = res.x;
                    Cells = res.y;
                }
            }
        }
    }

    void Unity_Power_float(float A, float B, out float Out)
    {
        Out = pow(A, B);
    }

    void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
    {
        Out = A * B;
    }

        // Graph Vertex
        // GraphVertex: <None>
        
        // Graph Pixel
        struct SurfaceDescriptionInputs
    {
        float4 uv0;
        float4 VertexColor;
        float3 TimeParameters;
    };

    struct SurfaceDescription
    {
        float Alpha;
        float AlphaClipThreshold;
    };

    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
    {
        SurfaceDescription surface = (SurfaceDescription)0;
        float4 _SampleTexture2D_B4797219_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
        float _SampleTexture2D_B4797219_R_4 = _SampleTexture2D_B4797219_RGBA_0.r;
        float _SampleTexture2D_B4797219_G_5 = _SampleTexture2D_B4797219_RGBA_0.g;
        float _SampleTexture2D_B4797219_B_6 = _SampleTexture2D_B4797219_RGBA_0.b;
        float _SampleTexture2D_B4797219_A_7 = _SampleTexture2D_B4797219_RGBA_0.a;
        float2 _Property_6459D444_Out_0 = Vector2_288FEC1D;
        float2 _Multiply_B7513082_Out_2;
        Unity_Multiply_float(_Property_6459D444_Out_0, (IN.TimeParameters.x.xx), _Multiply_B7513082_Out_2);
        float2 _TilingAndOffset_20F18B0F_Out_3;
        Unity_TilingAndOffset_float(IN.uv0.xy, float2 (1, 1), _Multiply_B7513082_Out_2, _TilingAndOffset_20F18B0F_Out_3);
        float _Property_9E48D93B_Out_0 = Vector1_ACBBFEA9;
        float _Multiply_C3781331_Out_2;
        Unity_Multiply_float(_Property_9E48D93B_Out_0, IN.TimeParameters.x, _Multiply_C3781331_Out_2);
        float _Property_86EFC281_Out_0 = Vector1_7A60B76E;
        float _Voronoi_47BC6ACC_Out_3;
        float _Voronoi_47BC6ACC_Cells_4;
        Unity_Voronoi_float(_TilingAndOffset_20F18B0F_Out_3, _Multiply_C3781331_Out_2, _Property_86EFC281_Out_0, _Voronoi_47BC6ACC_Out_3, _Voronoi_47BC6ACC_Cells_4);
        float _Power_B3755451_Out_2;
        Unity_Power_float(_Voronoi_47BC6ACC_Out_3, 1.5, _Power_B3755451_Out_2);
        float4 _Multiply_2B883C98_Out_2;
        Unity_Multiply_float(_SampleTexture2D_B4797219_RGBA_0, (_Power_B3755451_Out_2.xxxx), _Multiply_2B883C98_Out_2);
        float4 _Multiply_6A355D3A_Out_2;
        Unity_Multiply_float(IN.VertexColor, _Multiply_2B883C98_Out_2, _Multiply_6A355D3A_Out_2);
        float4 _Property_B06C3993_Out_0 = _Tint;
        float4 _Multiply_434B9292_Out_2;
        Unity_Multiply_float(_Multiply_6A355D3A_Out_2, _Property_B06C3993_Out_0, _Multiply_434B9292_Out_2);
        float _Split_C8A9A842_R_1 = _Multiply_434B9292_Out_2[0];
        float _Split_C8A9A842_G_2 = _Multiply_434B9292_Out_2[1];
        float _Split_C8A9A842_B_3 = _Multiply_434B9292_Out_2[2];
        float _Split_C8A9A842_A_4 = _Multiply_434B9292_Out_2[3];
        surface.Alpha = _Split_C8A9A842_A_4;
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
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };

        // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 texCoord0;
                float4 color;
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
                float4 interp01 : TEXCOORD1;
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
                output.interp01.xyzw = input.color;
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
                output.color = input.interp01.xyzw;
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

        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
    {
        SurfaceDescriptionInputs output;
        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





        output.uv0 =                         input.texCoord0;
        output.VertexColor =                 input.color;
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
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

        ENDHLSL
    }

    }
    FallBack "Hidden/Shader Graph/FallbackError"
}
