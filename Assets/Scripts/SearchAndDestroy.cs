using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchAndDestroy : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;

    // Patroling
    public LayerMask whatIsGround, whatIsPlayer, whatIsObstacle;
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange = 10;

    // States
    public float sightRange, attackRange, changeAndAttackRange = 10;
    public bool playerInSightRange, playerInAttackRange;

    // Shooting
    public GameObject projectilePrefab;
    public GameObject shootPoint;
    public float shootForce = 7f;
    public float rateOfFire = 1;
    public bool alreadyAttacked, playerInSight;

    void Awake(){
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // check for sight range & attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInSight = !Physics.Linecast(transform.position, player.transform.position, whatIsObstacle);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange && !playerInSight) Patroling();
        if (playerInSightRange && !playerInAttackRange && playerInSight) ChasePlayer();
        if (playerInSightRange && playerInAttackRange && !playerInSight) ChasePlayer();
        if (playerInSightRange && playerInAttackRange && playerInSight) AttackPlayer();

    }

    private void Patroling()
    {
        // search for new walk point
        if(!walkPointSet) SearchWalkPoint();

        if(walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < agent.stoppingDistance)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        // offset from player position
        walkPoint = new Vector3(player.transform.position.x + randomX, transform.position.y, player.transform.position.z + randomZ);

        // check if walkPoint is reachable
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(walkPoint, path);
        if (path.status == NavMeshPathStatus.PathComplete)
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        // save player position (after we lose him, go to last known position)
        walkPoint = player.transform.position;
        agent.SetDestination(player.transform.position);

        // if distance is not so much, shoot some hearths
        float distanceToPlayer = Vector3.Magnitude(player.transform.position - transform.position);
        if (distanceToPlayer < changeAndAttackRange && playerInSight)
            Shoot();
    }

    private void AttackPlayer()
    {
        // stop and start shooting
        agent.SetDestination(transform.position);
        agent.transform.LookAt(player.transform.position);
        Shoot();
    }

    private void Shoot()
    {
        if (!alreadyAttacked){
            // TODO: add shotgun style of shooting (multiple projectiles, spread)
            int noOfProjectiles = Random.Range(2, 5);
            for (int i=0; i < noOfProjectiles; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, shootPoint.transform.position, shootPoint.transform.rotation);
                Vector3 direction = transform.forward.normalized * shootForce;
                // randomize shells
                float x = Random.Range(-0.1f, 0.1f);
                float y = Random.Range(-0.1f, 0.1f);
                Vector3 directionWithSpread = direction + new Vector3(x, y, 0);

                projectile.GetComponent<Rigidbody>().AddForce(directionWithSpread, ForceMode.Impulse);
            }

            alreadyAttacked = true;
            Invoke("ResetAttack", rateOfFire);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, changeAndAttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.DrawCube(walkPoint, new Vector3(0.1f, 15f, 0.1f));
    }
}
