using UnityEngine;

[CreateAssetMenu(fileName = "CardConfigs", menuName = "Cards/CardConfigs")]
public class CardConfigs : ScriptableObject
{
    [field: SerializeField] public CartType Type { get; private set; }

}
