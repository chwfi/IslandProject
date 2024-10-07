// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "POLYBOX/Water Simple"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,0.2)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 5.0
        
        [Header(Foam Maps)]
        _FoamColor ("Foam Color", Color) = (1,1,1,1)
        _NoiseMap1 ("Noise Map 1 (RGB)", 2D) = "white" {}
        _FoamScale ("Foam Scale", Float) = 1.0
        _FoamStrength ("Foam Strength ", Range(0,4)) = 1.0
        _FoamSpeed ("Foam Speed", Float) = 1.0
        _FoamSharpness ("Foam Sharpness", Range(1,100)) = 2
        
        [Header(Shore Settings)]
        _ShoreOffset ("Shore Offset", Range(-1, 1)) = 0.0
        _ShoreWidth ("Shore Width", Range(0.5, 5)) = 1.5
        _ShorePower ("Shore Power", Range(1,10)) = 2.0
    }
    SubShader
    {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 200
           
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha:premul keepalpha vertex:vert nolightmap 
        #include "Polybox_Water.cginc"       

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
			float2 wUV : TEXCOORD0;
			float3 worldPos : TEXCOORD1;
			float3 camRelativeWorldPos : TEXCOORD2;
            float3 viewDir;
            float3 worldNormal;

            float4 screenPos;
            float eyeDepth;
        };

        sampler2D_float _CameraDepthTexture;
        float4 _CameraDepthTexture_TexelSize;

        half _Glossiness;
        half _Metallic;
        fixed4 _Color, _FoamColor;

        sampler2D _NoiseMap1;
        half _FoamBlendSpeed, _FoamSpeed, _FoamStrength, _FoamScale;
        half _FoamWidth, _FoamSharpness;
        half _ShoreOffset, _ShoreWidth,  _ShorePower;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        
		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
            COMPUTE_EYEDEPTH(o.eyeDepth);
            
			float4 wPos = mul(unity_ObjectToWorld, v.vertex);
            o.worldPos = wPos.xyz;
            o.wUV = wPos.xz * _PolyboxGlobalWaterScale;

            float waveHeight = GetOceanHeight(o.wUV, 0);
            wPos.y += waveHeight;
            v.vertex = mul(unity_WorldToObject, wPos);
            
            o.camRelativeWorldPos = wPos.xyz - _WorldSpaceCameraPos;
            //o.camRelativeWorldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0)).xyz - _WorldSpaceCameraPos;
        }
        
        float GenerateFoam(half t, half shoreLine, half2 uv1)
        {
            uv1 = uv1 * _FoamScale + _Time.x * _FoamSpeed;
            fixed4 noiseMap = tex2D (_NoiseMap1, uv1);
            float noise = _FoamStrength * noiseMap.r;            
            
            //Finalize
            half foam = saturate(pow(shoreLine * noise, _FoamSharpness));
            return foam;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float t = _Time.y;
            //World Position from Depth
            float rawZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos));       
            float3 viewPlane = IN.camRelativeWorldPos.xyz / dot(IN.camRelativeWorldPos.xyz, unity_WorldToCamera._m20_m21_m22);
            float3 worldPos = viewPlane * LinearEyeDepth(rawZ) + _WorldSpaceCameraPos;

            //Water Shore Effect
            float waterDepth = length(IN.worldPos - worldPos);
            float offsetMask = saturate(_ShorePower*( waterDepth-_ShoreOffset));

            half foamMask = offsetMask * saturate(_ShorePower * (1 -  _ShoreWidth * waterDepth) * offsetMask);
            foamMask = GenerateFoam(0, foamMask, IN.worldPos.xz);

            foamMask *= _FoamColor.a;
            fixed4 c = lerp(_Color, _FoamColor, foamMask);

            o.Albedo = c.rgb;

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;//lerp(_Glossiness, 0, foamMask);
            o.Alpha = c.a;

            o.Smoothness = (1.0 - dot(IN.viewDir, IN.worldNormal)) * o.Smoothness;
        }
        ENDCG
    }
Fallback "Legacy Shaders/Transparent/VertexLit"
//Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"
}
