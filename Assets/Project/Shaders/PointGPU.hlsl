#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
	StructuredBuffer<float3> _Positions;
	StructuredBuffer<float3> _uvs;
	StructuredBuffer<float3> _Normals;
#endif

float _Step;
float4 _color;
float1 _intensity;
float3 _worldPos;
float1 _scale;
float3 _uvPosition;
float3 _normalDirection;

void ConfigureProcedural () { //run per vertex
	#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
		float3 position = _Positions[unity_InstanceID];
	    _uvPosition = _uvs[unity_InstanceID];
		_normalDirection = _Normals[unity_InstanceID];
		unity_ObjectToWorld = 0.0;
		unity_ObjectToWorld._m03_m13_m23_m33 = float4((position * _scale) + _worldPos, 1.0);
		unity_ObjectToWorld._m00_m11_m22 = _Step ;
#endif
}

void ShaderGraphFunction_float(float3 In, out float3 Out, out float4 Color, out float1 Intensity , out float3 UVPosition , out float3 NormalDirection)
{
	Out = In;
    Color = _color;
    Intensity = _intensity;
    UVPosition = _uvPosition;
    NormalDirection = _normalDirection;

}

void ShaderGraphFunction_half(half3 In, out half3 Out, out float4 Color, out float1 Intensity, out float3 UVPosition, out float3 NormalDirection)
{
	Out = In;
    Color = _color;
    Intensity = _intensity;
    UVPosition = _uvPosition;
    NormalDirection = _normalDirection;
}