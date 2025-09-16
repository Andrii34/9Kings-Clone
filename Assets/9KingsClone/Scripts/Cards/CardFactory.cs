using UnityEngine;
using Zenject;



public class CardFactory : IFactory<RectTransform, Card>
{
    private readonly CardSelector _prefab;
    private readonly DiContainer _container;

    public CardFactory(CardSelector prefab, DiContainer container)
    {
        _prefab = prefab;
        _container = container;
    }

    public Card Create(RectTransform parent)
    {
        Card card = _container.InstantiatePrefabForComponent<Card>(_prefab, parent);
        RectTransform rect = card.RectTransform;
        rect.SetParent(parent, false);
        rect.anchoredPosition = Vector2.zero;
        rect.localRotation = Quaternion.identity;
        rect.localScale = Vector3.one;
        return card;
    }
}
