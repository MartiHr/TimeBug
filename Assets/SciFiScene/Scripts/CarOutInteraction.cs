using UnityEngine;
using System;

public class CarOutInteraction : MonoBehaviour
{

    public GameObject flyingCar;
    public GameObject player;
    public GameObject staticCar;
    public GameObject aimCamera;
    public GameObject playerCamera;

    private Vector3 offset = new Vector3(-3, 0, 0); // Move player to the left

    private void setToPlayer()
    {
        //player.transform.position = new Vector3(flyingCar.transform.position.x - 5, Math.Abs(flyingCar.transform.position.y + 5), flyingCar.transform.position.z);

        player.transform.position = flyingCar.transform.TransformPoint(offset);

        staticCar.transform.position = new Vector3(flyingCar.transform.position.x, Math.Abs(flyingCar.transform.position.y - 2), flyingCar.transform.position.z);

        staticCar.transform.forward = flyingCar.transform.forward;
        player.transform.rotation = Quaternion.Euler(0, staticCar.transform.rotation.eulerAngles.y, 0);

        playerCamera.transform.rotation = Quaternion.Euler(playerCamera.transform.rotation.eulerAngles.x, staticCar.transform.rotation.eulerAngles.y, playerCamera.transform.rotation.eulerAngles.z);
        aimCamera.transform.rotation = Quaternion.Euler(aimCamera.transform.rotation.eulerAngles.x, staticCar.transform.rotation.eulerAngles.y, aimCamera.transform.rotation.eulerAngles.z);

        staticCar.SetActive(true);
        flyingCar.SetActive(false);
        player.SetActive(true);
        aimCamera.SetActive(true);
        playerCamera.SetActive(true);
    }


    //private void setToPlayer()
    //{
    //    player.transform.position = flyingCar.transform.TransformPoint(offset); // Relative to car

    //    staticCar.transform.position = flyingCar.transform.position + new Vector3(0, , 0); // Below car

    //    player.transform.rotation = flyingCar.transform.rotation; // Align with car
    //    staticCar.transform.rotation = flyingCar.transform.rotation; // Align with car

    //    staticCar.SetActive(true);
    //    flyingCar.SetActive(false);
    //    player.SetActive(true);
    //    aimCamera.SetActive(true);
    //    playerCamera.SetActive(true);
    //}


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
