using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEmitter : MonoBehaviour
{
    [SerializeField]
    float radius;

    void Start() {
        RemoveDarknessInRadius();
    }

    public void RemoveDarknessInRadius() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders) {
            Tile tile = collider.GetComponent<Tile>();
            if (tile != null) {
                tile.SetDarkness(false);
                if (tile.IsOccupied()) {
                    Placeable placeable = tile.GetPlaceable();
                    if (placeable.isHidden) { placeable.Reveal(); }
                }
            }
        }
    }
}
