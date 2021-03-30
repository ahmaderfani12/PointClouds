using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGraph : MonoBehaviour
{


    [SerializeField] ComputeShader computeShader = default;

    [SerializeField] Mesh sourceMesh = default;

    [SerializeField] Material material = default;

    [SerializeField] Mesh mesh = default;

    [SerializeField, Range(0.001f, 0.2f)] float step = 0.02f;

    ComputeBuffer positionsBuffer;

    int stepId;
    int positionsId;
    Bounds bounds;
    Vector3[] vertices;
    private void Awake()
    {
         positionsId = Shader.PropertyToID("_Positions");
         stepId = Shader.PropertyToID("_Step");

        vertices = sourceMesh.vertices;
        Debug.Log("vertis Count :" + vertices.Length);

        positionsBuffer = new ComputeBuffer(vertices.Length, 3 * 4);
        positionsBuffer.SetData(vertices);

        computeShader.SetBuffer(0, "_Positions", positionsBuffer);
        computeShader.SetFloat(stepId, step);

        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);

       int groups = Mathf.CeilToInt(vertices.Length / 64f);
      

        computeShader.Dispatch(0, groups, 1, 1);

        bounds = new Bounds(Vector3.zero, Vector3.one * 200);
    }

    private void Update()
    {
        

        computeShader.SetFloat(stepId, step);
        material.SetFloat(stepId, step);

        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count);

    }


    void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }

}
