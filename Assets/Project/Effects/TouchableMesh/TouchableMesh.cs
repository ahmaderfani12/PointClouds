using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableMesh : MeshToPointCloud
{
    [SerializeField] private HandSphere handSphere;
    [SerializeField , Range(0,1)] private float drag = 0.5f;

    private ComputeBuffer velocitesBuffer;

    private Vector3 handVelocity;
    private Vector3 handPosition;
    private float handRadius;

    private void Awake()
    {
        SetVelocityBuffer();
        InitializeFromMeshData();
        base.SetBound();
    }
 
    private new void Update()
    {
        base.Update();
        DispachComputeShader();
        GetHandData();
        SetHandDataToComputeShader(); 
    }

    private void SetVelocityBuffer()
    {
        Vector3[] velocites = new Vector3[sourceMesh.vertexCount];
        velocitesBuffer = new ComputeBuffer(sourceMesh.vertexCount, 3 * 4);
        velocitesBuffer.SetData(velocites);
        computeShader.SetBuffer(0,"_velocity", velocitesBuffer);
    }
    private void GetHandData()
    {
        handPosition = handSphere.transform.position;
        handVelocity = handSphere.Velocity;
        handRadius = handSphere.Radious;
    }
    private void SetHandDataToComputeShader()
    {
        computeShader.SetVector("_handVelocity", handVelocity);
        computeShader.SetVector("_handPosition", handPosition);
        computeShader.SetFloat("_drag", drag);
        computeShader.SetFloat("_handRadius", handRadius);
    }
}
