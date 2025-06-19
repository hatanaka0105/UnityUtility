#if UNITY_EDITOR
using UnityEngine;

[ExecuteInEditMode]
public class MeshVisualizer : MonoBehaviour
{
    [SerializeField] private Mesh mesh;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Material material;
    [SerializeField] private bool showMesh = true;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private Vector3 scale = Vector3.one;

    public bool ShowMesh => showMesh;
    public Material Material => material;

    private void OnRenderObject()
    {
        if (Application.isPlaying)
        {
            return;
        }
        if (showMesh)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(rotation), scale);

            if (material != null)
            {
                if (mesh != null)
                {
                    Graphics.DrawMesh(mesh, matrix, material, 0);
                }
                else if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null)
                {
                    Graphics.DrawMesh(skinnedMeshRenderer.sharedMesh, matrix, material, 0);
                }
            }
            else
            {
                Gizmos.color = Color.white;
                if (mesh != null)
                {
                    Gizmos.matrix = matrix;
                    Gizmos.DrawMesh(mesh);
                }
                else if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null)
                {
                    Gizmos.matrix = matrix;
                    Gizmos.DrawMesh(skinnedMeshRenderer.sharedMesh);
                }
            }
        }
    }
}
#endif