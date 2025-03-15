using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Camera playerCamera;
    public float shootingRange = 100f;
    public GameObject chargePrefab;
    public Transform weaponTip;
    public AudioSource gunshotSound;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //Play the gun sound
        gunshotSound.Play();

        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, shootingRange))
        {
            Debug.Log("Hit object with tag: " + hit.collider.tag);

            // Check if the hit object is an enemy and stop its movement
            EnemyMovement enemy = hit.collider.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                enemy.HitByBullet();
            }
        }

        Instantiate(chargePrefab, weaponTip.position, weaponTip.rotation);
    }
}
