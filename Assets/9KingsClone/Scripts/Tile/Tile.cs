using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }
    public TileOccupant Occupant { get; private set; } 

    public void SetOccupant(TileOccupant ocupant)
    {
        Occupant = ocupant;
        if (ocupant != null) ocupant.transform.position = transform.position;
    }
    public void ClearTile()
    {
        Occupant = null;
        Occupant.Remove();
    }
}
