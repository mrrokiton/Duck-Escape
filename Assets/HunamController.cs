using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class HunamController : MonoBehaviour
{
    public static int score = 0;
    static int max_score = 1;

    PlayerManager playerManager;

    PhotonView PV;

    GameObject[] players;

    public NavMeshAgent agent;
    public Transform player;
    public Animator animator;
    public LayerMask whatIsGround, whatIsPlayer;

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
        //PV.GetComponent<PhotonView>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        //playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
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
        if (distanceToWalkPoint.magnitude < 1f) {
            animator.SetBool("Standing", true);
            walkPointSet = false;
        }
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

        players = GameObject.FindGameObjectsWithTag("Player").OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position)).ToArray();
        player = players[0].transform;
        agent.SetDestination(player.position);
    }

    void OnTriggerEnter(Collider other)
    {
        Score(other);
    }

    void Score(Collider collider)
    {
        score++;
        if (PhotonNetwork.IsMasterClient)
        {
            if (collider.gameObject.tag == "Player")
            {
                PhotonNetwork.Destroy(gameObject.GetPhotonView());
                Debug.Log("DESTROYED " + score);

                if (score >= max_score)
                {
                    PhotonNetwork.LoadLevel(3);
                }
            }
        }
    }
}
