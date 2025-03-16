using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 2f;
    public LayerMask groundLayer;

    private bool isHit = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (player != null && !isHit)
        {
            Vector3 direction = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z);
            float distanceToPlayer = direction.magnitude;
            direction.Normalize();

            bool isWithinAttackRange = distanceToPlayer <= stoppingDistance;
            animator.SetBool("Attacking", isWithinAttackRange);

            if (!isWithinAttackRange)
            {
                float moveAmount = moveSpeed * Time.deltaTime;

                if (moveAmount > distanceToPlayer - stoppingDistance)
                {
                    moveAmount = distanceToPlayer - stoppingDistance;
                }

                transform.position += direction * moveAmount;
                AlignWithTerrain();
            }

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        AlignWithTerrain();
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void AlignWithTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 2f, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }

    public void HitByBullet()
    {
        isHit = true;
        Debug.Log(gameObject.name + " was hit!");

        if (animator != null)
        {
            animator.speed = 0;
        }
    }
}
