using System.Collections.Generic;
using UnityEngine;
 
public class CustomRingMeshGenarator : MonoBehaviour
{
    #region PROPERTIES


    [Range(0.1f, 5)] [SerializeField] private float innerOctagonRingRadius = 0.5f;
    [Range(0.1f, 5)] [SerializeField] private float thickness = 0.5f;
    [Range(3, 72)] [SerializeField] private int pointCount = 3;

    private float outerOctagonRingRadius => innerOctagonRingRadius + thickness;

    private Mesh mesh;

    #endregion

    #region UNITY CALLBACKS
    private void FixedUpdate()
    {
        DrawCustomRingMesh();
    }

    private void OnDrawGizmosSelected()
    {
        //DrawMeshWithGozmos();
    }
    #endregion

    #region UTIL
    private void DrawCustomRingMesh()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        List<Vector3> vertsList = new List<Vector3>();//VERTICES LIST
        List<int> triList = new List<int>();//TRIANGLE LIST
        List<Vector3> normalList = new List<Vector3>(); //NORMAL LIST (WE NEED NORMALS IF NEED LIGTING TO WORK ON THE MESH)
        List<Vector2> uvList = new List<Vector2>();//MAP TEXTURES

        AddVertices(vertsList, normalList, uvList);
        AddTriangles(vertsList, triList);

        //mesh.vertices = vertsList.ToArray();//SETTING PROPERTIES MAKE MORE GARBAGE
        //mesh.triangles = triList.ToArray();
        //mesh.normals = normalList.ToArray();

        mesh.SetVertices(vertsList);
        mesh.triangles = triList.ToArray();
        mesh.SetNormals(normalList);
        mesh.SetUVs(0, uvList);

        //mesh.RecalculateNormals();//THIS IS AN EXPENSIVE OPERATION. SHOULD NOT BE USED WHEN THE MESH IS GENAREATED AGAIN ANF AGAIN.
    }

    private void AddVertices(List<Vector3> vertsList, List<Vector3> normalList, List<Vector2> uvList)
    {
        Vector2 _pivotPoint = new Vector2(0, 0);
        Vector2 _point1 = new Vector2(0, 0 + innerOctagonRingRadius);
        Vector2 _point2 = new Vector2(0, 0 + outerOctagonRingRadius);

        int _pointCount = pointCount; //OCTAGON HAS 8 VERTICES
        float offset = 360.0f / _pointCount;

        for (int i = 0; i < _pointCount; i++)
        {
            Vector3 _newPoint1 = RotatePoint(_pivotPoint, _point1, new Vector3(0, 0, -offset * i));

            vertsList.Add(_newPoint1);
            normalList.Add(-Vector3.forward);
            uvList.Add(GetUVPoint(_newPoint1));

            Vector3 _newPoint2 = RotatePoint(_pivotPoint, _point2, new Vector3(0, 0, -offset * i));

            vertsList.Add(_newPoint2);
            normalList.Add(-Vector3.forward);
            uvList.Add(GetUVPoint(_newPoint2));
        }
    }

    private void AddTriangles(List<Vector3> vertsList, List<int> triList)
    {
        //LEFT HAND COORDINATE SYSTEM
        //WINDING ORDER IS IMPORATANT. IN A QUAD ONLY ONE SIDE WILL GET RENDRED.

        for (int i = 0; i < vertsList.Count - 2; i += 2)
        {
            triList.Add(i);
            triList.Add(i + 1);
            triList.Add(i + 2);

            triList.Add(i + 1);
            triList.Add(i + 3);
            triList.Add(i + 2);
        }

        triList.Add(vertsList.Count - 2);
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

    private void DrawMeshWithGozmos()
    {
        Vector2 _pivotPoint = new Vector2(0, 0);
        Vector2 _point1 = new Vector2(0, 0 + innerOctagonRingRadius);
        Vector2 _point2 = new Vector2(0, 0 + outerOctagonRingRadius);

        int _pointCount = pointCount;

        List<Vector3> _innerPointList = new List<Vector3>();
        List<Vector3> _outerPointList = new List<Vector3>();

        float offset = 360.0f / _pointCount;

        for (int i = 0; i < _pointCount; i++)
        {
            Vector3 _newPoint1 = RotatePoint(_pivotPoint, _point1, new Vector3(0, 0, -offset * i));
            _newPoint1 += transform.position;
            _newPoint1 = transform.rotation * _newPoint1;
            _innerPointList.Add(_newPoint1);

            Vector3 _newPoint2 = RotatePoint(_pivotPoint, _point2, new Vector3(0, 0, -offset * i));
            _newPoint2 += transform.position;
            _newPoint2 = transform.rotation * _newPoint2;
            _outerPointList.Add(_newPoint2);
        }

        for (int i = 0; i < _innerPointList.Count; i++)
        {
            Gizmos.DrawLine(_innerPointList[i], _innerPointList[(i + 1) % _innerPointList.Count]);
            Gizmos.DrawLine(_outerPointList[i], _outerPointList[(i + 1) % _innerPointList.Count]);
        }
    }
    #endregion
}

//WHEN YOU MANIPULATE VERTEX IN THE SHADER IT'BE HANDLED BY THE GPU. IT'S MUCH FASTER THAN DOING IT ON CPU FOR EACH VERTEX.