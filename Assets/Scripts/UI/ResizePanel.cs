using UnityEngine;
using UnityEngine.EventSystems;

public class ResizePanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public Vector2 _minSize;
    public Vector2 _maxSize;

    private RectTransform rectTransform;
    private Vector2 _currentPointerPosition;
    private Vector2 _previousPointerPosition;

    void Awake()
    {
        rectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        rectTransform.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out _previousPointerPosition);
    }

    public void OnDrag(PointerEventData data)
    {
        if (rectTransform == null)
        {
            return;
        }

        Vector2 sizeDelta = rectTransform.sizeDelta;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out _currentPointerPosition);
        Vector2 resizeValue = _currentPointerPosition - _previousPointerPosition;

        sizeDelta += new Vector2(resizeValue.x, -resizeValue.y);
        sizeDelta = new Vector2(Mathf.Clamp(sizeDelta.x, _minSize.x, _maxSize.x), Mathf.Clamp(sizeDelta.y, _minSize.y, _maxSize.y));

        rectTransform.sizeDelta = sizeDelta;

        _previousPointerPosition = _currentPointerPosition;
    }
}