using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    public AudioSource audioSource; // Assign in the Inspector
    public AudioClip[] sandFootsteps;
    public AudioClip[] tileFootsteps;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        //if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        //{
            if (!audioSource.isPlaying)
            {
                PlayFootstepSound();
            }
        //}
    }

    void PlayFootstepSound()
    {
        string terrainTag = DetectTerrainTag();
        AudioClip clip = null;

        if (terrainTag == "Sand")
        {
            clip = sandFootsteps[Random.Range(0, sandFootsteps.Length)];
        }
        else if (terrainTag == "Tile")
        {
            clip = tileFootsteps[Random.Range(0, tileFootsteps.Length)];
        }

        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    string DetectTerrainTag()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            return hit.collider.tag;
        }
        return "Default";
    }
}
