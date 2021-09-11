using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshInitializer : MonoBehaviour
{
    #region PROPERTIES
    private MeshFilter meshFilter;
    private Mesh mesh;
    #endregion

    #region UNITY CALLBACKS
    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

        mesh = new Mesh();
        mesh.MarkDynamic();
        mesh.name = "Quad";
        meshFilter.mesh = mesh;
    }
    #endregion
}
 