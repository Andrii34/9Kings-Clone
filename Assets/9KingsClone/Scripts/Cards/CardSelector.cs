using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Zenject;
public class CardSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Settings")]
    [SerializeField] private float _hoverHeight = 50f; 
    [SerializeField] private float _hoverScale = 1.1f; 
    [SerializeField] private float _animationDuration = 0.25f;

    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private Vector3 _originalScale;
    private int _originalSiblingIndex;
    private CardHand _cardHand;
    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
                
            }
            return _rectTransform;
        }
    }
    [Inject]
    private void Construct(CardHand cardHand)
    {
        _cardHand = cardHand;
    }
    private void OnEnable()
    {
        _cardHand.OnHandUpdated += UpdateStartPos;
    }
    private void OnDisable()
    {
        _cardHand.OnHandUpdated -= UpdateStartPos;
    }
    private void UpdateStartPos()
    {
        Debug.Log("Hand Updated");
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        _originalPosition = _rectTransform.anchoredPosition;
        _originalScale = _rectTransform.localScale;
        _originalSiblingIndex = _rectTransform.GetSiblingIndex();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _rectTransform.SetAsLastSibling(); 
        _rectTransform.DOAnchorPos(_originalPosition + Vector2.up * _hoverHeight, _animationDuration);
        _rectTransform.DOScale(_originalScale * _hoverScale, _animationDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rectTransform.SetSiblingIndex(_originalSiblingIndex);
        _rectTransform.DOAnchorPos(_originalPosition, _animationDuration);
        _rectTransform.DOScale(_originalScale, _animationDuration);
    }
}
