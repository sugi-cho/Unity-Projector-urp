using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer)), RequireComponent(typeof(MeshFilter))]
public class ProjectorUrp : MonoBehaviour
{
    new Renderer renderer;
    Matrix4x4 projectionMatrix;
    Matrix4x4 inverseProjection;
    MaterialPropertyBlock mpb;

    int PropTexture = Shader.PropertyToID("_Texture");
    int PropProjection = Shader.PropertyToID("_Projection");
    int PropInverse = Shader.PropertyToID("_Inverse");
    int PropNear = Shader.PropertyToID("_Near");
    int PropFar = Shader.PropertyToID("_Far");

    public Texture SourceTexture => SourceTexture;
    [SerializeField] Texture sourceTexture;
    [SerializeField] float fieldOfView = 60f;
    [SerializeField] float aspectRatio = 1.6f / 0.9f;
    [SerializeField] float nearClip = 0.1f;
    [SerializeField] float farClip = 100f;

    private void OnValidate()
    {
        Setup();
    }

    private void Start()
    {
        Setup();
    }

    [ContextMenu("setup")]
    void Setup()
    {
        renderer = GetComponent<Renderer>();
        if (mpb == null)
            mpb = new MaterialPropertyBlock();

        projectionMatrix = Matrix4x4.Perspective(fieldOfView, aspectRatio, nearClip, farClip);
        inverseProjection = Matrix4x4.Inverse(projectionMatrix);

        renderer.GetPropertyBlock(mpb);
        mpb.SetTexture(PropTexture, sourceTexture);
        mpb.SetMatrix(PropProjection, projectionMatrix);
        mpb.SetMatrix(PropInverse, inverseProjection);
        mpb.SetFloat(PropNear, nearClip);
        mpb.SetFloat(PropFar, farClip);
        renderer.SetPropertyBlock(mpb);
    }
}
