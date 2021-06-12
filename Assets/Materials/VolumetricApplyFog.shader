Shader "Custom/VolumetricFogApplyBlurr"
{
    Properties
    {
	_MainTex("Texture", 2D) = "white" {}
	_VolFogTex("Volumetric Fog", 2D) = "white" {}
    }

	SubShader
    {
	Tags { "RenderType" = "Opaque" }
	LOD 100

	Pass
	{
	    CGPROGRAM
	    #pragma vertex vert
	    #pragma fragment frag
	    // make fog work
	    #pragma multi_compile_fog

	    #include "UnityCG.cginc"

	    struct appdata
	    {
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	    };

	    struct v2f
	    {
		float2 uv : TEXCOORD0;
		UNITY_FOG_COORDS(1)
		float4 vertex : SV_POSITION;
	    };

	    sampler2D _MainTex;
	    // sampler2D _CameraOpaqueTexture;
	    float4 _MainTex_TexelSize;
	    float4 _MainTex_ST;
	    sampler2D _VolFogTex;

	    float _offset;

	    v2f vert(appdata v)
	    {
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		return o;
	    }

	    fixed4 frag(v2f input) : SV_Target
	    {
		float2 res = _MainTex_TexelSize.xy;

		fixed4 col;
		fixed4 volFog = tex2D(_VolFogTex, input.uv);
		col.rgb = tex2D(_MainTex, input.uv).rgb*volFog.a;
		col.rgb += volFog.rgb;

		return col;
	    }
	    ENDCG
	}
    }
}
