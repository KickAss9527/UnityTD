﻿Shader "Custom/NewSurfaceShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_TransVal ("Transparency Value", Range(0,1)) = 0.5
		_Scale ("Scale", Range(0, 1)) = 0   
		_LineWidth ("LineWidth", Range(0, 1)) = 0.05
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Transparent"}
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
//		#pragma surface surf Standard fullforwardshadows
		#pragma surface surf Lambert alpha
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _Scale;
		half _LineWidth;
		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
//			if(IN.uv_MainTex.x > 0.5) c.g = c.b = 0;
			float lineWidth = _LineWidth*_Scale;
			float rIn = 1-lineWidth -0.5;
			rIn *= rIn;
			float rOut = 1-0.5;
			rOut *= rOut;

			float2 xy = IN.uv_MainTex;
			xy.x -= 0.5;
			xy.y -= 0.5;
			float sq = xy.x*xy.x + xy.y*xy.y;

			if(sq < rIn)
			{
				c.a = 0.1;
			}
			else if(sq > rOut)
			{
				c.a = 0;
			}
			else 
			{
				c.a = 1;
			}
			c.r = 0;
			c.g = 1;
			c.b = 0;
			o.Albedo = c;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
