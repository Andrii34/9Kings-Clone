using NUnit.Framework;
using System;
using UnityEngine;

public class TileOccupant : MonoBehaviour
{
    
    internal void Remove()
    {
        Destroy(gameObject);
    }
}
