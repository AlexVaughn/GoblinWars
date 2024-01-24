using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pather))]
public class CollidesWithDropOff : MonoBehaviour
{
    Pather pather;
    [HideInInspector]
    public ResourceDropOff dropOff;
    [HideInInspector]
    public System.Action onCollide;

    void Start() {
        pather = GetComponent<Pather>();
    }

    void OnTriggerEnter(Collider col) {
        ResourceDropOff dropOff = col.gameObject.GetComponent<ResourceDropOff>();
        if (dropOff != null) {
            pather.allowMove = false;
            this.dropOff = dropOff;
            onCollide();
        }
    }
}
