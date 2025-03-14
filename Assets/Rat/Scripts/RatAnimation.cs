using UnityEngine;

public class MouseAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
;
    }

    void Update()
    {
        // Calculate movement speed
        float speed = ((Vector2)transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }
}