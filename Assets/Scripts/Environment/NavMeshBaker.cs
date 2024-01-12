using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    NavMeshSurface navMesh;

    public static NavMeshBaker instance;


    private void Start()
    {
        navMesh = GetComponent<NavMeshSurface>();
        instance = this;
    }

    public void NavMeshBake()
    {
        navMesh.BuildNavMesh();
    }
}
