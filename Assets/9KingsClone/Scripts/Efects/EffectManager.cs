
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

public class EffectManager
{
    private List<IEffectTarget> _units = new List<IEffectTarget>();
    private List<IEffectTarget> _buildings = new List<IEffectTarget>();
    private List<IEffectTarget> _tiles = new List<IEffectTarget>();
    

    public void RegisterUnit(IEffectTarget unit) => _units.Add(unit);
    public void RegisterBuilding(IEffectTarget building) => _buildings.Add(building);
    public void RegisterTile(IEffectTarget tile) => _tiles.Add(tile);

    
    public async UniTask ApplyEffectsSequentially()
    {
        foreach (var tile in _tiles)
        {
             await tile.UplayEffects();
        }

        foreach (var build in _buildings)
        {
          await build.UplayEffects();
        }

        
       
        foreach (var unit in _units)
        {
            await unit.UplayEffects();
        }  
    }

}

