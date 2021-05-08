using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyPatrol : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private float distThreshold = 3f;

    public void NextTarget () {
        if (currentIndex == waypoints.Count-1) {
            currentIndex = 0;
        }else{
            currentIndex++;
        }
    }

    private void Update() {
        navMesh.SetDestination(waypoints[currentIndex].position);
        if (Vector3.Distance(waypoints[currentIndex].position,navMesh.transform.position) > distThreshold) return;
        NextTarget();
    }
}
