using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IEffect
{

    int Priority { get; }  
    int Duration { get; }        
    void TickDuration();
    UniTask ApplyAsync(IEffectTarget building);

    bool IsExpired { get; }
}
