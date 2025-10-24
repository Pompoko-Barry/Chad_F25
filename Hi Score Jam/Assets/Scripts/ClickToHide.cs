using UnityEngine;
using UnityEngine.UI;

public class ClickToHide : MonoBehaviour
{
    // Your buttons
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    // function to enable 
    public void enableButton(GameObject onButton)
    {
        onButton.SetActive(true);
    }

    // function to disable
    public void disableButton(GameObject offButton)
    {
        offButton.SetActive(false);
    }
}
