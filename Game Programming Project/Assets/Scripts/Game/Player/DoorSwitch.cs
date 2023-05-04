using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorSwitch : MonoBehaviour
{
    public Camera cam;
    public LayerMask layerMask;
    public float distance = 3f;

    public TMP_Text deductionText;

    private void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance, layerMask))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObject hitObject = hitInfo.collider.gameObject;

                Transform doorTransformLeft = hitObject.transform.Find("Door Left");
                Transform doorTransformRight = hitObject.transform.Find("Door Right");
                DoorController doorControllerLeft = doorTransformLeft.GetComponent<DoorController>();
                DoorController doorControllerRight = doorTransformRight.GetComponent<DoorController>();
                Transform wall = hitObject.transform.parent;
                ToggleLight lights = wall.GetChild(0).GetComponent<ToggleLight>();
                if (lights.toggle)
                {
                    if(hitObject.name == wall.GetChild(1).gameObject.name)
                    {
                        Deduct();
                    }
                }
                else
                {
                    if (hitObject.name == wall.GetChild(2).gameObject.name)
                    {
                        Deduct();
                    }
                }
                doorControllerLeft.Toggle();
                doorControllerRight.Toggle();
                SFXManager.instance.PlaySFX(4);
            }
        }
    }

    private void Deduct()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameData.instance.gameTimer -= 10;
            deductionText.text = "TIME DEDUCTED";
            StartCoroutine(RemoveText());
        }
        else if(SceneManager.GetActiveScene().name == "Infinite")
        {
            InfiniteData.instance.decrementation += 50;
            InfiniteData.instance.gameLives--;
            deductionText.text = "SCORE DEDUCTED";
            StartCoroutine(RemoveText());
        }
    }

    IEnumerator RemoveText()
    {
        yield return new WaitForSeconds(2);
        deductionText.text = "";
    }
}
