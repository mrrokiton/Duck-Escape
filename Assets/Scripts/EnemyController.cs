using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Photon.Pun;

public class EnemyController : MonoBehaviour
{
    PlayerManager playerManager;

    PhotonView PV;
    Rigidbody rb;

    public static int death = 0;

    GameObject[] players;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public Animator animator;

    //patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attack
    public float timeBetweenAttacks;

    //state
    public float sightRange;
    public bool playerInSightRange;

    void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        
        
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (playerInSightRange)
            ChasePlayer();
        else
            Patroling();
    }

    private void Patroling()
    {
        animator.SetBool("Chasing", false);
        animator.SetBool("Standing", false);
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            animator.SetBool("Standing", true);
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        animator.SetBool("Chasing", true);
        animator.SetBool("Standing", false);

        players = GameObject.FindGameObjectsWithTag("Player").OrderBy(x=> Vector3.Distance(this.transform.position, x.transform.position)).ToArray();
        player = players[0].transform;
        agent.SetDestination(player.position);
    }

    void OnTriggerEnter(Collider collider)
    {
        Death(collider);
    }

    void Death(Collider collider)
    {
        death++;
        if (PhotonNetwork.IsMasterClient)
        { 
            if (collider.gameObject.tag == "Player")
            {
                Debug.Log("DEATH " + death);

                if (death > 0)
                {
                    death = 0;
                    PhotonNetwork.LoadLevel(3);
                }
            }
        }
    }

}
