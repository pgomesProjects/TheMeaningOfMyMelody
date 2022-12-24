using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int columns;
    [SerializeField] private int rows;

    [SerializeField] private Tile tilePrefab;
    [SerializeField] private RectTransform container;

    private Dictionary<Vector2, Tile> tiles;

    private int tileSize = 100;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        container.sizeDelta = new Vector2(columns * tileSize, rows * tileSize);

        for(int x = 0; x < columns; x++)
        {
            for(int y = 0; y < rows; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x * tileSize, y * tileSize), Quaternion.identity);
                spawnedTile.transform.SetParent(container, false);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.CheckColor(isOffset);

                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if(tiles.TryGetValue(pos, out var tile))
            return tile;

        return null;
    }
}
