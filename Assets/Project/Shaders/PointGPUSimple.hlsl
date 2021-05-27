#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
	StructuredBuffer<float3> _Positions;
#endif

float _Step;


void ConfigureProcedural () { //run per vertex
	#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
		float3 position = _Positions[unity_InstanceID];

		unity_ObjectToWorld = 0.0;
		unity_ObjectToWorld._m03_m13_m23_m33 = float4((position), 1.0);
		unity_ObjectToWorld._m00_m11_m22 = _Step ;
#endif
}

void ShaderGraphFunction_float(float3 In, out float3 Out)
{
	Out = In;
}

void ShaderGraphFunction_half(half3 In, out half3 Out)
{
	Out = In;
}