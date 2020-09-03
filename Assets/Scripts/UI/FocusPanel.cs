using UnityEngine;
using UnityEngine.EventSystems;

public class FocusPanel : MonoBehaviour, IPointerDownHandler
{
    private RectTransform _panel;

    private void Awake()
    {
        _panel = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        _panel.SetAsLastSibling();
    }
}