using UnityEngine;
using UnityEngine.EventSystems;

public class DragArea : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform handle;
    private float handleRange = 60f;

    private Vector2 inputVector = Vector2.zero;
    private RectTransform bg;

    private void Awake()
    {
        bg = GetComponent<RectTransform>();
        handle = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bg, eventData.position, eventData.pressEventCamera, out pos))
        {
            // Normalize position inside the circle (-1 to 1)
            pos.x /= (bg.sizeDelta.x / 2f);
            pos.y /= (bg.sizeDelta.y / 2f);

            inputVector = new Vector2(pos.x, pos.y);

            // Clamp magnitude so stick stays in circle
            if (inputVector.magnitude > 1f)
                inputVector = inputVector.normalized;

            // Move handle
            handle.anchoredPosition = inputVector * handleRange;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero; // reset handle to center
    }

    public float Horizontal() => inputVector.x;
    public float Vertical() => inputVector.y;
    public Vector2 Direction() => inputVector;
}