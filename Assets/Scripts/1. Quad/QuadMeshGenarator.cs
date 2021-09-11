using System.Collections.Generic;
using UnityEngine;
 
public class QuadMeshGenarator : MonoBehaviour
{
    #region PROPERTIES
    Mesh mesh;
    #endregion

    #region UNITY CALLBACKS
    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        //mesh = GetComponent<MeshFilter>().sharedMesh;

        List<Vector3> vertsList = new List<Vector3>();//VERTICES LIST
        List<int> triList = new List<int>();//TRIANGLE LIST
        List<Vector3> normalList = new List<Vector3>(); //NORMAL LIST (WE NEED NORMALS IF NEED LIGTING TO WORK ON THE MESH)
        List<Vector2> uvList = new List<Vector2>();//MAP TEXTURES

        AddVertices(vertsList, normalList,uvList);
        AddTriangles(triList);

        //mesh.vertices = vertsList.ToArray();//SETTING PROPERTIES MAKE MORE GARBAGE
        //mesh.triangles = triList.ToArray();
        //mesh.normals = normalList.ToArray();

        mesh.SetVertices(vertsList);
        mesh.triangles = triList.ToArray();
        mesh.SetNormals(normalList);
        mesh.SetUVs(0, uvList);

        //mesh.RecalculateNormals();//THIS IS AN EXPENSIVE OPERATION. SHOULD NOT BE USED WHEN THE MESH IS GENAREATED AGAIN ANF AGAIN.
    }
    #endregion

    #region UTIL
    private void AddVertices(List<Vector3> vertsList, List<Vector3> normalList, List<Vector2> uvList)
    {
        //QUADS VERTICES ARE PLACES AT (0,0),(0,5),(5,0),(5,5)
        vertsList.Add(new Vector2(0, 0));
        vertsList.Add(new Vector2(0, 5));
        vertsList.Add(new Vector2(5, 0));
        vertsList.Add(new Vector2(5, 5));

        normalList.Add(Vector3.forward);
        normalList.Add(Vector3.forward);
        normalList.Add(Vector3.forward);
        normalList.Add(Vector3.forward);

        uvList.Add(new Vector2(0, 0));
        uvList.Add(new Vector2(0, 1));
        uvList.Add(new Vector2(1, 0));
        uvList.Add(new Vector2(1, 1));
    }

    private void AddTriangles(List<int> triList)
    {
        //LEFT HAND COORDINATE SYSTEM
        //WINDING ORDER IS IMPORATANT. IN A QUAD ONLY ONE SIDE WILL GET RENDRED.

        //FIRST TRIANGLE
        triList.Add(0);
        triList.Add(1);
        triList.Add(2);

        //SECOND TRIANGLE
        triList.Add(2);
        triList.Add(1);
        triList.Add(3);
    }
    #endregion
}