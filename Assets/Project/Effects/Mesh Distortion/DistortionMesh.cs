using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistortionMesh : MeshToPointCloud
{
    ComputeBuffer positionsBufferTemp;

    [SerializeField, Range(0f, 1000)] float chaos = 0.02f;

    public float Chaos
    {
        get => chaos;
        set
        {
            if ((value > 0) && (value < 1000))
            {
                chaos = value;
            }
        }
    }

    private void Awake()
    {
        base.InitializeFromMeshData();

        SetTempPositionBufferToComputeShader();
    }

    private void SetTempPositionBufferToComputeShader()
    {
        positionsBufferTemp = new ComputeBuffer(sourceMesh.vertexCount, 3 * 4);
        positionsBufferTemp.SetData(sourceMesh.vertices);
        computeShader.SetBuffer(0, "_PositionsTemp", positionsBufferTemp);
    }

    private new void Update()
    {
        base.Update();
        DispachComputeShader();
    }

    protected override void SetMaterialDynamicData()
    {
        base.SetMaterialDynamicData();

        computeShader.SetFloat("_Time", Time.time);
        computeShader.SetFloat("_Chaos", chaos);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        positionsBufferTemp.Release();
        positionsBufferTemp = null;
    }
}
