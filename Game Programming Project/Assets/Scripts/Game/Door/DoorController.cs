using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool isOpen = false;
    public void Toggle()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            if(name == "Door Left")
            {
                transform.Translate(Vector3.left * 0.9f);
            }
            else
            {
                transform.Translate(Vector3.right * 0.9f);
            }
            StartCoroutine(DoorTimer());
        }
        else
        {
            if (name == "Door Left")
            {
                transform.Translate(Vector3.right * 0.9f);
            }
            else
            {
                transform.Translate(Vector3.left * 0.9f);
            }
        }
    }

    IEnumerator DoorTimer()
    {
        yield return new WaitForSeconds(5);
        Toggle();
    }
}
