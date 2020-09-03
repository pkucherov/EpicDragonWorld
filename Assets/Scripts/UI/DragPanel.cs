using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 _pointerOffset;
    private RectTransform _canvasRectTransform;
    private RectTransform _panelRectTransform;

    private void Awake()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            _canvasRectTransform = canvas.transform as RectTransform;
            _panelRectTransform = transform.parent as RectTransform;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        _panelRectTransform.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_panelRectTransform, data.position, data.pressEventCamera, out _pointerOffset);
        MainManager.Instance.SetDraggingWindow(true);
    }

    public void OnDrag(PointerEventData data)
    {
        if (_panelRectTransform == null)
        {
            return;
        }

        Vector2 pointerPostion = ClampToWindow(data);
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, pointerPostion, data.pressEventCamera, out localPointerPosition))
        {
            _panelRectTransform.localPosition = localPointerPosition - _pointerOffset;
        }

        StartCoroutine(CheckDragStop());
    }

    private IEnumerator CheckDragStop()
    {
        yield return new WaitForSeconds(0.5f);
        if (!InputManager.LEFT_MOUSE_PRESS && !InputManager.RIGHT_MOUSE_PRESS)
        {
            MainManager.Instance.SetDraggingWindow(false);
        }
    }

    Vector2 ClampToWindow(PointerEventData data)
    {
        Vector2 rawPointerPosition = data.position;
        Vector3[] canvasCorners = new Vector3[4];
        _canvasRectTransform.GetWorldCorners(canvasCorners);

        float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
        float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);

        Vector2 newPointerPosition = new Vector2(clampedX, clampedY);
        return newPointerPosition;
    }
}