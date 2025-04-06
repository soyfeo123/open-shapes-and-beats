// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AdultLink/RadialProgressBar/RadialProgressBar"
{
	Properties
	{
		_Radius("Radius", Range( 0 , 1)) = 0.3
		_Arcrange("Arc range", Range( 0 , 360)) = 360
		_Fillpercentage("Fill percentage", Range( 0 , 1)) = 0.25
		_Globalopacity("Global opacity", Range( 0 , 1)) = 1
		[HDR]_Barmincolor("Bar min color", Color) = (1,0,0,1)
		[HDR]_Barmaxcolor("Bar max color", Color) = (0,1,0.08965516,1)
		[HDR]_Barsecondarymincolor("Bar secondary min color", Color) = (1,0,0,1)
		[HDR]_Barsecondarymaxcolor("Bar secondary max color", Color) = (0,1,0.08965516,1)
		[HDR]_Bordermincolor("Border min color", Color) = (1,0,0,1)
		[HDR]_Bordermaxcolor("Border max color", Color) = (0,1,0.08965516,1)
		_Mainborderradialwidth("Main border radial width", Range( 0 , 0.2)) = 0
		_Mainbordertangentwidth("Main border tangent width", Range( 0 , 1)) = 0
		_Backgroundbordertangentwidth("Background border tangent width", Range( 0 , 1)) = 0
		_Backgroundborderradialwidth("Background border radial width", Range( 0 , 0.2)) = 0
		[HDR]_Backgroundbordercolor("Background border color", Color) = (0.2132353,0.6418865,1,1)
		[HDR]_Backgroundfillcolor("Background fill color", Color) = (0.2132353,0.6418865,1,1)
		[NoScaleOffset]_Maintex("Main tex", 2D) = "white" {}
		_Maintexscrollspeed("Main tex scroll speed", Vector) = (0,0,0,0)
		_Maintextiling("Main tex tiling", Vector) = (0,0,0,0)
		_Maintexoffset("Main tex offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Secondarytex("Secondary tex", 2D) = "black" {}
		_Secondarytexscrollspeed("Secondary tex scroll speed", Vector) = (0,0,0,0)
		_Secondarytextiling("Secondary tex tiling", Vector) = (0,0,0,0)
		_Secondarytexoffset("Secondary tex offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Noisetex("Noise tex", 2D) = "white" {}
		[Toggle]_Invertnoisetex("Invertnoisetex", Float) = 0
		_Noisetexspeed("Noise tex speed", Vector) = (0,0,0,0)
		_Noisetextiling("Noise tex tiling", Vector) = (0,0,0,0)
		_Noisetexoffset("Noise tex offset", Vector) = (0,0,0,0)
		_Noiseintensity("Noise intensity", Float) = 5
		_Maintexrotationspeed("Main tex rotation speed", Float) = 1
		_Secondarytexrotationspeed("Secondary tex rotation speed", Float) = 1
		[KeywordEnum(Scroll,Rotate,None)] _Mainscrollrotate("Main scroll rotate", Float) = 0
		[KeywordEnum(Scroll,Rotate,None)] _Secondaryscrollrotate("Secondary scroll rotate", Float) = 0
		_Backgroundborderopacity("Background border opacity", Range( 0 , 1)) = 1
		_Backgroundopacity("Background opacity", Range( 0 , 1)) = 1
		_Secondarytexopacity("Secondary tex opacity", Range( 0 , 1)) = 1
		_Maintexopacity("Main tex opacity", Range( 0 , 1)) = 1
		_Mainbarborderopacity("Main bar border opacity", Range( 0 , 1)) = 1
		_Rotation("Rotation", Range( 0 , 360)) = 0
		[Toggle]_Invertsecondarytex("Invert secondary tex", Float) = 0
		[Toggle]_Invertmaintex("Invert main tex", Float) = 0
		_Noisetexcontrast("Noise tex contrast", Float) = 1
		_Secondarytexcontrast("Secondary tex contrast", Float) = 1
		_Maintexcontrast("Main tex contrast", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _SECONDARYSCROLLROTATE_SCROLL _SECONDARYSCROLLROTATE_ROTATE _SECONDARYSCROLLROTATE_NONE
		#pragma shader_feature _MAINSCROLLROTATE_SCROLL _MAINSCROLLROTATE_ROTATE _MAINSCROLLROTATE_NONE
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Secondarytexcontrast;
		uniform float _Invertsecondarytex;
		uniform sampler2D _Secondarytex;
		uniform float2 _Secondarytexscrollspeed;
		uniform float2 _Secondarytextiling;
		uniform float2 _Secondarytexoffset;
		uniform float _Secondarytexrotationspeed;
		uniform float _Noisetexcontrast;
		uniform float _Invertnoisetex;
		uniform sampler2D _Noisetex;
		uniform float2 _Noisetexspeed;
		uniform float2 _Noisetextiling;
		uniform float2 _Noisetexoffset;
		uniform float _Noiseintensity;
		uniform float _Mainborderradialwidth;
		uniform float _Backgroundborderradialwidth;
		uniform float _Rotation;
		uniform float _Mainbordertangentwidth;
		uniform float _Fillpercentage;
		uniform float _Arcrange;
		uniform float _Backgroundbordertangentwidth;
		uniform float _Radius;
		uniform float4 _Barsecondarymincolor;
		uniform float4 _Barsecondarymaxcolor;
		uniform float _Secondarytexopacity;
		uniform float4 _Bordermincolor;
		uniform float4 _Bordermaxcolor;
		uniform float _Mainbarborderopacity;
		uniform float _Maintexcontrast;
		uniform float _Invertmaintex;
		uniform sampler2D _Maintex;
		uniform float2 _Maintexscrollspeed;
		uniform float2 _Maintextiling;
		uniform float2 _Maintexoffset;
		uniform float _Maintexrotationspeed;
		uniform float4 _Barmincolor;
		uniform float4 _Barmaxcolor;
		uniform float _Maintexopacity;
		uniform float4 _Backgroundbordercolor;
		uniform float _Backgroundborderopacity;
		uniform float4 _Backgroundfillcolor;
		uniform float _Backgroundopacity;
		uniform float _Globalopacity;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_output_332_0 = (float2( 0,0 ) + (_Secondarytexoffset - float2( 0,0 )) * (float2( 1,1 ) - float2( 0,0 )) / (float2( 360,360 ) - float2( 0,0 )));
			float2 uv_TexCoord341 = i.uv_texcoord * _Secondarytextiling + temp_output_332_0;
			float2 panner343 = ( _Time.y * (float2( 0,0 ) + (_Secondarytexscrollspeed - float2( 0,0 )) * (float2( 0.01,0.01 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( 0,0 ))) + uv_TexCoord341);
			float2 uv_TexCoord339 = i.uv_texcoord * _Secondarytextiling + temp_output_332_0;
			float mulTime340 = _Time.y * _Secondarytexrotationspeed;
			float cos344 = cos( mulTime340 );
			float sin344 = sin( mulTime340 );
			float2 rotator344 = mul( uv_TexCoord339 - ( _Secondarytextiling * float2( 0.5,0.5 ) ) , float2x2( cos344 , -sin344 , sin344 , cos344 )) + ( _Secondarytextiling * float2( 0.5,0.5 ) );
			#if defined(_SECONDARYSCROLLROTATE_SCROLL)
				float2 staticSwitch345 = panner343;
			#elif defined(_SECONDARYSCROLLROTATE_ROTATE)
				float2 staticSwitch345 = rotator344;
			#elif defined(_SECONDARYSCROLLROTATE_NONE)
				float2 staticSwitch345 = uv_TexCoord341;
			#else
				float2 staticSwitch345 = panner343;
			#endif
			float4 tex2DNode89 = tex2D( _Secondarytex, staticSwitch345 );
			float4 appendResult399 = (float4(( 1.0 - tex2DNode89.r ) , ( 1.0 - tex2DNode89.g ) , ( 1.0 - tex2DNode89.b ) , tex2DNode89.a));
			float4 Secondarytexture126 = CalculateContrast(_Secondarytexcontrast,lerp(tex2DNode89,appendResult399,_Invertsecondarytex));
			float2 uv_TexCoord94 = i.uv_texcoord * _Noisetextiling + _Noisetexoffset;
			float2 panner96 = ( _Time.y * (float2( 0,0 ) + (_Noisetexspeed - float2( 0,0 )) * (float2( 0.01,0.01 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( 0,0 ))) + uv_TexCoord94);
			float Noiseintensity167 = _Noiseintensity;
			float4 temp_output_98_0 = ( tex2D( _Noisetex, panner96 ) * Noiseintensity167 );
			float4 Noisetexture127 = CalculateContrast(_Noisetexcontrast,lerp(temp_output_98_0,( 1.0 - temp_output_98_0 ),_Invertnoisetex));
			float MainbarBorderwidth138 = _Mainborderradialwidth;
			float BackgroundBorderWidth183 = _Backgroundborderradialwidth;
			float2 temp_output_74_0 = ( float2( 1,1 ) + MainbarBorderwidth138 + BackgroundBorderWidth183 );
			float2 temp_output_11_0 = (-temp_output_74_0 + (i.uv_texcoord - float2( 0,0 )) * (temp_output_74_0 - -temp_output_74_0) / (float2( 1,1 ) - float2( 0,0 )));
			float cos383 = cos( radians( _Rotation ) );
			float sin383 = sin( radians( _Rotation ) );
			float2 rotator383 = mul( temp_output_11_0 - float2( 0,0 ) , float2x2( cos383 , -sin383 , sin383 , cos383 )) + float2( 0,0 );
			float2 break28 = (rotator383).xy;
			float temp_output_29_0 = (0.0 + (atan2( break28.y , break28.x ) - -UNITY_PI) * (1.0 - 0.0) / (UNITY_PI - -UNITY_PI));
			float Borderwidth2157 = (0.0 + (_Mainbordertangentwidth - 0.0) * (0.05 - 0.0) / (1.0 - 0.0));
			float Fillpercentage140 = _Fillpercentage;
			float ArcRange205 = (0.0 + (_Arcrange - 0.0) * (1.0 - 0.0) / (360.0 - 0.0));
			float BorderWidth3252 = (0.0 + (_Backgroundbordertangentwidth - 0.0) * (0.05 - 0.0) / (1.0 - 0.0));
			float MainbarFillPercentage145 = ceil( ( temp_output_29_0 - ( Borderwidth2157 + (1.0 + (Fillpercentage140 - 0.0) * ((1.0 + (ArcRange205 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) - 1.0) / (1.0 - 0.0)) + BorderWidth3252 ) ) );
			float BarRadius133 = _Radius;
			float Length135 = length( temp_output_11_0 );
			float MainbarFill150 = ( MainbarFillPercentage145 * floor( ( BarRadius133 + Length135 ) ) * ( 1.0 - floor( Length135 ) ) );
			float4 lerpResult236 = lerp( _Barsecondarymincolor , _Barsecondarymaxcolor , Fillpercentage140);
			float4 BarSecondaryColor237 = lerpResult236;
			float4 break363 = ( Secondarytexture126 * Noisetexture127 * MainbarFill150 * BarSecondaryColor237 );
			float4 appendResult364 = (float4(break363.r , break363.g , break363.b , ( break363.a * _Secondarytexopacity )));
			float4 lerpResult73 = lerp( _Bordermincolor , _Bordermaxcolor , Fillpercentage140);
			float4 Bordercolor121 = lerpResult73;
			float MainbarBorderPercentage146 = ceil( ( temp_output_29_0 - (1.0 + (( ( Fillpercentage140 * ArcRange205 ) - BorderWidth3252 ) - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) ) );
			float temp_output_47_0 = floor( ( BarRadius133 + MainbarBorderwidth138 + Length135 ) );
			float temp_output_50_0 = ( 1.0 - floor( ( Length135 + -MainbarBorderwidth138 ) ) );
			float Biggercircle151 = ( MainbarBorderPercentage146 * temp_output_47_0 * temp_output_50_0 );
			float MainbarBorder161 = ( Biggercircle151 - MainbarFill150 );
			float4 break367 = ( Bordercolor121 * MainbarBorder161 );
			float4 appendResult368 = (float4(break367.r , break367.g , break367.b , ( break367.a * _Mainbarborderopacity )));
			float2 temp_output_357_0 = (float2( 0,0 ) + (_Maintexoffset - float2( 0,0 )) * (float2( 1,1 ) - float2( 0,0 )) / (float2( 360,360 ) - float2( 0,0 )));
			float2 uv_TexCoord349 = i.uv_texcoord * _Maintextiling + temp_output_357_0;
			float2 panner352 = ( _Time.y * (float2( 0,0 ) + (_Maintexscrollspeed - float2( 0,0 )) * (float2( 0.01,0.01 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( 0,0 ))) + uv_TexCoord349);
			float2 uv_TexCoord348 = i.uv_texcoord * _Maintextiling + temp_output_357_0;
			float mulTime347 = _Time.y * _Maintexrotationspeed;
			float cos351 = cos( mulTime347 );
			float sin351 = sin( mulTime347 );
			float2 rotator351 = mul( uv_TexCoord348 - ( _Maintextiling * float2( 0.5,0.5 ) ) , float2x2( cos351 , -sin351 , sin351 , cos351 )) + ( _Maintextiling * float2( 0.5,0.5 ) );
			#if defined(_MAINSCROLLROTATE_SCROLL)
				float2 staticSwitch353 = panner352;
			#elif defined(_MAINSCROLLROTATE_ROTATE)
				float2 staticSwitch353 = rotator351;
			#elif defined(_MAINSCROLLROTATE_NONE)
				float2 staticSwitch353 = uv_TexCoord349;
			#else
				float2 staticSwitch353 = panner352;
			#endif
			float4 tex2DNode38 = tex2D( _Maintex, staticSwitch353 );
			float4 appendResult390 = (float4(( 1.0 - tex2DNode38.r ) , ( 1.0 - tex2DNode38.g ) , ( 1.0 - tex2DNode38.b ) , tex2DNode38.a));
			float4 Maintexture131 = CalculateContrast(_Maintexcontrast,lerp(tex2DNode38,appendResult390,_Invertmaintex));
			float4 lerpResult6 = lerp( _Barmincolor , _Barmaxcolor , Fillpercentage140);
			float4 BarColor123 = lerpResult6;
			float4 break371 = ( MainbarFill150 * Maintexture131 * BarColor123 );
			float4 appendResult372 = (float4(break371.r , break371.g , break371.b , ( break371.a * _Maintexopacity )));
			float temp_output_258_0 = (1.0 + (ArcRange205 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0));
			float BackgroundBorderPercentage257 = ceil( ( temp_output_29_0 - temp_output_258_0 ) );
			float temp_output_186_0 = ( MainbarBorderwidth138 + BackgroundBorderWidth183 );
			float Background181 = ( BackgroundBorderPercentage257 * floor( ( BarRadius133 + temp_output_186_0 + Length135 ) ) * ( 1.0 - floor( ( Length135 + -temp_output_186_0 ) ) ) );
			float BackgroundFillPercentage211 = ceil( ( temp_output_29_0 - ( BorderWidth3252 + temp_output_258_0 ) ) );
			float BackgroundFillFull262 = ( BackgroundFillPercentage211 * floor( ( BarRadius133 + MainbarBorderwidth138 + Length135 ) ) * ( 1.0 - floor( ( Length135 + -MainbarBorderwidth138 ) ) ) );
			float clampResult217 = clamp( ( Background181 - BackgroundFillFull262 ) , 0.0 , 1.0 );
			float BackgroundBorder191 = clampResult217;
			float4 BGColor194 = _Backgroundbordercolor;
			float4 break375 = ( BackgroundBorder191 * BGColor194 );
			float4 appendResult376 = (float4(break375.r , break375.g , break375.b , ( break375.a * _Backgroundborderopacity )));
			float BackgroundFillEmpty223 = ( BackgroundFillFull262 - Biggercircle151 );
			float4 BackgroundFillColor229 = _Backgroundfillcolor;
			float4 break379 = ( BackgroundFillEmpty223 * BackgroundFillColor229 );
			float4 appendResult380 = (float4(break379.r , break379.g , break379.b , ( break379.a * _Backgroundopacity )));
			float4 temp_output_59_0 = ( appendResult364 + appendResult368 + appendResult372 + appendResult376 + appendResult380 );
			o.Emission = temp_output_59_0.xyz;
			float Opacity202 = _Globalopacity;
			o.Alpha = ( temp_output_59_0.w * Opacity202 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
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
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
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
	CustomEditor "AdultLink.RadialProgressBarEditor"
}