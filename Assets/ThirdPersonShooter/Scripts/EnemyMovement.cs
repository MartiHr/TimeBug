using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 2f;

    private float initialYPosition;
    private bool isHit = false;  // Track if enemy was hit

    private Animator animator;

    void Start()
    {
        initialYPosition = transform.position.y;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (player != null && !isHit)
        {
            Vector3 direction = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > stoppingDistance)
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, initialYPosition, transform.position.z);
            }
            else 
            {
                animator.SetBool("Attacking", true);
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Stop enemy when hit
    public void HitByBullet()
    {
        isHit = true;
        Debug.Log(gameObject.name + " was hit!");

        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.speed = 0; // Freezes animation on the current frame
        }
    }
}
