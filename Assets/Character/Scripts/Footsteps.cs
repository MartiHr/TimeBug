using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource footstepsSound, sprintSound, jumpSound, landSound;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    bool wasGroundedLastFrame = true; // Track previous ground state

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = characterController.isGrounded;

        if (!wasGroundedLastFrame && isGrounded)
        {
            Debug.Log("land");
            landSound.Play();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpSound.Play();
        }

        wasGroundedLastFrame = isGrounded;

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) && characterController.isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                footstepsSound.enabled = false;
                sprintSound.enabled = true;
            }
            else
            {
                footstepsSound.enabled = true;
                sprintSound.enabled = false;
            }
        }
        else
        {
            sprintSound.enabled = false;
            footstepsSound.enabled = false;
        }
    }
}
