using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGraph : MonoBehaviour
{

    [SerializeField] ComputeShader computeShader = default;

    [SerializeField] Mesh sourceMesh = default;

    [SerializeField] Material material = default;

    [SerializeField] Mesh pointsMesh = default;

    [SerializeField, Range(0.001f, 0.2f)] float step = 0.02f;

    [SerializeField] Color pointsColor = Color.white;

    [SerializeField, Range(0.0f, 1.0f)] float colorIntensity = 0.5f;
    [SerializeField, Range(0.0f, 1.0f)] float colorFromTextureLerp = 0;
    [SerializeField] Texture2D sourceMeshTexture = default;
    [SerializeField] private bool useUvs = false;

    ComputeBuffer positionsBuffer;
    ComputeBuffer uvsBuffer;


    Bounds bounds;
    Vector3[] vertices;
    Vector2[] uvs;


    private void Awake()
    {
        SetPositionBufferToComputeShader();

        SetStaticMaterialData();
        DispachComputeShader();

        bounds = new Bounds(Vector3.zero, Vector3.one * 200);
    }

    private void SetPositionBufferToComputeShader()
    {
        vertices = sourceMesh.vertices;
        Debug.Log("vertis Count :" + vertices.Length);
        positionsBuffer = new ComputeBuffer(vertices.Length, 3 * 4);
        positionsBuffer.SetData(vertices);
    }

    private void SetStaticMaterialData()
    {
        material.SetBuffer("_Positions", positionsBuffer);
        colorFromTextureLerp = 0;
        if (useUvs)
        {
            uvs = sourceMesh.uv;
            uvsBuffer = new ComputeBuffer(uvs.Length, 2 * 4);
            uvsBuffer.SetData(uvs);
            material.SetBuffer("_uvs", uvsBuffer);
            material.SetTexture("_MainTex", sourceMeshTexture);
            colorFromTextureLerp = 1;
        }
    }

    private void DispachComputeShader()
    {
        int groups = Mathf.CeilToInt(vertices.Length / 64f);
        computeShader.Dispatch(0, groups, 1, 1);
    }


    private void Update()
    {
        SetMaterialDynamicData();
        DrawInsyanceMeshes();
    }

    private void DrawInsyanceMeshes()
    {
        Graphics.DrawMeshInstancedProcedural(pointsMesh, 0, material, bounds, positionsBuffer.count);
    }

   
    private void SetMaterialDynamicData()
    {
        material.SetFloat("_Step", step);
        material.SetFloat("_scale", this.transform.localScale.x);
        material.SetFloat("_intensity", colorIntensity);
        material.SetVector("_worldPos", this.transform.position);
        material.SetVector("_color", new Vector4(pointsColor.r, pointsColor.g, pointsColor.b, 1));
        material.SetFloat("_ColorFromTextureLerp", colorFromTextureLerp);
    }

    void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
        if (useUvs)
        {
            uvsBuffer.Release();
            uvsBuffer = null;
        }

    }


}
