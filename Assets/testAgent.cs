using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class testAgent : MonoBehaviour
{
    protected NavMeshAgent Navigator => GetComponent<NavMeshAgent>();
    // Start is called before the first frame update
    void Start()
    {
        SetupNavigation();
        Navigator?.SetDestination(Vector3.zero);
    }
    bool SetupNavigation()
    {
        var nav = Navigator;
        nav.updateRotation = false;
        nav.updateUpAxis = false;
        NavMeshHit hit;
        bool success = NavMesh.SamplePosition(transform.position,
            out hit, 1.0f, NavMesh.AllAreas);
        if (success)
        {
            transform.position = hit.position;
        }
        Debug.Log(success);
        return success;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
