using System.Collections.Generic;
using UnityEngine;
 
public class OctagonRingMeshGenarator : MonoBehaviour
{
    #region PROPERTIES
    [SerializeField] private float innerOctagonRingRadius = 5f;
    [SerializeField] private float outerOctagonRingRadius = 8f;
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
        AddTriangles(vertsList,triList);

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
        Vector2 _pivotPoint = new Vector2(0, 0);
        Vector2 _point1 = new Vector2(0, 0 + innerOctagonRingRadius);
        Vector2 _point2 = new Vector2(0, 0 + outerOctagonRingRadius);

        int _pointCount = 8; //OCTAGON HAS 8 VERTICES

        for (int i = 0; i < _pointCount; i++)
        {
            Vector3 _newPoint1 = RotatePoint(_pivotPoint, _point1, new Vector3(0, 0, -45 * i));
            //Instantiate(obj, _newPoint1, Quaternion.identity);//DEBUGGING PURPOSES

            vertsList.Add(_newPoint1);
            normalList.Add(-Vector3.forward);
            uvList.Add(GetUVPoint(_newPoint1));

            Vector3 _newPoint2 = RotatePoint(_pivotPoint, _point2, new Vector3(0, 0, -45 * i));
            //Instantiate(obj, _newPoint2, Quaternion.identity);//DEBUGGING PURPOSES

            vertsList.Add(_newPoint2);
            normalList.Add(-Vector3.forward);
            uvList.Add(GetUVPoint(_newPoint2));
        }
    }

    private void AddTriangles(List<Vector3> vertsList, List<int> triList)
    {
        //LEFT HAND COORDINATE SYSTEM
        //WINDING ORDER IS IMPORATANT. IN A QUAD ONLY ONE SIDE WILL GET RENDRED.

        for(int i=0;i<vertsList.Count-2;i+=2)
        {
            triList.Add(i);
            triList.Add(i+1);
            triList.Add(i+2);

            triList.Add(i+1);
            triList.Add(i+3);
            triList.Add(i+2);
        }

        triList.Add(vertsList.Count-2);
        triList.Add(vertsList.Count - 1);
        triList.Add(0);

        triList.Add(0);
        triList.Add(vertsList.Count - 1);
        triList.Add(1);

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
        return (point / (outerOctagonRingRadius * 2)) + new Vector2(0.5f, 0.5f);
    }
    #endregion
}