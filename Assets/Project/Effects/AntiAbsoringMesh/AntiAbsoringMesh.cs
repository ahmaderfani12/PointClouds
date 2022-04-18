using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiAbsoringMesh : MeshToPointCloud
{
    [SerializeField] private HandSphere handSphere;
    [SerializeField , Range(0,20)] private float effectRadious=1;
    [SerializeField, Range(0.01f, 20)] private float effectPower = 1;

    private Vector3 handPosition;
    private ComputeBuffer positionsBufferTemp;

    private void Awake()
    {
        InitializeFromMeshData();
        SetTempPositionBufferToComputeShader();
        base.SetBound();
    }

    private new void Update()
    {
        base.Update();
        DispachComputeShader();
        GetHandData();
        SetHandDataToComputeShader();
    }

    private void GetHandData()
    {
        handPosition = handSphere.transform.position;
    }
    private void SetHandDataToComputeShader()
    {
        computeShader.SetVector("_handPosition", handPosition);
        computeShader.SetFloat("_effectRadious", effectRadious);
        computeShader.SetFloat("_effectPower", effectPower);
    }
    private void SetTempPositionBufferToComputeShader()
    {
        positionsBufferTemp = new ComputeBuffer(sourceMesh.vertexCount, 3 * 4);
        positionsBufferTemp.SetData(sourceMesh.vertices);
        computeShader.SetBuffer(0, "_PositionsTemp", positionsBufferTemp);
    }

}
