using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject palacePrefab;
    Controller controller;
    WorldGrid worldGrid;
    Tile palaceTile;
    [HideInInspector]
    public GameObject palace;
    [SerializeField]
    GameObject tree1Prefab;
    [SerializeField]
    GameObject stoneDeposit1Prefab;

    public void Init(WorldGrid worldGrid, Controller controller) {
        this.controller = controller;
        this.worldGrid = worldGrid;
        palaceTile = worldGrid.GetTileAt(new Int2D(
            worldGrid.gridSize.x / 2 - 1, worldGrid.gridSize.z / 2 - 1));
        GeneratePalace();
        GenerateStoneDeposits();
        GenerateTrees();
    }

    public Vector3 GetPalaceLocation() {
        return palaceTile.transform.position;
    }

    void GeneratePalace() {
        palace = Instantiate(palacePrefab);
        controller.buildings.Add(palace);
        palace.GetComponent<Placeable>().Place(worldGrid, palaceTile);
    }

    Tile NewOrigin(List<Vector3> usedPoints, float minDist) {
        int tries = 0;
        while (tries < 500) {
            tries += 1;
            Tile tile = worldGrid.RandomTile();
            Vector3 tile_pos = tile.transform.position;
            if (usedPoints.Any(ele => Vector3.Distance(ele, tile_pos) < minDist)) {
                continue;
            }
            usedPoints.Add(tile_pos);
            return tile;
        }
        return null;
    }

    void GenerateStoneDeposits() {
        int stoneDepositCount = 18;
        List<Vector3> usedPoints = new List<Vector3>();
        for (int i = 0; i < stoneDepositCount; i++) {
            Tile tile = NewOrigin(usedPoints, 40f);
            if (tile != null) {
                GameObject stoneDeposit = Instantiate(stoneDeposit1Prefab, transform);
                bool result = stoneDeposit.GetComponent<Placeable>().Place(worldGrid, tile);
                stoneDeposit.SetActive(false);
                if (!result) { i -= 1; }
            }
        }
    }

    void SpawnTreesAtOrigin(Tile tile) {
        Vector3 clusterCenter = tile.transform.position;
        float clusterRadius = Random.Range(25f, 50f);
        int numberOfTrees = 12 + Mathf.FloorToInt(clusterRadius/2);
        int tries = 0;
        while (numberOfTrees > 0 && tries < 500) {
            // Choose a random angle and distance within the cluster
            float angle = Random.Range(0f, 2f * Mathf.PI);
            float distance = clusterRadius * Mathf.Sqrt(Random.Range(0f, 1f));
            // Calculate the position of the tree
            Vector3 treePosition = clusterCenter + new Vector3(
                distance * Mathf.Cos(angle), 0, distance * Mathf.Sin(angle));
            Tile treeTile = worldGrid.GetTileAtWorldPosition(treePosition);
            // Instantiate the tree at the chosen position
            if (!treeTile.IsOccupied()
            && Vector3.Distance(palaceTile.transform.position, treePosition) > 75f)
            {
                GameObject tree = Instantiate(tree1Prefab, transform);
                tree.GetComponent<Placeable>().Place(worldGrid, treeTile);
                tree.SetActive(false);
                numberOfTrees -= 1;
            }
            tries += 1;
        }
    }

    void GenerateTrees() {
        int forestCount = 80;
        List<Vector3> usedPoints = new List<Vector3>();
        for (int i = 0; i < forestCount; i++) {
            Tile tile = NewOrigin(usedPoints, 10f);
            if (tile != null) { SpawnTreesAtOrigin(tile); }
        }
    }
}
