using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistortionMesh : MonoBehaviour
{

    [SerializeField] ComputeShader computeShader = default;

    [SerializeField] Mesh sourceMesh = default;

    [SerializeField] Material material = default;

    [SerializeField] Mesh mesh = default;

    [SerializeField, Range(0.001f, 0.2f)] float step = 0.02f; // scale

    [SerializeField, Range(0f, 1000)] float chaos = 0.02f;

    [SerializeField] bool changeSizeManual = false;

    ComputeBuffer positionsBuffer;
    ComputeBuffer positionsBufferTemp;

    int stepId;
    int positionsId;
    int timeId;
    int chaosId;
    Bounds bounds;

    private void Awake()
    {
        //Get IDs
        positionsId = Shader.PropertyToID("_Positions");
        stepId = Shader.PropertyToID("_Step");
        timeId = Shader.PropertyToID("_Time");
        chaosId = Shader.PropertyToID("_Chaos");



        //Create two buffer, real vertex pos and distorted pos
        positionsBuffer = new ComputeBuffer(sourceMesh.vertexCount, 3 * 4);
        positionsBuffer.SetData(sourceMesh.vertices);

        positionsBufferTemp = new ComputeBuffer(sourceMesh.vertexCount, 3 * 4);
        positionsBufferTemp.SetData(sourceMesh.vertices);

        //Set buffers to compute shader
        computeShader.SetBuffer(0, "_Positions", positionsBuffer);
        computeShader.SetBuffer(0, "_PositionsTemp", positionsBufferTemp);

        //Set real vertex pos buffet to material
        material.SetBuffer(positionsId, positionsBuffer);

        bounds = new Bounds(Vector3.zero, Vector3.one * 200);
    }

    private void Update()
    {
        //Set dynamic variables in shaders
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(chaosId, chaos);
        material.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);

        //Run Compute Shader
        int groups = Mathf.CeilToInt(sourceMesh.vertexCount / 64f);
        computeShader.Dispatch(0, groups, 1, 1);
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count);

        if(!changeSizeManual) SetAutoStep();
    }

    void SetAutoStep()
    {
        step = Mathf.Lerp(0.01f, 0.04f, chaos / 1000);
    }
    void OnDisable()
    {
        positionsBuffer.Release();
        positionsBufferTemp.Release();
        positionsBuffer = null;
        positionsBufferTemp = null;
    }
}

