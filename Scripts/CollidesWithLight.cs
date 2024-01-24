using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidesWithLight : MonoBehaviour
{
    [HideInInspector]
    public bool hasCollided = false;

    void OnTriggerExit(Collider col) {
        if (!hasCollided) {
            Tile tile = col.gameObject.GetComponent<Tile>();
            if (tile != null && !tile.HasDarkness()) {
                hasCollided = true;
            }
        }
    }
}
