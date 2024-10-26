Shader "Custom/Polybox_WaterFoam_Ocean"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,0.2)
        _MaskTex ("Mask", 2D) = "black" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Specular ("Specular", Range(0,1)) = 5.0
        
        _FoamColor ("Foam Color", Color) = (1,1,1,1)

        [Header(Fade Settings)]
        _FadeoutWidth ("Fadeout Width", Range(0,1)) = 0.5
        _FadeoutPower ("Fadeout Power", Range(0,5)) = 2.0

        [Header(Shore Settings)]
        _ShoreWidth ("Shore Width", Range(0,1)) = 0.5
        _ShorePower ("Shore Power", Range(1,10)) = 2.0
    }
    SubShader
    {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf StandardSpecular alpha:blend vertex:vert nolightmap //fullforwardshadows
        #include "Polybox_Water.cginc"       

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MaskTex;

        struct Input
        {
			float2 wUV : TEXCOORD0;
            float2 uv_MaskTex;
        };

        half _Glossiness;
        half _Specular;
        fixed4 _Color, _FoamColor;

        half  _FadeoutWidth,  _FadeoutPower;
        half  _ShoreWidth,  _ShorePower;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        
		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
            
			float4 wPos = mul(unity_ObjectToWorld, v.vertex);
            o.wUV = wPos.xz * _PolyboxGlobalWaterScale;

            float waveHeight = GetOceanHeight(o.wUV, 0);
            wPos.y += waveHeight;
            v.vertex = mul(unity_WorldToObject, wPos);
        }

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            fixed4 mask = tex2D (_MaskTex, IN.wUV * 0.1);
            
            float t = _Time.y;
            half tFoam = fmod(t * 2, 1);
            //Fadeout
            half fadeMask = fmod(mask.r, 1);
            fadeMask = saturate((_FadeoutWidth - fadeMask) * _FadeoutPower);
            //Shorefoam
            half shoreMask = saturate((mask.g-(1.0 - _ShoreWidth)) * _ShorePower);

            half foamMask = GetFoamShore(t, IN.wUV, 1.5, fadeMask, shoreMask);

            fixed4 c = lerp(_Color, _FoamColor, foamMask * _FoamColor.a);
            o.Albedo = lerp(c.rgb,fixed3(1,1,1),foamMask);

            o.Specular = lerp(_Specular,0,foamMask);
            o.Smoothness = lerp(_Glossiness,0,foamMask);
            o.Alpha = c.a;
        }
        ENDCG
    }
Fallback "Legacy Shaders/Transparent/VertexLit"
}
