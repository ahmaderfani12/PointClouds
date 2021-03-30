using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGPU : MonoBehaviour
{

	[SerializeField]
	ComputeShader computeShader = default;
	[SerializeField]
	Material material = default;
	[SerializeField]
	Mesh mesh = default;
	[SerializeField, Range(10, 200)]
	int resolution = 10;

	FunctionLibrary.FunctionName transitionFunction;

	ComputeBuffer positionsBuffer;

	static readonly int
		positionsId = Shader.PropertyToID("_Positions"),
		resolutionId = Shader.PropertyToID("_Resolution"),
		stepId = Shader.PropertyToID("_Step"),
		timeId = Shader.PropertyToID("_Time");

	float step;
	int groups;
	Bounds bounds;
	void OnEnable()
	{
		positionsBuffer = new ComputeBuffer(resolution * resolution, 3 * 4);
		computeShader.SetBuffer(0, positionsId, positionsBuffer);

		float step = 2f / resolution;
		computeShader.SetInt(resolutionId, resolution);
		computeShader.SetFloat(stepId, step);
		computeShader.SetFloat(timeId, Time.time);

		material.SetBuffer(positionsId, positionsBuffer);
		material.SetFloat(stepId, step);

		step = 2f / resolution;
		groups = Mathf.CeilToInt(resolution / 8f);
		bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
	}
	void OnDisable()
	{
		positionsBuffer.Release();
		positionsBuffer = null;
	}

    private void Update()
    {
		UpdateFunctionOnGPU();
	}
    void UpdateFunctionOnGPU()
	{

		

		computeShader.SetFloat(timeId, Time.time);


		
		computeShader.Dispatch(0, groups, groups, 1);

		

		
		Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count);
	}
}
