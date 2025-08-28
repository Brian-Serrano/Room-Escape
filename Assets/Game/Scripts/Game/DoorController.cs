using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Vector3 openOffset;

    private bool isOpen = false;
    private Vector3 closedPos;
    private Vector3 openPos;

    private void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + openOffset;
    }

    public void Toggle()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            transform.position = openPos;
            StopAllCoroutines();
            StartCoroutine(DoorTimer());
        }
        else
        {
            transform.position = closedPos;
        }
    }

    private IEnumerator DoorTimer()
    {
        yield return new WaitForSeconds(5f);
        Toggle();
    }
}
