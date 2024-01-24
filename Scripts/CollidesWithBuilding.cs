using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pather))]
public class CollidesWithBuilding : MonoBehaviour
{
    Pather pather;
    [HideInInspector]
    public Building building;

    void Start() {
        pather = GetComponent<Pather>();
    }

    void OnTriggerEnter(Collider col) {
        Building building = col.gameObject.GetComponent<Building>();
        if (building != null) {
            pather.allowMove = false;
            this.building = building;
        }
    }
}
