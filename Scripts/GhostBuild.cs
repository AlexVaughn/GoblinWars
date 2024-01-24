using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GhostBuild : MonoBehaviour
{
    GameObject ghostBuild;
    Product product;
    Controller controller;
    Placeable placeable;
    GuiManager manager;

    void Update() {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Tile tile = hit.transform.GetComponent<Tile>();
                if (tile != null) {
                    transform.position = placeable.GetWorldTilePlace(controller.worldGrid, tile);
                    Place(tile);
                }
                // Right click to cancel building
                if (Input.GetMouseButtonDown(1)) {
                    manager.SetCancelBuildVisibility(false);
                    Destroy(gameObject);
                }
            }
        }
    }

    public void SetGhost(GameObject newGhostPrefab, Product product,
        GuiManager manager, Controller controller)
    {
        this.product = product;
        this.manager = manager;
        this.controller = controller;
        ghostBuild = Instantiate(newGhostPrefab, transform);
        placeable = ghostBuild.GetComponent<Placeable>();
        ghostBuild.GetComponent<BoxCollider>().enabled = false;
    }

    void Place(Tile tile) {
        if (Input.GetMouseButtonDown(0)) {
            List<Tile> tiles = controller.worldGrid.GetTilesFromTile(tile, placeable.GetSize());
            if (controller.worldGrid.AllBuildable(tiles)) {
                if (controller.bank.Buy(product)) {
                    ghostBuild.GetComponent<BoxCollider>().enabled = true;
                    ghostBuild.transform.SetParent(transform.parent);
                    placeable.SetTiles(tiles);
                    manager.SetCancelBuildVisibility(false);
                    LightChild lightChild = ghostBuild.GetComponent<LightChild>();
                    if (lightChild != null) { lightChild.RemoveDarkness(); }
                    else { controller.buildings.Add(ghostBuild); }
                    Destroy(gameObject);
                }
                else {
                    manager.CreateMouseMessage("Not enough resources!");
                }
            }
            else {
                manager.CreateMouseMessage("Cannot place there!");
            }
        }
    }
}
