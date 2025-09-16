using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCard : MonoBehaviour,IDragging, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event Action StartDrag;
    public event Action<Vector2> Dragging;
    public event Action EndDrag;

    private RectTransform _rectTransform;
    private Canvas _canvas;
   

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       StartDrag?.Invoke();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            eventData.position,
            _canvas.worldCamera,
            out Vector2 localMousePos
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_canvas == null) return;

        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform.parent as RectTransform,
            eventData.position,
            _canvas.worldCamera,
            out Vector2 localPoint
        );

        _rectTransform.anchoredPosition = localPoint;

        
        Dragging?.Invoke(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag?.Invoke();
    }
}
