using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource engineSound;
    public float minPitch = 1.0f;
    public float maxPitch = 2.0f;
    public float accelerationSpeed = 2.0f;

    void Update()
    {
        float thrustInput = Input.GetAxis("Vertical"); // W/S or Up/Down
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, Mathf.Abs(thrustInput));

        // Smoothly adjust pitch
        engineSound.pitch = Mathf.Lerp(engineSound.pitch, targetPitch, Time.deltaTime * accelerationSpeed);
    }
}
