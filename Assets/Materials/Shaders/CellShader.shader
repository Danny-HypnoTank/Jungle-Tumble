// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'


Shader "Cell Shader" 
{
	Properties
	{
		// Default colour filter ( Red, Green, Blue, Alpha )
		_Color( "Diffuse Material Color", Color ) = ( 1, 1, 1, 1 )

		// Default for the unlit areas of the shader
		_UnlitColor( "Unlit Color", Color ) = ( 0.5, 0.5, 0.5, 1 )

		// The variations between the two, giving a 'two toned' shading effect
		_DiffuseThreshold( "Lighting Threshold", Range ( -1.1, 1 ) ) = 0.1

		// The colour of the glare / shininess
		_SpecColor( "Specular Material Color", Color ) = ( 1, 1, 1, 1 )

		// Glare / shininess level
		_Shininess ( "Shininess", Range ( 0.5, 1 ) ) = 1

		// How thick the lines are
		_OutlineThickness( "Outline Thickness", Range (0, 1 ) ) = 0.1

		// Texture in question
		_MainTex ( "Albedo (RGB)", 2D ) = "white" {}

	}

// ______________________________________________________

		SubShader
	{

		Pass

	{
		// Light sits forward
		Tags { "LightMode" = "ForwardBase" }


		// Pass for ambient light and first light source

		CGPROGRAM

		// Tells the cg to use a vertex shader called vert and a fragment shader called frag
		#pragma vertex vert 		
		#pragma fragment frag
		

	// _____________

	// Custom uniforms for the shader to use
	uniform float4 _Color;
	uniform float4 _UnlitColor;
	uniform float _DiffuseThreshold;
	uniform float4 _SpecColor;
	uniform float _Shininess;
	uniform float _OutlineThickness;


	// Uniforms from unity
	uniform float4 _LightColor0;
	uniform sampler2D _MainTex;
	uniform float4 _MainTex_ST;


	struct vertexInput 
	{
		
		// Shading variables
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 texcoord : TEXCOORD0;

	};

	struct vertexOutput 
	{

		float4 pos : SV_POSITION;
		float3 normalDir : TEXCOORD1;
		float4 lightDir : TEXCOORD2;
		float3 viewDir : TEXCOORD3;
		float2 uv : TEXCOORD0;
	};

	vertexOutput vert ( vertexInput input )
	{
		vertexOutput output;

		//The normal direction
		output.normalDir = normalize ( mul ( float4 ( input.normal, 0.0 ) , unity_WorldToObject ).xyz );

		//The world position
		float4 posWorld = mul(unity_ObjectToWorld, input.vertex);

		//The view direction - vector from object to camera
		output.viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz); 

		// The light direction																	 
		float3 fragmentToLightSource = ( _WorldSpaceCameraPos.xyz - posWorld.xyz );
		output.lightDir = float4 ( normalize( lerp ( _WorldSpaceLightPos0.xyz , fragmentToLightSource, _WorldSpaceLightPos0.w ) ) ,lerp(1.0 , 1.0 / length( fragmentToLightSource ) , _WorldSpaceLightPos0.w ) );

		// Fragment input output
		output.pos = UnityObjectToClipPos ( input.vertex );

		// UV map
		output.uv = input.texcoord;

		return output;

	}

	float4 frag ( vertexOutput input ) : COLOR
	{

		float nDotL = saturate ( dot ( input.normalDir, input.lightDir.xyz ) );
	 
	// Diffuse threshold 
	float diffuseCutoff = saturate ( ( max ( _DiffuseThreshold, nDotL ) - _DiffuseThreshold ) * 1000);

	// Specular threshold 
	float specularCutoff = saturate ( max ( _Shininess, dot ( reflect ( -input.lightDir.xyz, input.normalDir ) , input.viewDir ) ) - _Shininess ) * 1000;

	// Calculate Outlines
	float outlineStrength = saturate ( ( dot ( input.normalDir, input.viewDir ) - _OutlineThickness ) * 1000 );

	// General ambient light
	float3 ambientLight = ( 1 - diffuseCutoff ) * _UnlitColor.xyz;
	float3 diffuseReflection = (1 - specularCutoff) * _Color.xyz * diffuseCutoff;
	float3 specularReflection = _SpecColor.xyz * specularCutoff;

	// Completed shader
	float3 combinedLight = (ambientLight + diffuseReflection) * outlineStrength + specularReflection;

	// _____________________________________________________


	// 2 optional outputs below, one with texture and one without

	return float4(combinedLight, 1.0);

	//return float4(combinedLight, 1.0) + tex2D(MainTex, input.uv;)
	
	}

		ENDCG

	}


	}
		Fallback "Diffuse"
}