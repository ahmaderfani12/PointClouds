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
float _YRotation;

void ConfigureProcedural()
{ //run per vertex
#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
	
		float4x4 rotate = { cos(_YRotation),  0.0,  sin(_YRotation), 0.0f,
						    0.0,   1.0,  0.0f, 0.0f,
					        -sin(_YRotation),      0.0f,  cos(_YRotation), 0.0f,
					        0.0f,      0.0f,  0.0f, 1.0f
                   };
	
		float4x4 scale = {_Step,0.0,0.0,0.0,
							0.0,_Step,0.0,0.0,
							0.0,0.0,_Step,0.0,
							0.0,0.0,0.0,1.0 };
	
		float3 position = _Positions[unity_InstanceID];
	
		float4x4 pos = {1.0,0.0,0.0,(position.x * _scale)+_worldPos.x,
							0.0,1.0,0.0,(position.y * _scale)+_worldPos.y,
							0.0,0.0,1.0,(position.z * _scale)+ _worldPos.z,
							0.0,0.0,0.0,1.0 };
	
		float4x4 localPos = {1.0,0.0,0.0,position.x * _scale,
						0.0,1.0,0.0,position.y * _scale,
						0.0,0.0,1.0, position.z * _scale,
						0.0,0.0,0.0,1.0 };
	
		float4x4 localPosNegative = {1.0,0.0,0.0,- position.x * _scale,
							0.0,1.0,0.0,- position.y * _scale,
							0.0,0.0,1.0,- position.z * _scale,
							0.0,0.0,0.0,1.0 };
	
	    _uvPosition = _uvs[unity_InstanceID];
		_normalDirection = _Normals[unity_InstanceID];


	unity_ObjectToWorld = pos;
	unity_ObjectToWorld =mul(mul(mul(mul(unity_ObjectToWorld,localPosNegative),rotate),localPos),scale);

	

#endif
}

void ShaderGraphFunction_float(float3 In, out float3 Out, out float4 Color, out float1 Intensity, out float3 UVPosition, out float3 NormalDirection)
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