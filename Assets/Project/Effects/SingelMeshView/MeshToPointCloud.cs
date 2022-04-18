using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshToPointCloud : MonoBehaviour
{
    [Header("Static Setups")]

    [SerializeField] protected  ComputeShader computeShader = default;

    [SerializeField] protected Mesh sourceMesh = default;

    [SerializeField] Material material = default;

    [SerializeField] Mesh pointsMesh = default;

    [SerializeField] private bool setColorFromTexture = false;

    [SerializeField] private bool useNormalDirection = false;

    [SerializeField] Texture2D sourceMeshTexture = default;

    [Header("Dynamic Setups")]
    [SerializeField, Range(0.001f, 0.2f)] float step = 0.02f; //size
    [SerializeField] Color pointsColor = Color.white;
    [SerializeField, Range(0.0f, 1.0f)] float colorIntensity = 0.5f;
    [SerializeField, Range(0.0f, 1.0f)] float colorFromTextureLerp = 0;

    [SerializeField] private bool useAlpha = false;



    ComputeBuffer positionsBuffer;
    ComputeBuffer uvsBuffer;
    ComputeBuffer normalsBuffer;


    Bounds bounds;
    Vector3[] vertices;
    Vector2[] uvs;
    Vector3[] normals;

    //If we just want to show the Points whithout any caculation(VFX) on Compute shader(like this class)
    //we can ignore executing Compute shader
    private void Awake()
    {
        InitializeFromMeshData();
        SetBound();
    }

    protected void InitializeFromMeshData()
    {
        GetPositionsDataFromMesh();
        SetStaticMaterialData();    
    }

    private void GetPositionsDataFromMesh()
    {
        vertices = sourceMesh.vertices;
        Debug.Log("vertis Count :" + vertices.Length);
        positionsBuffer = new ComputeBuffer(vertices.Length, 3 * 4);
        positionsBuffer.SetData(vertices);

        computeShader.SetBuffer(0, "_Positions", positionsBuffer);
    }

    private void SetStaticMaterialData()
    {
        material.SetBuffer("_Positions", positionsBuffer);
        material.SetFloat("_UseNormals", 0);
        colorFromTextureLerp = 0;
        if (setColorFromTexture)
            SetUVAndTextureData();
        if (useNormalDirection)
            SetNormalsData();
    }

    private void SetUVAndTextureData()
    {
        uvs = sourceMesh.uv;
        uvsBuffer = new ComputeBuffer(uvs.Length, 2 * 4);
        uvsBuffer.SetData(uvs);
        material.SetBuffer("_uvs", uvsBuffer);
        material.SetTexture("_MainTex", sourceMeshTexture);
        colorFromTextureLerp = 1;
    }

    private void SetNormalsData()
    {
        normals = sourceMesh.normals;
        normalsBuffer = new ComputeBuffer(normals.Length, 3 * 4);
        normalsBuffer.SetData(normals);
        material.SetBuffer("_Normals", normalsBuffer);
        material.SetFloat("_UseNormals", 1);
    }

    protected void DispachComputeShader()
    {
        int groups = Mathf.CeilToInt(vertices.Length / 64f);
        computeShader.Dispatch(0, groups, 1, 1);
    }

    protected void Update()
    {
        SetMaterialDynamicData();
        DrawInstanceMeshes();
    }

    protected void DrawInstanceMeshes()
    {
        Graphics.DrawMeshInstancedProcedural(pointsMesh, 0, material, bounds, positionsBuffer.count);
    }

    protected virtual void SetMaterialDynamicData()
    {
        material.SetFloat("_Step", step);
        material.SetFloat("_scale", this.transform.localScale.x);
        material.SetFloat("_intensity", colorIntensity);
        material.SetVector("_worldPos", this.transform.position);
        material.SetVector("_color", new Vector4(pointsColor.r, pointsColor.g, pointsColor.b, 1));
        material.SetFloat("_ColorFromTextureLerp", colorFromTextureLerp);
        material.SetFloat("_UseAlpha", useAlpha?1:0);
        material.SetMatrix("_quaternion", Matrix4x4.TRS(new Vector3(0, 0, 0), transform.rotation, new Vector3(1, 1, 1)));
    }

    protected void SetBound()
    {
        bounds = new Bounds(Vector3.zero, Vector3.one * 200);
    }


    protected virtual void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
        if (setColorFromTexture)
        {
            uvsBuffer.Release();
            uvsBuffer = null;
        }
        if (useNormalDirection)
        {
            normalsBuffer.Release();
            normalsBuffer = null;
        }
    }
}
