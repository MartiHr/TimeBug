using NUnit.Framework.Interfaces;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    #region Movement
    public float currentMoveSpeed;
    public float walkSpeed = 3, walkBackSpeed = 2;
    public float fightSpeed = 1, fightBackSpeed = 1;
    public float runSpeed = 7, runBackSpeed = 5;
    public float crouchSpeed = 2, crouchBackSpeed = 1;
    public float airSpeed = 1.5f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public float hzInput, vInput;
    public CharacterController controller;
    #endregion

    [SerializeField]
    float groundYOffset;
    [SerializeField]
    LayerMask groundMask;
    Vector3 spherePos;

    #region Gravity
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpForce = 10f;
    [HideInInspector] public bool jumped;
    Vector3 velocity;
    #endregion

    #region States
    public MovementBaseState previousState;
    public MovementBaseState currentState;

    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public CrouchState Crouch = new CrouchState();
    public RunState Run = new RunState();
    public JumpState Jump = new JumpState();
    //public RiffleState Riffle = new RiffleState();
    //public RiffleWalkState RiffleWalk = new RiffleWalkState();
    #endregion

    [HideInInspector] public Animator anim;
    //[HideInInspector] public ShootingManager shootingManager;

    private void Awake()
    {
        //shootingManager = GetComponent<ShootingManager>();
        anim = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        SwitchState(Idle);
    }

    void Update()
    {
        GetDirectionAndMove();
        Gravity();
        Falling();

        // Apply damping for smoother transitions in the blend tree
        float dampTime = 0.1f;  // Adjust this value to control the smoothness
        anim.SetFloat("hzInput", hzInput, dampTime, Time.deltaTime);
        anim.SetFloat("vInput", vInput, dampTime, Time.deltaTime);

        currentState.UpdateState(this);
    }

    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    void GetDirectionAndMove()
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        dir = transform.forward * vInput + transform.right * hzInput;
        Vector3 airDir = Vector3.zero;

        if (!isGrounded())
        {
            // When is in the air, adjust the speed
            airDir = transform.forward * vInput + transform.right * hzInput;
        }
        else
        {
            float targetSpeed = dir.magnitude * currentMoveSpeed;
            if (targetSpeed > currentMoveSpeed)
            {
                currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, targetSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, targetSpeed, deceleration * Time.deltaTime);
            }
        }

        controller.Move((dir.normalized * currentMoveSpeed + airDir.normalized * airSpeed) * Time.deltaTime);
    }

    public bool isGrounded()
    {
        // Adjust sphere position to be slightly below the player's feet
        spherePos = new Vector3(transform.position.x, transform.position.y - controller.height / 2 - 1f, transform.position.z);

        //Debug.Log(Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask) ? "is grounded" : "it is not");
        // Perform the ground check using Physics.CheckSphere
        return Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask);
    }

    private void OnDrawGizmos()
    {
        if (controller != null)
        {
            Gizmos.color = Color.red;
            Vector3 spherePos = new Vector3(transform.position.x, transform.position.y - controller.height / 2 - 0.5f, transform.position.z);
            Gizmos.DrawWireSphere(spherePos, controller.radius - 0.05f);
        }
    }

    void Gravity()
    {
        if (isGrounded())
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f;  // Small value to ensure player sticks to the ground
            }
        }
        else
        {
            // Apply gravity over time when not grounded
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void Falling()
    {
        //Debug.Log(!isGrounded() ? "Falling" : "On the ground");
        anim.SetBool("Falling", !isGrounded());
    }

    public void JumpForce()
    {
        if (isGrounded())  // Ensure jump can only be triggered when grounded
        {
            velocity.y += jumpForce;
        }
    }

    public void Jumped()
    {
        jumped = true;
    }
}