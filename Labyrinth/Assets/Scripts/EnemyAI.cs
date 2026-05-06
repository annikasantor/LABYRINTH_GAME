using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack }
    public State currentState = State.Patrol;

    [Header("References")]
    public Transform[] patrolPoints;
    public Transform player;

    [Header("Settings")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float attackDistance = 1.5f;
    public float attackCooldown = 1f;


    [Header("Detection Settings")]
    public float chaseDistance = 8f;
    public float viewDistance = 10f;
    [Range(0, 360)]
    public float viewAngle = 90f;
    public LayerMask obstacleMask;


    [Header("State Materials")]
    public Material patrolMaterial;
    public Material chaseMaterial;
    public Material attackMaterial;

    private Renderer rend;
    private int patrolIndex = 0;
    private float attackTimer = 0f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = patrolMaterial;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Attack:
                Attack();
                break;
        }

        attackTimer -= Time.deltaTime;
    }

    // -------- PATROL -------- //
    void Patrol()
    {
        rend.material = patrolMaterial;

        Transform point = patrolPoints[patrolIndex];
        MoveTowards(point.position, patrolSpeed);

        if (Vector3.Distance(transform.position, point.position) < 0.3f)
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;

        if (ShouldStartChasing())
            currentState = State.Chase;
    }

    // -------- CHASE -------- //
    void Chase()
    {
        rend.material = chaseMaterial;

        // Lost both detection methods, go back to patrol
        if (!PlayerInChaseRange() && !PlayerInViewRange())
        {
            currentState = State.Patrol;
            return;
        }

        MoveTowards(player.position, chaseSpeed);

        float dist = Vector3.Distance(transform.position, player.position);

        // Attack requires visibility + proximity
        if (dist <= attackDistance && PlayerInViewRange())
            currentState = State.Attack;

    }

    // -------- ATTACK -------- //
    void Attack()
    {
        rend.material = attackMaterial;

        // Lost all detection
        if (!PlayerInChaseRange() && !PlayerInViewRange())
        {
            currentState = State.Patrol;
            return;
        }

        if (!PlayerInViewRange())
        {
            currentState = State.Chase;
            return;
        }

        transform.LookAt(player);

        if (attackTimer <= 0f)
        {
            Debug.Log("Enemy attacked the player!");
            attackTimer = attackCooldown;
        }

    }

    // ---------------- Helpers ---------------- //
    bool PlayerInChaseRange()
    {
        Vector3 dirToPlayer = player.position - transform.position;
        float distance = dirToPlayer.magnitude;

        if (distance > chaseDistance)
            return false;

        // Awareness ray (short-range)
        if (Physics.Raycast(transform.position, dirToPlayer.normalized, distance, obstacleMask))
            return false;

        return true;
    }

    bool PlayerInViewRange()
    {
        Vector3 dirToPlayer = player.position - transform.position;
        float distance = dirToPlayer.magnitude;

        if (distance > viewDistance)
            return false;

        dirToPlayer.Normalize();

        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > viewAngle / 2f)
            return false;

        if (Physics.Raycast(transform.position, dirToPlayer, distance, obstacleMask))
            return false;

        return true;
    }

    bool ShouldStartChasing()
    {
        return PlayerInChaseRange() || PlayerInViewRange();
    }

    void MoveTowards(Vector3 target, float speed)
    {
        Vector3 dir = (target - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        transform.LookAt(target);
    }

    // -------- GIZMOS -------- //
    void OnDrawGizmosSelected()
    {
        // Chase / Awareness Radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        // Vision Range
        Gizmos.color = Color.cyan;
        //Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 left = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + left * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + right * viewDistance);

        // Attack Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

}
