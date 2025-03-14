using UnityEngine;

public class Sway : MonoBehaviour
{
    public float amountX;
    public float amountY;
    public float maxAmount;
    public float smoothAmmount;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }
    void Update()
    {
        float movementX = -Input.GetAxis("Mouse X") * amountX;
        float movementY = -Input.GetAxis("Mouse Y") * amountY;
        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmmount);
    }
}
