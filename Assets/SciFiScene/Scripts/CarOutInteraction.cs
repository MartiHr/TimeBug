using UnityEngine;

public class CarOutInteraction : MonoBehaviour
{

    public GameObject flyingCar;
    public GameObject player;
    public GameObject staticCar;
    public GameObject aimCamera;
    public GameObject playerCamera;

    private void setToPlayer()
    {
        player.transform.position = new Vector3(flyingCar.transform.position.x - 5, flyingCar.transform.position.y, flyingCar.transform.position.z);
        staticCar.transform.position = new Vector3(flyingCar.transform.position.x, flyingCar.transform.position.y - 2, flyingCar.transform.position.z);

        staticCar.SetActive(true);
        flyingCar.SetActive(false);
        player.SetActive(true);
        aimCamera.SetActive(true);
        playerCamera.SetActive(true);
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            setToPlayer();
        }
    }

}
