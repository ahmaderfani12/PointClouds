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

    [SerializeField] Color pointsColor = Color.white;

    [SerializeField, Range(0.0f, 1.0f)] float colorIntensity = 0;

    ComputeBuffer positionsBuffer;

    int positionsId;
    Bounds bounds;
    Vector3[] vertices;

    private void Awake()
    {
         positionsId = Shader.PropertyToID("_Positions");

        vertices = sourceMesh.vertices;
        Debug.Log("vertis Count :" + vertices.Length);

        positionsBuffer = new ComputeBuffer(vertices.Length, 3 * 4);
        positionsBuffer.SetData(vertices);

        material.SetBuffer(positionsId, positionsBuffer);

        computeShader.SetBuffer(0, "_Positions", positionsBuffer);

       int groups = Mathf.CeilToInt(vertices.Length / 64f);
      
        computeShader.Dispatch(0, groups, 1, 1);

        bounds = new Bounds(Vector3.zero, Vector3.one * 200);
    }

    private void Update()
    {
        SetMaterialDynamicData();
        DrawInsyanceMeshes();
    }

    private void DrawInsyanceMeshes()
    {
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count);
    }

    private void SetMaterialDynamicData()
    {
        material.SetFloat("_Step", step);
        material.SetFloat("_intensity", colorIntensity);
        material.SetVector("_color", new Vector4(pointsColor.r, pointsColor.g, pointsColor.b, 1));
    }

    void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }

}
