using System.Collections.Generic;
using UnityEngine;
 
public class OctagonMeshGenarator : MonoBehaviour
{
    #region PROPERTIES
    [SerializeField] private float octagonRingRadius = 5f;
    public GameObject obj;

    private Mesh mesh;

    #endregion

    #region UNITY CALLBACKS
    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        List<Vector3> vertsList = new List<Vector3>();//VERTICES LIST
        List<int> triList = new List<int>();//TRIANGLE LIST
        List<Vector3> normalList = new List<Vector3>(); //NORMAL LIST (WE NEED NORMALS IF NEED LIGTING TO WORK ON THE MESH)
        List<Vector2> uvList = new List<Vector2>();//MAP TEXTURES

        AddVertices(vertsList, normalList, uvList);
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
        Vector2 _pivotPoint = new Vector2(0,0);
        Vector2 point1 = new Vector2(0, 0 + octagonRingRadius);

        int _pointCount = 8; //OCTAGON HAS 8 VERTICES

        for (int i = 0; i < _pointCount; i++)
        {
            Vector3 newPoint = RotatePoint(_pivotPoint, point1, new Vector3(0, 0, -45 * i));
            Instantiate(obj,newPoint,Quaternion.identity);//DEBUGGING PURPOSES

            vertsList.Add(newPoint);
            normalList.Add(-Vector3.forward);
            uvList.Add(GetUVPoint(newPoint));
        }
    }

    private void AddTriangles(List<int> triList)
    {
        //LEFT HAND COORDINATE SYSTEM
        //WINDING ORDER IS IMPORATANT. IN A QUAD ONLY ONE SIDE WILL GET RENDRED.

        //TRIANGLE 1
        triList.Add(0);
        triList.Add(1);
        triList.Add(2);

        //TRIANGLE 2
        triList.Add(0);
        triList.Add(2);
        triList.Add(3);

        //TRIANGLE 3
        triList.Add(0);
        triList.Add(3);
        triList.Add(4);

        //TRIANGLE 4
        triList.Add(0);
        triList.Add(4);
        triList.Add(5);

        //TRIANGLE 5
        triList.Add(0);
        triList.Add(5);
        triList.Add(6);

        //TRIANGLE 6
        triList.Add(0);
        triList.Add(6);
        triList.Add(7);
    }

    private Vector3 RotatePoint(Vector3 pivot, Vector3 point, Vector3 angle)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angle) * dir;
        point = pivot + dir;
        return point;
    }

    private Vector2 GetUVPoint(Vector2 point)
    {
        Debug.Log((point / (octagonRingRadius * 2)) + new Vector2(0.5f, 0.5f));
        return (point / (octagonRingRadius * 2)) + new Vector2(0.5f,0.5f);
    }
    #endregion
}