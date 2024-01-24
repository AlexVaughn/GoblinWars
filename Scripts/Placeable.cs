using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Placeable : MonoBehaviour
{
    [SerializeField]
    Int2D size;
    List<Tile> tiles;
    [HideInInspector]
    public bool isHidden = true;

    public void SetTiles(List<Tile> tiles) {
        this.tiles = tiles;
        foreach (Tile tile in tiles) {
            tile.SetPlaceable(this);
            // tile.gameObject.name = tile.GetGridLocation().x.ToString() + ","
            //     + tile.GetGridLocation().z.ToString();
            // Debug.Log("Set tile " + tile.gameObject.name);
            // tile.SetDarkness(false);
        }
    }

    public List<Tile> GetTiles() {
        return tiles;
    }

    public Int2D GetSize() {
        return size;
    }

    public Vector3 GetWorldTilePlace(WorldGrid worldGrid, Tile tile) {
        return new Vector3(
            tile.transform.position.x - (size.x - 1) * worldGrid.GetTileSize() / 2,
            tile.transform.position.y,
            tile.transform.position.z - (size.z - 1) * worldGrid.GetTileSize() / 2);
    }

    public bool Place(WorldGrid worldGrid, Tile tile) {
        List<Tile> tiles = worldGrid.GetTilesFromTile(tile, size);
        if (tiles == null) { return false; }
        SetTiles(tiles);
        transform.position = GetWorldTilePlace(worldGrid, tile);
        return true;
    }

    public void Reveal() {
        foreach (Tile tile in tiles) { tile.SetDarkness(false); }
        gameObject.SetActive(true);
        isHidden = false;
    }
}