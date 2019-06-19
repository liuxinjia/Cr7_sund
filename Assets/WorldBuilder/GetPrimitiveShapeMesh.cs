using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPrimitiveShapeMesh : MonoBehaviour
{
    public struct PrimitiveVertex
    {
        public Vector3[] vertices;
        public Vector2[] uv;
        public int[] triangles;
    }

    public static Mesh GetPrimitiveVertex(PrimitiveType shape)
    {
        var go = GameObject.CreatePrimitive(shape);
        var mesh = go.GetComponent<MeshFilter>().mesh;
        Destroy(go);
        return mesh;
    }
}
