// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "POLYBOX/Shorline"
{
	Properties
	{
		_Tiling("Tiling", Vector) = (1,1,0,0)
		[Header(Coloring)]_ShallowWaterColor("ShallowWaterColor", Color) = (0,0.5810094,1,0)
		_DeepWaterColor("DeepWaterColor", Color) = (0,0.31591,0.8301887,0)
		_FoamColor("Foam Color", Color) = (1,1,1,1)
		_FalseShadowColor("FalseShadowColor", Color) = (0.3113208,0.3113208,0.3113208,1)
		_DeepWaterDistance("DeepWaterDistance", Float) = 1
		[Header(WaveShape)]_WaveThickness("WaveThickness", Range( 0.0001 , 0.5)) = 0.213
		_WarbleStrength("WarbleStrength", Float) = 0.03
		_WaveOffsetFrequency1("WaveOffsetFrequency1", Float) = 1
		_WaveOffsetStrength1("WaveOffsetStrength1", Float) = 0.15
		_WaveOffsetFrequency2("WaveOffsetFrequency2", Float) = 0.1
		_WaveOffsetStrength2("WaveOffsetStrength2", Float) = 0.1
		_ShorelineNoiseStrength("ShorelineNoiseStrength", Float) = 0.03
		_ShorelineNoiseFrequency("ShorelineNoiseFrequency", Float) = 0.03
		_WaveRollSpeed("WaveRollSpeed", Float) = 3
		[Header(FoamShape)]_FoamDetailNoise("FoamDetailNoise", 2D) = "gray" {}
		_FoamSoftness("FoamSoftness", Range( 0.001 , 1)) = 0.05
		_FoamNoiseScaling("FoamNoiseScaling", Float) = 1
		_FoamNoiseStretch("FoamNoiseStretch", Float) = 2
		_FoamNoiseClipMin("FoamNoiseClipMin", Range( -0.5 , 1)) = 0
		_FoamNoiseClipMax("FoamNoiseClipMax", Range( 0 , 1.5)) = 1
		_FoamFadeStretch("FoamFadeStretch", Float) = 2.2
		[Header(False Shadow)]_FalseShadowWidth("FalseShadowWidth", Float) = 0.02
		_FalseShadowFallStart("FalseShadowFallStart", Float) = 0.35
		_FalseShadowFallEnd("FalseShadowFallEnd", Float) = 0.44
		_FalseShadowFadeSpeed("FalseShadowFadeSpeed", Float) = 6
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _FoamColor;
		uniform float4 _ShallowWaterColor;
		uniform float4 _DeepWaterColor;
		uniform float2 _Tiling;
		uniform float _DeepWaterDistance;
		uniform float4 _FalseShadowColor;
		uniform float _ShorelineNoiseFrequency;
		uniform float _ShorelineNoiseStrength;
		uniform float _WarbleStrength;
		uniform float _WaveOffsetFrequency1;
		uniform float _WaveOffsetStrength1;
		uniform float _WaveOffsetFrequency2;
		uniform float _WaveOffsetStrength2;
		uniform float _WaveThickness;
		uniform float _FalseShadowFallStart;
		uniform float _FalseShadowFallEnd;
		uniform float _FalseShadowWidth;
		uniform float _FalseShadowFadeSpeed;
		uniform sampler2D _FoamDetailNoise;
		uniform float _FoamFadeStretch;
		uniform float _WaveRollSpeed;
		uniform float _FoamNoiseScaling;
		uniform float _FoamNoiseStretch;
		uniform float _FoamNoiseClipMin;
		uniform float _FoamNoiseClipMax;
		uniform float _FoamSoftness;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 FoamColor480 = _FoamColor;
			float4 appendResult534 = (float4((FoamColor480).rgb , 0.0));
			float2 FullUV283 = ( i.uv_texcoord * _Tiling );
			float4 lerpResult423 = lerp( _ShallowWaterColor , _DeepWaterColor , saturate( (0.0 + (FullUV283.y - 0.0) * (1.0 - 0.0) / (_DeepWaterDistance - 0.0)) ));
			float2 break286 = FullUV283;
			float temp_output_541_0 = ( break286.x * 6.28318548202515 );
			float2 appendResult537 = (float2(sin( temp_output_541_0 ) , cos( temp_output_541_0 )));
			float simplePerlin2D258 = snoise( appendResult537 );
			simplePerlin2D258 = simplePerlin2D258*0.5 + 0.5;
			float4 temp_cast_1 = (( break286.y * ( 1.0 + ( simplePerlin2D258 * 0.2 ) ) )).xxxx;
			float simplePerlin2D264 = snoise( appendResult537*_ShorelineNoiseFrequency );
			simplePerlin2D264 = simplePerlin2D264*0.5 + 0.5;
			float temp_output_277_0 = ( temp_output_541_0 + 12.3 );
			float2 appendResult540 = (float2(sin( temp_output_277_0 ) , cos( temp_output_277_0 )));
			float simplePerlin2D275 = snoise( appendResult540*_ShorelineNoiseFrequency );
			simplePerlin2D275 = simplePerlin2D275*0.5 + 0.5;
			float4 appendResult276 = (float4(simplePerlin2D264 , simplePerlin2D275 , simplePerlin2D264 , simplePerlin2D275));
			float mulTime49 = _Time.y * 0.1;
			float4 appendResult182 = (float4(mulTime49 , ( mulTime49 + 0.25 ) , ( mulTime49 + 0.5 ) , ( mulTime49 + 0.75 )));
			float2 break288 = ( FullUV283 * 6.28318548202515 );
			float2 appendResult547 = (float2(sin( break288.x ) , cos( break288.x )));
			float mulTime257 = _Time.y * 0.1;
			float2 appendResult255 = (float2(0.0 , ( ( break288.y * _WarbleStrength ) + mulTime257 )));
			float simplePerlin2D251 = snoise( ( appendResult547 + appendResult255 )*_WaveOffsetFrequency1 );
			simplePerlin2D251 = simplePerlin2D251*0.5 + 0.5;
			float temp_output_254_0 = ( simplePerlin2D251 * _WaveOffsetStrength1 );
			float2 appendResult295 = (float2(0.0 , -2.0));
			float simplePerlin2D291 = snoise( ( appendResult547 + appendResult295 )*_WaveOffsetFrequency2 );
			simplePerlin2D291 = simplePerlin2D291*0.5 + 0.5;
			float temp_output_293_0 = ( temp_output_254_0 + ( simplePerlin2D291 * _WaveOffsetStrength2 ) );
			float2 appendResult301 = (float2(0.0 , -10.0));
			float simplePerlin2D297 = snoise( ( appendResult547 + appendResult301 )*_WaveOffsetFrequency2 );
			simplePerlin2D297 = simplePerlin2D297*0.5 + 0.5;
			float temp_output_298_0 = ( temp_output_254_0 + ( simplePerlin2D297 * _WaveOffsetStrength2 ) );
			float4 appendResult300 = (float4(temp_output_293_0 , temp_output_298_0 , temp_output_293_0 , temp_output_298_0));
			float4 Input53 = frac( ( frac( appendResult182 ) + appendResult300 ) );
			float4 temp_output_62_0 = abs( ( ( Input53 * float4( 2,2,2,2 ) ) - float4( 1,1,1,1 ) ) );
			float4 Output54 = ( temp_output_62_0 * temp_output_62_0 * ( 1.0 - ( step( float4( 0.5,0.5,0.5,0.5 ) , Input53 ) * float4( 0.8,0.8,0.8,0.8 ) ) ) );
			float4 WaveGradient117 = ( ( ( temp_cast_1 - ( appendResult276 * _ShorelineNoiseStrength * ( 1.0 - break286.y ) ) ) - Output54 ) * ( 1.0 / _WaveThickness ) );
			float4 break209 = WaveGradient117;
			float4 appendResult210 = (float4(break209.w , break209.x , break209.y , break209.z));
			float4 temp_cast_2 = (0.02).xxxx;
			float4 temp_output_456_0 = abs( ( appendResult210 - temp_cast_2 ) );
			float4 break465 = Input53;
			float4 appendResult466 = (float4(break465.w , break465.x , break465.y , break465.z));
			float4 temp_cast_3 = (_FalseShadowFallStart).xxxx;
			float4 temp_cast_4 = (_FalseShadowFallEnd).xxxx;
			float4 smoothstepResult495 = smoothstep( temp_output_456_0 , ( temp_output_456_0 + 0.01 ) , ( ( 1.0 - saturate( (float4( 0,0,0,0 ) + (appendResult466 - temp_cast_3) * (float4( 1,1,1,1 ) - float4( 0,0,0,0 )) / (temp_cast_4 - temp_cast_3)) ) ) * _FalseShadowWidth * min( ( appendResult466 * _FalseShadowFadeSpeed ) , float4( 1,1,1,1 ) ) ));
			float4 WaveFrontShadowMask431 = smoothstepResult495;
			float4 break483 = WaveFrontShadowMask431;
			float4 lerpResult488 = lerp( lerpResult423 , _FalseShadowColor , max( max( break483.x , break483.y ) , max( break483.z , break483.w ) ));
			float4 WaterColor418 = lerpResult488;
			float4 temp_output_369_0 = ( Input53 * float4( 2,2,2,2 ) );
			float4 temp_output_376_0 = ( saturate( ( temp_output_369_0 - float4( 1,1,1,1 ) ) ) * _FoamFadeStretch );
			float4 temp_output_372_0 = ( 1.0 - saturate( temp_output_369_0 ) );
			float4 break190 = ( ( WaveGradient117 * ( 1.0 - ( temp_output_376_0 * temp_output_376_0 ) ) ) + ( ( 1.0 - ( temp_output_372_0 * temp_output_372_0 ) ) * _WaveRollSpeed ) );
			float2 appendResult156 = (float2(FullUV283.x , break190.x));
			float2 appendResult554 = (float2(_FoamNoiseStretch , 1.0));
			float2 temp_output_248_0 = ( _FoamNoiseScaling * appendResult554 );
			float2 temp_output_242_0 = ( appendResult156 * temp_output_248_0 );
			float2 appendResult193 = (float2(FullUV283.x , break190.y));
			float2 temp_output_243_0 = ( appendResult193 * temp_output_248_0 );
			float2 appendResult220 = (float2(FullUV283.x , break190.z));
			float2 temp_output_244_0 = ( appendResult220 * temp_output_248_0 );
			float2 appendResult222 = (float2(FullUV283.x , break190.w));
			float2 temp_output_245_0 = ( appendResult222 * temp_output_248_0 );
			float4 appendResult522 = (float4(tex2D( _FoamDetailNoise, temp_output_242_0 ).r , tex2D( _FoamDetailNoise, temp_output_243_0 ).r , tex2D( _FoamDetailNoise, temp_output_244_0 ).r , tex2D( _FoamDetailNoise, temp_output_245_0 ).r));
			float4 temp_cast_5 = (_FoamNoiseClipMin).xxxx;
			float4 temp_cast_6 = (_FoamNoiseClipMax).xxxx;
			float4 temp_output_131_0 = ( 1.0 - abs( (float4( -1,-1,-1,-1 ) + (appendResult522 - temp_cast_5) * (float4( 1,1,1,1 ) - float4( -1,-1,-1,-1 )) / (temp_cast_6 - temp_cast_5)) ) );
			float4 temp_output_311_0 = ( temp_output_131_0 + -_FoamSoftness );
			float4 temp_cast_7 = (0.48).xxxx;
			float4 temp_cast_8 = (0.85).xxxx;
			float4 Fade174 = saturate( (float4( 0,0,0,0 ) + (Input53 - temp_cast_7) * (float4( 1,1,1,1 ) - float4( 0,0,0,0 )) / (temp_cast_8 - temp_cast_7)) );
			float4 temp_cast_9 = (0.5).xxxx;
			float4 temp_output_352_0 = (float4( 0,0,0,0 ) + (Fade174 - float4( 0,0,0,0 )) * (temp_cast_9 - float4( 0,0,0,0 )) / (float4( 1,1,1,1 ) - float4( 0,0,0,0 )));
			float4 temp_cast_10 = (0.5).xxxx;
			float4 temp_cast_11 = (0.2).xxxx;
			float4 smoothstepResult382 = smoothstep( temp_output_131_0 , temp_output_311_0 , saturate( (float4( 0,0,0,0 ) + (WaveGradient117 - temp_output_352_0) * (float4( 1,1,1,1 ) - float4( 0,0,0,0 )) / (( temp_output_352_0 - temp_cast_11 ) - temp_output_352_0)) ));
			float4 pulbackMask383 = smoothstepResult382;
			float4 temp_cast_12 = (0.09).xxxx;
			float4 temp_cast_13 = (0.1).xxxx;
			float4 smoothstepResult399 = smoothstep( temp_cast_12 , temp_cast_13 , WaveGradient117);
			float4 WaveFrontMask400 = smoothstepResult399;
			float4 break386 = min( pulbackMask383 , WaveFrontMask400 );
			float4 lerpResult407 = lerp( appendResult534 , WaterColor418 , saturate( max( max( break386.x , break386.y ) , max( break386.z , break386.w ) ) ));
			float4 temp_output_147_0 = ( 1.0 - saturate( ( Input53 * float4( 2,2,2,2 ) ) ) );
			float4 temp_output_141_0 = (float4( 0,0,0,0 ) + (WaveGradient117 - float4( 0,0,0,0 )) * (float4( 1,1,1,1 ) - float4( 0,0,0,0 )) / (( 1.0 - ( temp_output_147_0 * temp_output_147_0 ) ) - float4( 0,0,0,0 )));
			float4 temp_cast_14 = (0.48).xxxx;
			float4 temp_cast_15 = (( 0.48 + -0.01 )).xxxx;
			float4 smoothstepResult314 = smoothstep( temp_cast_14 , temp_cast_15 , abs( ( WaveGradient117 - float4( 0.5,0.5,0.5,0.5 ) ) ));
			float4 WaveMask149 = smoothstepResult314;
			float4 smoothstepResult310 = smoothstep( temp_output_131_0 , temp_output_311_0 , ( 1.0 - ( ( 1.0 - ( temp_output_141_0 + ( ( 1.0 - saturate( ( Input53 * float4( 3.5,3.5,3.5,3.5 ) ) ) ) * float4( 0.4,0.4,0.4,0.4 ) ) ) ) * ( 1.0 - Fade174 ) ) ));
			float4 NoiseFoam120 = ( ( 1.0 - temp_output_141_0 ) * WaveMask149 * smoothstepResult310 * smoothstepResult382 );
			float4 TailFront206 = step( ( ( -0.03 + appendResult210 ) - ( step( Input53 , float4( 0.5,0.5,0.5,0.5 ) ) * float4( 100,100,100,100 ) ) ) , float4( 0,0,0,0 ) );
			float4 break236 = ( ( NoiseFoam120 - WaveFrontShadowMask431 ) * TailFront206 );
			float4 lerpResult405 = lerp( lerpResult407 , FoamColor480 , saturate( ( max( max( break236.x , break236.y ) , max( break236.z , break236.w ) ) * 1.3 ) ));
			o.Albedo = lerpResult405.rgb;
			o.Alpha = (lerpResult405).a;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18912
1009;73;910;936;1697.418;-534.2829;1;True;False
Node;AmplifyShaderEditor.TexCoordVertexDataNode;281;-1565.323,-470.1098;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;280;-1515.693,-278.5155;Inherit;False;Property;_Tiling;Tiling;0;0;Create;True;0;0;0;False;0;False;1,1;2,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;282;-1356.693,-335.5155;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;283;-1060.41,-370.8307;Inherit;False;FullUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;287;-2055.409,785.4182;Inherit;False;283;FullUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TauNode;546;-2044.497,928.2108;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;545;-1899.497,821.2108;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;288;-1648.142,817.6483;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;517;-1844.153,1050.271;Inherit;False;Property;_WarbleStrength;WarbleStrength;7;0;Create;True;0;0;0;False;0;False;0.03;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;263;-1554.27,1033.251;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.03;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;257;-1614.782,1156.788;Inherit;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;262;-1399.038,1004.86;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;544;-1466.497,865.2108;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;543;-1469.497,797.2108;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;295;-1277.013,1030.606;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;-2;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;301;-1267.934,1132.728;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;-10;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;547;-1289.497,820.2108;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;255;-1286.708,920.0908;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;511;-1175.047,1278.097;Inherit;False;Property;_WaveOffsetFrequency2;WaveOffsetFrequency2;10;0;Create;True;0;0;0;False;0;False;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;512;-1165.255,761.9629;Inherit;False;Property;_WaveOffsetFrequency1;WaveOffsetFrequency1;8;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;548;-1129.497,843.2108;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;550;-1097.497,969.2108;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;549;-1090.497,1081.211;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;513;-935.8984,713.734;Inherit;False;Property;_WaveOffsetStrength1;WaveOffsetStrength1;9;0;Create;True;0;0;0;False;0;False;0.15;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;251;-925.707,812.0908;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;297;-897.9382,1054.242;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;515;-898.1526,1168.271;Inherit;False;Property;_WaveOffsetStrength2;WaveOffsetStrength2;11;0;Create;True;0;0;0;False;0;False;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;49;-1060.421,214.1332;Inherit;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;291;-904.7564,947.0884;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;299;-702.9156,1013.268;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;254;-722.6835,795.5678;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.15;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;217;-902.6068,459.9127;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.75;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;292;-717.4909,911.2742;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;183;-899.1134,278.9191;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;216;-900.6068,368.9127;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;293;-554.7597,806.3452;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;182;-756.1134,228.9191;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;298;-550.9156,922.2684;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;300;-418.9156,864.7458;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FractNode;498;-566.3905,320.7962;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;285;-898.935,-727.4228;Inherit;False;283;FullUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;250;-299.2545,323.2781;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;286;-744.637,-705.1606;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TauNode;542;-945.6254,-501.2571;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;50;-130.9606,256.8827;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;53;70.10902,290.1243;Inherit;False;Input;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;541;-830.6254,-531.2571;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;55;-71.72403,957.1299;Inherit;False;53;Input;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CosOpNode;536;-733.6254,-343.2571;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;535;-731.6254,-421.2571;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;277;-835.4286,-132.6897;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;12.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;539;-723.6255,-65.25705;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;346;227.1503,1088.062;Inherit;False;2;0;FLOAT4;0.5,0.5,0.5,0.5;False;1;FLOAT4;0.5,0.5,0.5,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SinOpNode;538;-721.6255,-143.257;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;537;-602.6254,-409.2571;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;129.8702,960.2027;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;2,2,2,2;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;510;-696.5604,31.16972;Inherit;False;Property;_ShorelineNoiseFrequency;ShorelineNoiseFrequency;13;0;Create;True;0;0;0;False;0;False;0.03;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;58;261.8701,961.2027;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;540;-608.6255,-116.257;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;320;369.441,1159.585;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.8,0.8,0.8,0.8;False;1;FLOAT4;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;258;-448.1798,-388.2286;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;62;415.0668,959.6695;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;321;524.441,1128.585;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;264;-442.5083,-171.4344;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;275;-452.3394,-70.84874;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;260;-219.095,-322.1436;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;259;-77.09492,-382.1436;Inherit;False;2;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;278;-235.6749,21.64673;Inherit;False;Property;_ShorelineNoiseStrength;ShorelineNoiseStrength;12;0;Create;True;0;0;0;False;0;False;0.03;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;354;-1741.497,2211.7;Inherit;False;53;Input;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;270;-259.8513,-460.6117;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;603.4316,956.4349;Inherit;False;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;276;-209.4286,-164.6897;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;44.89151,-497.9351;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;54;767.1183,947.705;Inherit;False;Output;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;369;-1573.679,2232.748;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;2,2,2,2;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;267;-50.99397,-172.1402;Inherit;False;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0.03;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;374;-1440.385,2299.688;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;274;183.4792,-409.0934;Inherit;False;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;194.2872,-286.7691;Inherit;False;54;Output;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;143;100.7746,-126.2497;Inherit;False;Property;_WaveThickness;WaveThickness;6;1;[Header];Create;True;1;WaveShape;0;0;False;0;False;0.213;0.213;0.0001;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;380;-1327.213,2399.701;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;370;-1391.88,2120.347;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;67;416.8026,-349.3846;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;145;409.5762,-163.968;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;377;-1388.185,2488.289;Inherit;False;Property;_FoamFadeStretch;FoamFadeStretch;21;0;Create;True;0;0;0;False;0;False;2.2;2.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;376;-1183.885,2423.489;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;564.5521,-328.4171;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;372;-1268.444,2127.143;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;117;713.9782,-685.476;Inherit;False;WaveGradient;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;381;-984.2129,2434.701;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;371;-1133.444,2128.143;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;378;-869.8728,2384.925;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;373;-1012.444,2134.143;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;358;-1067.723,2275.729;Inherit;False;Property;_WaveRollSpeed;WaveRollSpeed;14;0;Create;True;0;0;0;False;0;False;3;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;158;-979.2289,1931.574;Inherit;True;117;WaveGradient;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;136;2.713543,1419.051;Inherit;False;53;Input;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;379;-747.2971,2011.222;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;357;-860.8796,2182.347;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;185.8829,1416.402;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;2,2,2,2;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;555;-534.6271,2507.166;Inherit;False;Property;_FoamNoiseStretch;FoamNoiseStretch;18;0;Create;True;0;0;0;False;0;False;2;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;289;-570.8677,1635.974;Inherit;False;283;FullUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;356;-660.8796,2159.347;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;139;306.7946,1411.455;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;272;145.4853,432.8317;Inherit;False;Constant;_FadeStart;FadeStart;2;0;Create;True;0;0;0;False;0;False;0.48;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;290;-413.6013,1661.204;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.BreakToComponentsNode;190;-434.6129,2056.969;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;273;155.4853,514.8317;Inherit;False;Constant;_FadeEnd;FadeEnd;2;0;Create;True;0;0;0;False;0;False;0.85;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;247;-426.7865,2378.422;Inherit;False;Property;_FoamNoiseScaling;FoamNoiseScaling;17;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;554;-329.6271,2523.166;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;222;-171.9588,2256.365;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;147;436.1465,1421.877;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;248;-172.7865,2444.422;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;220;-173.9588,2150.365;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;193;-170.7684,2043.561;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;306;192.0706,1577.531;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;3.5,3.5,3.5,3.5;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;156;-166.11,1932.984;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;271;414.6432,408.6511;Inherit;False;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;242;34.10048,1966.795;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;508;-16.02383,1711.099;Inherit;True;Property;_FoamDetailNoise;FoamDetailNoise;15;1;[Header];Create;True;1;FoamShape;0;0;False;0;False;None;None;False;gray;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;245;32.50043,2283.595;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;243;27.80039,2064.396;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;324;625.6854,410.7411;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;244;35.70048,2168.395;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;307;310.9823,1573.584;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;581.4474,1420.711;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;519;254.3005,1928.372;Inherit;True;Property;_TextureSample1;Texture Sample 1;20;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;518;253.8102,1745.437;Inherit;True;Property;_TextureSample0;Texture Sample 0;19;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;122;718.2327,1319.341;Inherit;False;117;WaveGradient;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;521;255.79,2298.937;Inherit;True;Property;_TextureSample3;Texture Sample 3;21;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;148;713.9838,1544.875;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;459;1324.855,-1148.01;Inherit;False;53;Input;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;174;765.7648,399.8941;Inherit;False;Fade;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;308;441.3342,1584.006;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;520;256.3005,2116.372;Inherit;True;Property;_TextureSample2;Texture Sample 2;19;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;353;688.4979,1239.2;Inherit;False;Constant;_PullbackDistance;PullbackDistance;9;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;523;745.2507,2096.059;Inherit;False;Property;_FoamNoiseClipMin;FoamNoiseClipMin;19;0;Create;True;0;0;0;False;0;False;0;-0.2;-0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;465;1482.322,-1144.896;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TFHCRemapNode;141;879.399,1463.894;Inherit;False;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;524;746.2507,2177.059;Inherit;False;Property;_FoamNoiseClipMax;FoamNoiseClipMax;20;0;Create;True;0;0;0;False;0;False;1;1;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;325;708.9086,1116.629;Inherit;False;174;Fade;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;522;583.2602,1833.048;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;309;633.9113,1661.469;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.4,0.4,0.4,0.4;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;304;1194.695,1589.733;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;315;1053.427,57.75293;Inherit;False;Constant;_Float1;Float 1;3;0;Create;True;0;0;0;False;0;False;-0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;471;1839.602,-1086.225;Inherit;False;Property;_FalseShadowFallStart;FalseShadowFallStart;23;0;Create;True;0;0;0;False;0;False;0.35;0.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;472;1842.566,-1016.034;Inherit;False;Property;_FalseShadowFallEnd;FalseShadowFallEnd;24;0;Create;True;0;0;0;False;0;False;0.44;0.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;184;927.9666,-323.6116;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.5,0.5,0.5,0.5;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;175;1287.361,1746.985;Inherit;False;174;Fade;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;228;1048.523,-216.3852;Inherit;False;Constant;_MASKSTEP;MASKSTEP;1;0;Create;True;0;0;0;False;0;False;0.48;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;129;750.5455,1850.716;Inherit;False;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;3;FLOAT4;-1,-1,-1,-1;False;4;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCRemapNode;352;893.4979,1106.2;Inherit;False;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;339;1122.499,1357.116;Inherit;False;Constant;_PullbackWidth;PullbackWidth;4;0;Create;True;0;0;0;False;0;False;0.2;20.38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;466;1600.322,-1146.896;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;209;1120.827,-1336.655;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.AbsOpNode;185;1060.967,-315.6116;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.AbsOpNode;130;922.5454,1846.716;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;347;1310.569,1272.799;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;312;1213.826,2023.581;Inherit;False;Property;_FoamSoftness;FoamSoftness;16;0;Create;True;0;0;0;False;0;False;0.05;0.05;0.001;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;210;1238.827,-1338.655;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;490;1189.379,-785.3644;Inherit;False;Constant;_constshadowoffset;constshadowoffset;3;0;Create;True;0;0;0;False;0;False;0.02;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;350;1475.744,1661.154;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;492;1482.766,-953.5718;Inherit;False;Property;_FalseShadowFadeSpeed;FalseShadowFadeSpeed;25;0;Create;True;0;0;0;False;0;False;6;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;348;1459.649,1734.954;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCRemapNode;470;2050.165,-1140.694;Inherit;True;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;313;1226.273,-23.28132;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;338;1454.787,1214.92;Inherit;False;5;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,1;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SmoothstepOpNode;314;1432.787,-257.7934;Inherit;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;473;2315.382,-1056.306;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;213;1091.067,-1581.236;Inherit;False;53;Input;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;478;1413.961,-821.7802;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.NegateNode;525;1514.363,1976.55;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;131;1029.626,1844.666;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;491;1705.766,-945.5718;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;349;1612.882,1686.045;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.AbsOpNode;456;1626.819,-745.5576;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMinOpNode;493;1979.103,-868.5902;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;311;1659.571,1845.646;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;342;1631.766,1224.03;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;351;1746.371,1656.897;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;497;2529.016,-652.8033;Inherit;False;Constant;_Float3;Float 3;3;0;Create;True;0;0;0;False;0;False;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;476;2480.3,-1008.591;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.StepOpNode;231;1246.56,-1577.133;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.5,0.5,0.5,0.5;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;212;1224.521,-1445.004;Inherit;False;Constant;_TailFontBuffer;TailFontBuffer;1;0;Create;True;0;0;0;False;0;False;-0.03;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;475;2342.014,-943.3405;Inherit;False;Property;_FalseShadowWidth;FalseShadowWidth;22;1;[Header];Create;True;1;False Shadow;0;0;False;0;False;0.02;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;149;1670.913,-250.6416;Inherit;False;WaveMask;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;211;1486.699,-1335.417;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.OneMinusNode;133;1106.231,1463.129;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;234;1394.015,-1572.057;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;100,100,100,100;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SmoothstepOpNode;382;1796.29,1322.233;Inherit;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;496;2712.34,-750.026;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SmoothstepOpNode;310;1898.211,1699.574;Inherit;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;474;2674.638,-947.4531;Inherit;False;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;152;1877.5,1537.545;Inherit;False;149;WaveMask;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SmoothstepOpNode;495;2878.706,-850.3507;Inherit;True;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;404;1009.379,-526.5436;Inherit;False;Constant;_Float2;Float 2;3;0;Create;True;0;0;0;False;0;False;0.09;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;233;1527.011,-1422.083;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;2146.286,1457.271;Inherit;True;4;4;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;403;1016.436,-455.0187;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;428;2947.199,1334.686;Inherit;False;283;FullUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StepOpNode;203;1687.702,-1354.694;Inherit;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;431;3211.457,-703.2065;Inherit;False;WaveFrontShadowMask;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SmoothstepOpNode;399;1194.717,-594.3355;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;120;2406.035,1423.509;Inherit;False;NoiseFoam;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;482;3317.954,644.3923;Inherit;False;431;WaveFrontShadowMask;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;206;1937.809,-1393.237;Inherit;False;TailFront;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;400;1513.845,-572.8479;Inherit;False;WaveFrontMask;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;419;2967.747,1197.444;Inherit;False;Property;_DeepWaterDistance;DeepWaterDistance;5;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;426;3099.266,1343.864;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;121;1450.797,99.3391;Inherit;True;120;NoiseFoam;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;438;1440.197,6.571737;Inherit;False;431;WaveFrontShadowMask;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;383;2202.936,1239.125;Inherit;False;pulbackMask;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCRemapNode;420;3236.747,1163.144;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;483;3552.793,675.6278;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;385;1291.693,619.2672;Inherit;False;383;pulbackMask;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;390;1305.649,785.4856;Inherit;True;400;WaveFrontMask;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;477;1697.184,69.33698;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;208;1446.607,297.9964;Inherit;True;206;TailFront;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMinOpNode;401;1518.376,694.9975;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;406;3410.905,1629.924;Inherit;False;Property;_FoamColor;Foam Color;3;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;176;1693.519,203.9753;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;422;3437.747,1159.444;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;485;3686.793,769.6278;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;424;3283.236,981.5372;Inherit;False;Property;_DeepWaterColor;DeepWaterColor;2;0;Create;True;0;0;0;False;0;False;0,0.31591,0.8301887,0;0,0.3159089,0.8301887,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMaxOpNode;484;3694.793,680.6278;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;408;3280.389,810.3474;Inherit;False;Property;_ShallowWaterColor;ShallowWaterColor;1;1;[Header];Create;True;1;Coloring;0;0;False;0;False;0,0.5810094,1,0;0,0.5810093,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;236;1810.303,296.3902;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMaxOpNode;486;3812.793,736.6278;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;423;3623.747,1056.444;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;480;3637.824,1637.441;Inherit;False;FoamColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;487;3578.693,1253.728;Inherit;False;Property;_FalseShadowColor;FalseShadowColor;4;0;Create;True;0;0;0;False;0;False;0.3113208,0.3113208,0.3113208,1;0.3113197,0.3113197,0.3113197,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;386;1776.286,570.7555;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMaxOpNode;388;1918.286,575.7555;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;481;2047.137,81.6263;Inherit;False;480;FoamColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;237;1952.303,301.3902;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;488;3910.158,988.4291;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;387;1910.286,664.7555;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;238;1944.303,390.3902;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;389;2036.286,631.7555;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;532;2120.037,216.1811;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;418;4062.206,1041.291;Inherit;False;WaterColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;239;2070.303,357.3902;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;409;2174.392,688.3422;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;417;2162.385,586.0243;Inherit;False;418;WaterColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;534;2295.037,299.1811;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;415;2061.822,473.7081;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;410;2223.098,434.6418;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;407;2436.1,591.9034;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;405;2554.982,301.5068;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;500;2779.278,556.4987;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;502;-1842.85,-763.155;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NegateNode;504;-1605.772,-666.9773;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;506;-1324.196,-590.6188;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;224;324.344,2940.767;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;123;315.9016,2670.338;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;192;588.3782,2006.28;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;528;2242.933,807.871;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;195;320.9364,2802.4;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;225;324.3441,3060.61;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;505;-1428.161,-696.7283;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3037.032,355.1512;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;POLYBOX/Shorline;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;282;0;281;0
WireConnection;282;1;280;0
WireConnection;283;0;282;0
WireConnection;545;0;287;0
WireConnection;545;1;546;0
WireConnection;288;0;545;0
WireConnection;263;0;288;1
WireConnection;263;1;517;0
WireConnection;262;0;263;0
WireConnection;262;1;257;0
WireConnection;544;0;288;0
WireConnection;543;0;288;0
WireConnection;547;0;543;0
WireConnection;547;1;544;0
WireConnection;255;1;262;0
WireConnection;548;0;547;0
WireConnection;548;1;255;0
WireConnection;550;0;547;0
WireConnection;550;1;295;0
WireConnection;549;0;547;0
WireConnection;549;1;301;0
WireConnection;251;0;548;0
WireConnection;251;1;512;0
WireConnection;297;0;549;0
WireConnection;297;1;511;0
WireConnection;291;0;550;0
WireConnection;291;1;511;0
WireConnection;299;0;297;0
WireConnection;299;1;515;0
WireConnection;254;0;251;0
WireConnection;254;1;513;0
WireConnection;217;0;49;0
WireConnection;292;0;291;0
WireConnection;292;1;515;0
WireConnection;183;0;49;0
WireConnection;216;0;49;0
WireConnection;293;0;254;0
WireConnection;293;1;292;0
WireConnection;182;0;49;0
WireConnection;182;1;183;0
WireConnection;182;2;216;0
WireConnection;182;3;217;0
WireConnection;298;0;254;0
WireConnection;298;1;299;0
WireConnection;300;0;293;0
WireConnection;300;1;298;0
WireConnection;300;2;293;0
WireConnection;300;3;298;0
WireConnection;498;0;182;0
WireConnection;250;0;498;0
WireConnection;250;1;300;0
WireConnection;286;0;285;0
WireConnection;50;0;250;0
WireConnection;53;0;50;0
WireConnection;541;0;286;0
WireConnection;541;1;542;0
WireConnection;536;0;541;0
WireConnection;535;0;541;0
WireConnection;277;0;541;0
WireConnection;539;0;277;0
WireConnection;346;1;55;0
WireConnection;538;0;277;0
WireConnection;537;0;535;0
WireConnection;537;1;536;0
WireConnection;57;0;55;0
WireConnection;58;0;57;0
WireConnection;540;0;538;0
WireConnection;540;1;539;0
WireConnection;320;0;346;0
WireConnection;258;0;537;0
WireConnection;62;0;58;0
WireConnection;321;0;320;0
WireConnection;264;0;537;0
WireConnection;264;1;510;0
WireConnection;275;0;540;0
WireConnection;275;1;510;0
WireConnection;260;0;258;0
WireConnection;259;1;260;0
WireConnection;270;0;286;1
WireConnection;51;0;62;0
WireConnection;51;1;62;0
WireConnection;51;2;321;0
WireConnection;276;0;264;0
WireConnection;276;1;275;0
WireConnection;276;2;264;0
WireConnection;276;3;275;0
WireConnection;103;0;286;1
WireConnection;103;1;259;0
WireConnection;54;0;51;0
WireConnection;369;0;354;0
WireConnection;267;0;276;0
WireConnection;267;1;278;0
WireConnection;267;2;270;0
WireConnection;374;0;369;0
WireConnection;274;0;103;0
WireConnection;274;1;267;0
WireConnection;380;0;374;0
WireConnection;370;0;369;0
WireConnection;67;0;274;0
WireConnection;67;1;56;0
WireConnection;145;1;143;0
WireConnection;376;0;380;0
WireConnection;376;1;377;0
WireConnection;142;0;67;0
WireConnection;142;1;145;0
WireConnection;372;0;370;0
WireConnection;117;0;142;0
WireConnection;381;0;376;0
WireConnection;381;1;376;0
WireConnection;371;0;372;0
WireConnection;371;1;372;0
WireConnection;378;0;381;0
WireConnection;373;0;371;0
WireConnection;379;0;158;0
WireConnection;379;1;378;0
WireConnection;357;0;373;0
WireConnection;357;1;358;0
WireConnection;138;0;136;0
WireConnection;356;0;379;0
WireConnection;356;1;357;0
WireConnection;139;0;138;0
WireConnection;290;0;289;0
WireConnection;190;0;356;0
WireConnection;554;0;555;0
WireConnection;222;0;290;0
WireConnection;222;1;190;3
WireConnection;147;0;139;0
WireConnection;248;0;247;0
WireConnection;248;1;554;0
WireConnection;220;0;290;0
WireConnection;220;1;190;2
WireConnection;193;0;290;0
WireConnection;193;1;190;1
WireConnection;306;0;136;0
WireConnection;156;0;290;0
WireConnection;156;1;190;0
WireConnection;271;0;53;0
WireConnection;271;1;272;0
WireConnection;271;2;273;0
WireConnection;242;0;156;0
WireConnection;242;1;248;0
WireConnection;245;0;222;0
WireConnection;245;1;248;0
WireConnection;243;0;193;0
WireConnection;243;1;248;0
WireConnection;324;0;271;0
WireConnection;244;0;220;0
WireConnection;244;1;248;0
WireConnection;307;0;306;0
WireConnection;146;0;147;0
WireConnection;146;1;147;0
WireConnection;519;0;508;0
WireConnection;519;1;243;0
WireConnection;518;0;508;0
WireConnection;518;1;242;0
WireConnection;521;0;508;0
WireConnection;521;1;245;0
WireConnection;148;0;146;0
WireConnection;174;0;324;0
WireConnection;308;0;307;0
WireConnection;520;0;508;0
WireConnection;520;1;244;0
WireConnection;465;0;459;0
WireConnection;141;0;122;0
WireConnection;141;2;148;0
WireConnection;522;0;518;1
WireConnection;522;1;519;1
WireConnection;522;2;520;1
WireConnection;522;3;521;1
WireConnection;309;0;308;0
WireConnection;304;0;141;0
WireConnection;304;1;309;0
WireConnection;184;0;117;0
WireConnection;129;0;522;0
WireConnection;129;1;523;0
WireConnection;129;2;524;0
WireConnection;352;0;325;0
WireConnection;352;4;353;0
WireConnection;466;0;465;3
WireConnection;466;1;465;0
WireConnection;466;2;465;1
WireConnection;466;3;465;2
WireConnection;209;0;117;0
WireConnection;185;0;184;0
WireConnection;130;0;129;0
WireConnection;347;0;352;0
WireConnection;347;1;339;0
WireConnection;210;0;209;3
WireConnection;210;1;209;0
WireConnection;210;2;209;1
WireConnection;210;3;209;2
WireConnection;350;0;304;0
WireConnection;348;0;175;0
WireConnection;470;0;466;0
WireConnection;470;1;471;0
WireConnection;470;2;472;0
WireConnection;313;0;228;0
WireConnection;313;1;315;0
WireConnection;338;0;122;0
WireConnection;338;1;352;0
WireConnection;338;2;347;0
WireConnection;314;0;185;0
WireConnection;314;1;228;0
WireConnection;314;2;313;0
WireConnection;473;0;470;0
WireConnection;478;0;210;0
WireConnection;478;1;490;0
WireConnection;525;0;312;0
WireConnection;131;0;130;0
WireConnection;491;0;466;0
WireConnection;491;1;492;0
WireConnection;349;0;350;0
WireConnection;349;1;348;0
WireConnection;456;0;478;0
WireConnection;493;0;491;0
WireConnection;311;0;131;0
WireConnection;311;1;525;0
WireConnection;342;0;338;0
WireConnection;351;0;349;0
WireConnection;476;0;473;0
WireConnection;231;0;213;0
WireConnection;149;0;314;0
WireConnection;211;0;212;0
WireConnection;211;1;210;0
WireConnection;133;0;141;0
WireConnection;234;0;231;0
WireConnection;382;0;342;0
WireConnection;382;1;131;0
WireConnection;382;2;311;0
WireConnection;496;0;456;0
WireConnection;496;1;497;0
WireConnection;310;0;351;0
WireConnection;310;1;131;0
WireConnection;310;2;311;0
WireConnection;474;0;476;0
WireConnection;474;1;475;0
WireConnection;474;2;493;0
WireConnection;495;0;474;0
WireConnection;495;1;456;0
WireConnection;495;2;496;0
WireConnection;233;0;211;0
WireConnection;233;1;234;0
WireConnection;153;0;133;0
WireConnection;153;1;152;0
WireConnection;153;2;310;0
WireConnection;153;3;382;0
WireConnection;203;0;233;0
WireConnection;431;0;495;0
WireConnection;399;0;117;0
WireConnection;399;1;404;0
WireConnection;399;2;403;0
WireConnection;120;0;153;0
WireConnection;206;0;203;0
WireConnection;400;0;399;0
WireConnection;426;0;428;0
WireConnection;383;0;382;0
WireConnection;420;0;426;1
WireConnection;420;2;419;0
WireConnection;483;0;482;0
WireConnection;477;0;121;0
WireConnection;477;1;438;0
WireConnection;401;0;385;0
WireConnection;401;1;390;0
WireConnection;176;0;477;0
WireConnection;176;1;208;0
WireConnection;422;0;420;0
WireConnection;485;0;483;2
WireConnection;485;1;483;3
WireConnection;484;0;483;0
WireConnection;484;1;483;1
WireConnection;236;0;176;0
WireConnection;486;0;484;0
WireConnection;486;1;485;0
WireConnection;423;0;408;0
WireConnection;423;1;424;0
WireConnection;423;2;422;0
WireConnection;480;0;406;0
WireConnection;386;0;401;0
WireConnection;388;0;386;0
WireConnection;388;1;386;1
WireConnection;237;0;236;0
WireConnection;237;1;236;1
WireConnection;488;0;423;0
WireConnection;488;1;487;0
WireConnection;488;2;486;0
WireConnection;387;0;386;2
WireConnection;387;1;386;3
WireConnection;238;0;236;2
WireConnection;238;1;236;3
WireConnection;389;0;388;0
WireConnection;389;1;387;0
WireConnection;532;0;481;0
WireConnection;418;0;488;0
WireConnection;239;0;237;0
WireConnection;239;1;238;0
WireConnection;409;0;389;0
WireConnection;534;0;532;0
WireConnection;415;0;239;0
WireConnection;410;0;415;0
WireConnection;407;0;534;0
WireConnection;407;1;417;0
WireConnection;407;2;409;0
WireConnection;405;0;407;0
WireConnection;405;1;481;0
WireConnection;405;2;410;0
WireConnection;500;0;405;0
WireConnection;504;0;502;2
WireConnection;506;0;281;1
WireConnection;506;1;505;0
WireConnection;224;0;244;0
WireConnection;123;0;242;0
WireConnection;192;0;123;0
WireConnection;192;1;195;0
WireConnection;192;2;224;0
WireConnection;192;3;225;0
WireConnection;195;0;243;0
WireConnection;225;0;245;0
WireConnection;505;0;504;0
WireConnection;0;0;405;0
WireConnection;0;9;500;0
ASEEND*/
//CHKSM=1F88B1B5E67C3EBABD091FB5600E544A90657059