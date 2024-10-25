float BlendNoiseTexture3(half3 noise, half t)
{
    t = fmod(t, 1);
    t *= 3;
    half n = lerp(noise.r, noise.g, saturate(t));
    n = lerp(n, noise.b, saturate(t-1));
    n = lerp(n, noise.r, saturate(t-2));
    return n.r;
}

float BlendNoiseTexture4(half4 noise, half t)
{
    t = fmod(t, 1);
    t *= 4;
    half n = lerp(noise.r, noise.g, saturate(t));
    n = lerp(n, noise.b, saturate(t-1));
    n = lerp(n, noise.a, saturate(t-2));
    n = lerp(n, noise.r, saturate(t-3));
    return n.r;
}

half FinalizeFoamShore(float noise, half fadeMask, half shoreMask, half foamWidth)
{    
    noise = (abs(0.5 - noise) + fadeMask - shoreMask) * 2 * 1/foamWidth;
    return saturate((1 - noise ) * 100 * foamWidth);
}

half FinalizeFoamSimple(float noise, half foamMask, half foamWidth)
{
    foamMask = (1-foamMask);
    noise = ((abs(0.5 - noise) + foamMask*0.1)*foamMask) * 1/foamWidth;
    return saturate((1 - noise ) * 100 * foamWidth);
}


uniform half _PolyboxGlobalWaterScale;
uniform sampler2D _PolyboxGlobalWaterWaveMap;
uniform sampler2D _PolyboxGlobalWaterWaveNormalMap;
uniform float4 _PolyboxGlobalWaterNoise;

half GetOceanHeight(float2 uv, half t)
{
    half2 uv1 = uv + (_PolyboxGlobalWaterNoise.y * _Time.z) * half2( 0.3, 0.5);
    half height = tex2Dlod (_PolyboxGlobalWaterWaveMap, half4(uv1 * _PolyboxGlobalWaterNoise.x, 2, 1)).a - 0.5;
    height *= _PolyboxGlobalWaterNoise.w;

    return height;
}
float GetOceanMask(float2 uv, half t)
{
    half2 uv1 = uv + (_PolyboxGlobalWaterNoise.y * _Time.z) * half2( 0.3, 0.5);
    float height = tex2Dlod (_PolyboxGlobalWaterWaveMap, half4(uv1 * _PolyboxGlobalWaterNoise.x, 2, 1)).a - 0.5;
    height *= _PolyboxGlobalWaterNoise.w;

    return height;
}

uniform sampler2D _PolyboxGlobalWaterFoamMap;
float GetFoamShore(half t, half2 uv, half scale, half fadeMask, half shoreMask)
{
    float waveMask = GetOceanMask(uv, t);

    half2 uv1 = uv * scale;    
    fixed4 noiseMap = tex2D (_PolyboxGlobalWaterFoamMap, uv1);

    float noise = noiseMap.r;//BlendNoiseTexture3(noiseMap, t * 0.1);

    float mask = saturate(waveMask * (shoreMask-fadeMask) * 2);
    return FinalizeFoamSimple(noise, mask, 0.1); //FinalizeFoamShore(noise, fadeMask, shoreMask, 0.1);
}
float GetFoamSimple(half t, half2 uv, half foamMask)
{
    half2 uv1 = uv;    
    fixed4 noiseMap = tex2D (_PolyboxGlobalWaterFoamMap, uv1);

    float noise = noiseMap.r;//BlendNoiseTexture3(noiseMap, t * 0.1);

    return FinalizeFoamSimple(noise, foamMask, 0.1);
}

float GetWorldPosFromLinearDepth(float sceneZ, float3 camRelativeWorldPos, float3 worldSpaceCameraPos, float3 unityTrans) {
    // calculate the view plane vector
    // note: Something like normalize(i.camRelativeWorldPos.xyz) is what you'll see other
    // examples do, but that is wrong! You need a vector that at a 1 unit view depth, not
    // a1 unit magnitude.
    float3 viewPlane = camRelativeWorldPos.xyz / dot(camRelativeWorldPos.xyz, unityTrans);

    // calculate the world position
    // multiply the view plane by the linear depth to get the camera relative world space position
    // add the world space camera position to get the world space position from the depth texture
    return viewPlane * sceneZ + worldSpaceCameraPos;
}