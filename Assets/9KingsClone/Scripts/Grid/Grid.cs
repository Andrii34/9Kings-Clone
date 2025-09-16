using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private float _hexSize = 1f;
    [SerializeField] private int _hexMapWidth = 10;
    [SerializeField] private int _hexMapHeight = 10;

    private const float HORIZONTAL_MULTIPLIER = 1.732050807568877f; 
    private const float VERTICAL_MULTIPLIER = 1.5f;

    public float HorizontalSpacing { get; private set; }
    public float VerticalSpacing { get; private set; }
    public float HexLogicalSize { get; private set; }

   
    private readonly List<GameObject> _hexTiles = new List<GameObject>();

    private void OnValidate()
    {
        HexLogicalSize = _hexSize;
        HorizontalSpacing = HexLogicalSize * HORIZONTAL_MULTIPLIER;
        VerticalSpacing = HexLogicalSize * VERTICAL_MULTIPLIER;
    }

    public int Width => _hexMapWidth;
    public int Height => _hexMapHeight;

   
    public void AddTile(GameObject tilePrefab, int col, int row)
    {
        if (tilePrefab == null) return;

        if (col < 0 || col >= _hexMapWidth || row < 0 || row >= _hexMapHeight)
            return;

        GameObject tileInstance = Instantiate(tilePrefab,Vector3.up,Quaternion.identity);
        tileInstance.transform.position = GetWorldPosition(col, row);
        _hexTiles.Add(tileInstance);
    }

 
    public void AddTileAtPoint(GameObject tilePrefab, Vector3 hitPoint)
    {
        if (tilePrefab == null) return;

        Vector2Int cell = GetNearestCell(hitPoint);
        AddTile(tilePrefab, cell.x, cell.y);
    }

  
    public Vector3 GetWorldPosition(int col, int row)
    {
        float offset = (row % 2) * (HorizontalSpacing * 0.5f);
        Vector3 position = new Vector3(
            col * HorizontalSpacing + offset,
            1f,
            row * VerticalSpacing
        ) + transform.position;
        return position;
    }

  
    public Vector2Int GetNearestCell(Vector3 worldPosition)
    {
        Vector3 localPos = worldPosition - transform.position;
        float rowF = Mathf.Round(localPos.z / VerticalSpacing);
        float colF = Mathf.Round((localPos.x - (rowF % 2) * (HorizontalSpacing * 0.5f)) / HorizontalSpacing);

        int row = Mathf.Clamp((int)rowF, 0, _hexMapHeight - 1);
        int col = Mathf.Clamp((int)colF, 0, _hexMapWidth - 1);

        return new Vector2Int(col, row);
    }

  
    public List<GameObject> GetAllTiles() => _hexTiles;
}

