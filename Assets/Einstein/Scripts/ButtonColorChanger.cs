using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    public Button myButton;
    public Color targetColor = Color.green;

    void Start()
    {
        myButton.onClick.AddListener(ChangeColor);
    }

    public void ChangeColor()
    {
        myButton.GetComponent<Image>().color = targetColor;
    }
}
