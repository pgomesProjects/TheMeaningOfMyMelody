using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int columns;
    [SerializeField] private int rows;

    [SerializeField] private Tile tilePrefab;
    [SerializeField] private RectTransform container;

    private int tileSize = 100;

    private void OnEnable()
    {
        columns = (int)(LevelManager.Instance.beatTempo * (Mathf.Floor((float)LevelManager.GetFullSongDuration()) / 60.0) * 4);
        GenerateGridSize();
    }

    private void GenerateGridSize()
    {
        container.sizeDelta = new Vector2(columns * tileSize, rows * tileSize);
    }

    public int GetColumns() => columns;
}