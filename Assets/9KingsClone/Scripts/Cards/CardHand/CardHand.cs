using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Zenject;

public class CardHand : MonoBehaviour
{
    [Header("Hand Settings")]
    [SerializeField] private int _maxHandSize = 10;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private RectTransform _handCenter;
    [SerializeField] private float _radius = 400f;             
    [SerializeField] private float _animationDuration = 0.25f;
    [SerializeField] private float _maxCardSpacing = 150f;
    [Header("Curve Settings")]
    [SerializeField] private AnimationCurve _handCurve = AnimationCurve.Linear(0, 0, 1, 200);
    public event Action OnHandUpdated;

    private readonly List<Card> _cards = new List<Card>();

    private CardFactory _cardFactory;

    [Inject]
    public void Construct(CardFactory cardFactory)
    {
        _cardFactory = cardFactory;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawCard();
        }
    }
    private void RemoveCard(Card card)
    {
        if (card == null) return;
        if (!_cards.Contains(card)) return;

        
        card.RectTransform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
        {
            
            _cards.Remove(card);
            card.OnUse -= RemoveCard;
            Destroy(card.gameObject);
            Debug.Log("Card Removed");

            UpdateCardPositions();
        });
    }
    public void DrawCard()
    {
        if (_cards.Count >= _maxHandSize) return;

        Card cardGO = _cardFactory.Create(_handCenter);
        cardGO.OnUse += RemoveCard;
        RectTransform card = cardGO.RectTransform;

        card.anchoredPosition = Vector2.zero;
        card.localRotation = Quaternion.identity;

        _cards.Add(cardGO);
        UpdateCardPositions();
    }



    private void UpdateCardPositions()
    {
        if (_cards.Count == 0) return;

        int count = _cards.Count;
        float spacing = Mathf.Min(_maxCardSpacing, _radius / Mathf.Max(1, count - 1));

        int completed = 0;

        for (int i = 0; i < count; i++)
        {
            float x = (i - (count - 1) / 2f) * spacing;
            float t = (x + _radius) / (_radius * 2f);
            float y = _handCurve.Evaluate(t);

            float delta = 0.01f;
            float yNext = _handCurve.Evaluate(Mathf.Clamp01(t + delta));
            float angle = Mathf.Atan2(yNext - y, delta * (_radius * 2)) * Mathf.Rad2Deg;

            _cards[i].RectTransform.DOAnchorPos(new Vector2(x, y), _animationDuration)
                     .OnComplete(() =>
                     {
                         completed++;
                         if (completed == count)
                         {
                             
                             OnHandUpdated?.Invoke();
                         }
                     });

            _cards[i].RectTransform.DORotateQuaternion(Quaternion.Euler(0, 0, angle), _animationDuration);
        }
    }




}







