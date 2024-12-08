using ProjectHH;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TestEnemy : CharacterBase
{
    private MeshRenderer _meshRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // 颜色从红色渐变到白色
        _meshRenderer.material.color = Color.Lerp(_meshRenderer.material.color, Color.white, 0.1f);
    }
}
