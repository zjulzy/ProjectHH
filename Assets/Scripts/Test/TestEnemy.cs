using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TestEnemy : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // 颜色从红色渐变到白色
        _meshRenderer.material.color = Color.Lerp(_meshRenderer.material.color, Color.white, 0.1f);
    }
}