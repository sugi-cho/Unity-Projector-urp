using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Renderer)), RequireComponent(typeof(MeshFilter))]
public class ProjectorUrp : MonoBehaviour
{
    new Renderer renderer;
    Matrix4x4 projectionMatrix;
    Matrix4x4 inverseProjection;
    MaterialPropertyBlock mpb;
    VisualEffect beamVfx;

    int PropTexture = Shader.PropertyToID("_Texture");
    int PropProjection = Shader.PropertyToID("_Projection");
    int PropInverse = Shader.PropertyToID("_Inverse");
    int PropNear = Shader.PropertyToID("_Near");
    int PropFar = Shader.PropertyToID("_Far");
    int PropFov = Shader.PropertyToID("_Fov");
    int PropAspect = Shader.PropertyToID("_Aspect");
    int propColor = Shader.PropertyToID("_Color");

    public Texture SourceTexture => SourceTexture;
    [SerializeField] Texture sourceTexture;
    [SerializeField] float fieldOfView = 60f;
    [SerializeField] float aspectRatio = 1.6f / 0.9f;
    [SerializeField] float nearClip = 0.1f;
    [SerializeField] float farClip = 100f;
    [SerializeField, ColorUsage(false, true)] Color projectionColor = Color.white;
    [SerializeField, ColorUsage(false, true)] Color beamColor = Color.white;

    private void OnValidate()
    {
        Setup();
    }

    private void Start()
    {
        Setup();
        var size = farClip - nearClip;
        GetComponent<MeshFilter>().mesh.bounds = new Bounds(Vector3.forward * size / 2f, Vector3.one * size / 2);
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
        mpb.SetColor(propColor, projectionColor);
        renderer.SetPropertyBlock(mpb);

        beamVfx = GetComponentInChildren<VisualEffect>();
        beamVfx.SetTexture(PropTexture, sourceTexture);
        beamVfx.SetFloat(PropNear, nearClip);
        beamVfx.SetFloat(PropFar, farClip);
        beamVfx.SetFloat(PropFov, fieldOfView);
        beamVfx.SetFloat(PropAspect, aspectRatio);
        beamVfx.SetVector4(propColor, beamColor);
    }
}
