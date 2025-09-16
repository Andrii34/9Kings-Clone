using System;
using UnityEngine;

public interface IDragging
{
    public event Action StartDrag;
    public event Action<Vector2> Dragging;
    public event Action EndDrag;
}
