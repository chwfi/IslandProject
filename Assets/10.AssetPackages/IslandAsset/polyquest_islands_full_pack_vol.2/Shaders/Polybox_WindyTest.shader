Shader "POLYBOX/FoliageWindy"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        [Header(Wind Settings)]
        _WindBaseHeight ("Root Height", Float) = 0
        _WindHeightBlend ("Height", Float) = 1
        _WindHeightBend ("Height Bend", Float) = 1

        _WindNoiseLeaves ("Noise Leaves", Vector) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard vertex:vert addshadow fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        uniform sampler2D _PolyboxGlobalWindNoiseMap;

        uniform half4 _PolyboxGlobalWind; //X,Y : Wind Dir; W: WindPower
      
        uniform half4 _PolyboxGlobalWindNoise;
        uniform half4 _PolyboxGlobalWindNoiseLeaves;
        uniform half4 _WindNoiseLeaves;

        uniform half _WindBaseHeight;
        uniform half _WindHeightBlend;
        uniform half _WindHeightBend;

        

        float4 SmoothCurve(float4 x) {
            return x * x * (3.0 - 2.0 * x);
        }
        float4 TriangleWave(float4 x) { 
            return abs(frac(x + 0.5) * 2.0 - 1.0);
        }
        float4 SmoothTriangleWave(float4 x) {
            return SmoothCurve(TriangleWave(x));
        }

        struct Input
        {
            float2 uv_MainTex;
        };
        
        void vert (inout appdata_full v) {
            float3 wPos = mul(unity_ObjectToWorld, v.vertex.xyz);
            //float3 worldRoot = mul(unity_ObjectToWorld, float3(0,0,0));
            float baseLength = length(v.vertex.xyz);
            float height = v.vertex.y; //ObjectSpace
            float2 wUV = wPos.xz;

            float heightMask = (height - _WindBaseHeight) * _WindHeightBlend;
            if(heightMask < 0)
                heightMask = 0;
            heightMask = pow(heightMask, _WindHeightBend) / _WindHeightBend;
            
            //fixed4 noise1 = tex2Dlod (_PolyboxGlobalWindNoiseMap, half4(wUV * _NoiseScale.x + _Time.xx * _NoiseFrequency.x + heightMask * _WindHeightBlend, 0, 0));
            //v.vertex.xyz += v.normal * noise1;

            float4 noiseSine = float4(wUV * _PolyboxGlobalWindNoise.x + _Time.xx * _PolyboxGlobalWindNoise.yz,
                wUV * (_PolyboxGlobalWindNoiseLeaves.x * _WindNoiseLeaves.x) + _Time.xx * _PolyboxGlobalWindNoiseLeaves.yz * _WindNoiseLeaves.yz);
            noiseSine = SmoothTriangleWave(noiseSine);
            
            //float noiseWind = (tex2Dlod (_PolyboxGlobalWindNoiseMap, float4(wUV * _PolyboxGlobalWindNoise.x + _Time.xx * _PolyboxGlobalWindNoise.y, 0, 2)).r-0.5) * _PolyboxGlobalWindNoise.w;
            //float noiseLeaves = (tex2Dlod (_PolyboxGlobalWindNoiseMap, float4(wUV * _PolyboxGlobalWindNoiseLeaves.x + _Time.xx * _PolyboxGlobalWindNoiseLeaves.y, 0, 1)).r-0.5) * _PolyboxGlobalWindNoiseLeaves.w;

            float noiseWind = (noiseSine.x+noiseSine.y - 1) * _PolyboxGlobalWindNoise.w;
            float noiseLeaves = (noiseSine.z+noiseSine.w - 1) * (_PolyboxGlobalWindNoiseLeaves.w * _WindNoiseLeaves.w);

            float fBF = heightMask;            
            //fBF += 1.0;
            //fBF *= fBF;
            //fBF = fBF * fBF - fBF;

            float2 windDir = _PolyboxGlobalWind.xy;
            //Base Wind
            wPos.xz += fBF * ((_PolyboxGlobalWind.w + noiseWind) * windDir);
            v.vertex.xyz = normalize(mul(unity_WorldToObject, wPos)) * baseLength;

            //Leaves
            wPos = mul(unity_ObjectToWorld, v.vertex.xyz);
            wPos.y += noiseLeaves * v.color.g;
            v.vertex.xyz = mul(unity_WorldToObject, wPos);

        }
      
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Standard"
}
