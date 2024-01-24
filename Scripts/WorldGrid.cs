using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using rand = UnityEngine.Random;

public class WorldGrid : MonoBehaviour
{
    [SerializeField]
    public Int2D gridSize;
    [SerializeField]
    Vector2 origin;
    [SerializeField]
    GameObject tilePrefab;
    [SerializeField]
    float tileSize;
    [SerializeField]
    GameObject tilesContainer;
    [SerializeField]
    GameObject ground;
    Vector3 groundSize;
    List<List<Tile>> tiles = new List<List<Tile>>();

    public void Init() {
        groundSize = ground.GetComponent<MeshRenderer>().bounds.size;
        GenerateGrid();
    }

    void GenerateGrid() {
        for (int i = 0; i < gridSize.x; i++) {
            tiles.Add(new List<Tile>());
            for (int j = 0; j < gridSize.z; j++) {
                GameObject tileObj = Instantiate(tilePrefab, tilesContainer.transform);
                Tile tile = tileObj.GetComponent<Tile>();
                tile.SetGridLocation(new Int2D(i, j));
                tile.SetLocation((i * tileSize) + origin.x, (j * tileSize) + origin.y);
                tiles[i].Add(tile);
            }
        }
    }

    public float GetTileSize() {
        return tileSize;
    }

    public List<Tile> GetTilesFromTile(Tile tile, Int2D size) {
        List<Tile> tiles = new List<Tile>();
        Int2D tileLocation = tile.GetGridLocation();
        for (int i = 0; i < size.x; i++) {
            for (int j = 0; j < size.z; j++) {
                Tile new_tile = GetTileAt(new Int2D(tileLocation.x - i, tileLocation.z - j));
                if (new_tile == null) { return null; }
                tiles.Add(new_tile);
            }
        }
        return tiles;
    }

    public bool AllBuildable(List<Tile> tiles) {
        if (tiles == null) { return false; }
        foreach (Tile tile in tiles) {
            if (tile.IsOccupied() || tile.HasDarkness()) { return false; }
        }
        return true;
    }

    public Tile GetTileAt(Int2D position) {
        if (0 <= position.x && position.x < gridSize.x
        && 0 <= position.z && position.z < gridSize.z) {
            return tiles[position.x][position.z];
        }
        return null;
    }

    public Tile RandomTile() {
        return tiles[rand.Range(0, gridSize.x)][rand.Range(0, gridSize.z)];
    }

    public int TileCount() {
        return gridSize.x * gridSize.z;
    }

    public Tile GetTileAtWorldPosition(Vector3 pos) {
        float offsetX = groundSize.x / 2;
        float offsetZ = groundSize.z / 2;
        int x = Mathf.Clamp(Mathf.FloorToInt((pos.x + offsetX) / groundSize.x * gridSize.x), 0, gridSize.x-1);
        int z = Mathf.Clamp(Mathf.FloorToInt((pos.z + offsetZ) / groundSize.z * gridSize.z), 0, gridSize.z-1);
        return tiles[x][z];
    }
}


[System.Serializable]
public class Int2D
{
    public int x;
    public int z;

    public Int2D(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}