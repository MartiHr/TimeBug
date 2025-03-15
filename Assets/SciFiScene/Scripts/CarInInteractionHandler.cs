using UnityEngine;

public class CarInInteraction : MonoBehaviour
{

    public GameObject flyingCar;
    public GameObject player;
    public GameObject staticCar;
    public GameObject aimCamera;
    public GameObject playerCamera;

    private AudioSource audioSource;

    private void setToCar()
    {
        flyingCar.transform.position = new Vector3(staticCar.transform.position.x, staticCar.transform.position.y + 2, staticCar.transform.position.z);
        staticCar.SetActive(false);
        flyingCar.SetActive(true);
        player.SetActive(false);
        aimCamera.SetActive(false);
        playerCamera.SetActive(false);
    }

    void Start()
    {
        audioSource = flyingCar.GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            setToCar();
            if (audioSource == null)
            {
                audioSource = flyingCar.GetComponent<AudioSource>();
            }
            audioSource.enabled = true;
            flyingCar.GetComponent<SoundManager>().enabled = true;
        }
    }
}
