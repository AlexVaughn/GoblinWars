using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile : MonoBehaviour
{
    Int2D gridLocation;
    Placeable placeable;

    public void SetGridLocation(Int2D gridLocation) {
        this.gridLocation = gridLocation;
    }

    public void SetLocation(float x, float z) {
        transform.position = new Vector3(x, 0.01f, z);
    }

    public Int2D GetGridLocation() {
        return gridLocation;
    }

    public void SetPlaceable(Placeable placeable) {
        this.placeable = placeable;
    }

    public Placeable GetPlaceable() {
        return placeable;
    }

    public void SetDarkness(bool enable) {
        GetComponent<MeshRenderer>().enabled = enable;
    }

    public bool HasDarkness() {
        return GetComponent<MeshRenderer>().enabled;
    }

    public bool IsOccupied() {
        return placeable != null;
    }
}