using UnityEngine;

public class MouseAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 lastPosition;

    void Start()
    {
        animator = GetComponentInChildren<Animator>(); // Find Animator in child
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody if using physics

        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate movement speed
        float speed = ((Vector2)transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        // Set animation state based on speed
        if (speed > 0.05f) // Small threshold to avoid jitter
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}