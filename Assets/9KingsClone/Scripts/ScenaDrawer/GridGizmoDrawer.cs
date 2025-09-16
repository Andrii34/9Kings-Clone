using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridGizmoDrawer : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private Color _gridColor = Color.white;

    private Vector3[] _hexVertices = new Vector3[6];
    private List<Vector3> _allLineVertices = new List<Vector3>();
    private bool _needsUpdate = true;

    // Для отслеживания изменений
    private int _lastWidth;
    private int _lastHeight;
    private float _lastHexSize;

    private void OnValidate()
    {
        _needsUpdate = true;
    }

    private void Update()
    {
        if (_grid == null) return;

        // Проверяем, изменились ли параметры сетки
        if (_grid.Width != _lastWidth ||
            _grid.Height != _lastHeight ||
            _grid.HexLogicalSize != _lastHexSize)
        {
            _needsUpdate = true;
            _lastWidth = _grid.Width;
            _lastHeight = _grid.Height;
            _lastHexSize = _grid.HexLogicalSize;
        }
    }

    private void CacheGridData()
    {
        if (!_needsUpdate || _grid == null) return;

        _allLineVertices.Clear();
        for (int z = 0; z < _grid.Height; z++)
        {
            for (int x = 0; x < _grid.Width; x++)
            {
                float offset = (z % 2) * (_grid.HorizontalSpacing * 0.5f);
                Vector3 center = new Vector3(
                    x * _grid.HorizontalSpacing + offset,
                    0f,
                    z * _grid.VerticalSpacing
                ) + _grid.transform.position;

                for (int i = 0; i < 6; i++)
                {
                    float angle = i * 60f * Mathf.Deg2Rad;
                    _hexVertices[i] = center + new Vector3(
                        _grid.HexLogicalSize * Mathf.Sin(angle),
                        0f,
                        _grid.HexLogicalSize * Mathf.Cos(angle)
                    );
                }

                for (int i = 0; i < 6; i++)
                {
                    _allLineVertices.Add(_hexVertices[i]);
                    _allLineVertices.Add(_hexVertices[(i + 1) % 6]);
                }
            }
        }

        _needsUpdate = false;
    }

    private void OnDrawGizmos()
    {
        if (_grid == null) return;

        CacheGridData();
        Gizmos.color = _gridColor;
        for (int i = 0; i < _allLineVertices.Count; i += 2)
        {
            Gizmos.DrawLine(_allLineVertices[i], _allLineVertices[i + 1]);
        }
    }
}




