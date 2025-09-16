using UnityEngine;
using Zenject;

public class CardsInstallers : MonoInstaller
{
    [SerializeField] private CardHand _cardHand;
    [SerializeField] private CardSelector _cardPrefab;
    [SerializeField] private Camera _camera;

    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(_camera).AsSingle();
        Container.Bind<CardHand>().FromInstance(_cardHand).AsSingle();
        Container.Bind<CardFactory>().AsSingle().WithArguments(_cardPrefab, Container);
    }
}
