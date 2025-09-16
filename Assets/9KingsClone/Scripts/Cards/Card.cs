using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using Zenject;

[RequireComponent(typeof(DragCard))]
public class Card : MonoBehaviour
{
    private DragCard _dragHandler;
    private Camera _camera;        
    [SerializeField] private GameObject _tilePrefab;
    private RectTransform _rectTransform;

    public event Action<Card> OnUse;
    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    [Inject]
    private void Construct(Camera camera)
    {
        _camera = camera;
    }
    private void Awake()
    {
        _dragHandler = GetComponent<DragCard>();
    }

    private void OnEnable()
    {
        _dragHandler.Dragging += OnDragging;
        _dragHandler.EndDrag += OnEndDrag;
    }

    private void OnDisable()
    {
        _dragHandler.Dragging -= OnDragging;
        _dragHandler.EndDrag -= OnEndDrag;
    }

    private void OnDragging(Vector2 screenPos)
    {
        // Пуск луча
        Ray ray = _camera.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 0.1f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Grid grid = hitInfo.collider.GetComponent<Grid>();
            if (grid != null)
            {
                
                Vector2Int cell = grid.GetNearestCell(hitInfo.point);
                // TODO: визуальный preview
            }
        }
    }

    private void OnEndDrag()
    {
        
        Vector2 mousePos = Input.mousePosition;
        Ray ray = _camera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Grid grid = hitInfo.collider.GetComponent<Grid>();
            if (grid != null)
            {
                grid.AddTileAtPoint(_tilePrefab, hitInfo.point);
                OnUse?.Invoke(this);
                
            }
        }
    }
}

