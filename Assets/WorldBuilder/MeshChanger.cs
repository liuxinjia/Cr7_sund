using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public abstract class MeshChanger : MonoBehaviour
{
    protected void UpdateMesh(Mesh newMesh)
    {
        GetComponent<MeshFilter>().mesh = newMesh;
        GetComponent<MeshCollider>().sharedMesh = newMesh;
    }
}
