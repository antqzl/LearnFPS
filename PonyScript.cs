using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class PonyScript : MonoBehaviour
{
    [Header("Pony things")]
    public NavMeshAgent ponyAgent;
    public Transform LookPoint;
    public LayerMask PlayerLayer;

    [Header("Pony Guarding Var")]
    public GameObject[] walkPoints;
    int currentPonyPosition = 0;
    public float ponySpeed;
    float walkingpointRadius = 2;

    [Header("Pony Mood/States")]
    public float visionRadius;
    public float attackingRadius;
    public bool playerInvisionRadius;
    public bool playerInattackingRadius;

    private void Awake()
    {
        ponyAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);

        if (!playerInvisionRadius && !playerInattackingRadius) Guard();
    }

    private void Guard()
    {
        if (Vector3.Distance(walkPoints[currentPonyPosition].transform.position, transform.position) < walkingpointRadius)
        {
            currentPonyPosition = Random.Range(0, walkPoints.Length);
            if(currentPonyPosition >= walkPoints.Length)
            {
                currentPonyPosition = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, walkPoints[currentPonyPosition].transform.position, Time.deltaTime * ponySpeed);
        //change pony facing
    }
}
