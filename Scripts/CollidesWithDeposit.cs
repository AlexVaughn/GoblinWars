using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pather))]
public class CollidesWithDeposit : MonoBehaviour
{
    Pather pather;
    [HideInInspector]
    public ResourceDeposit deposit;
    [HideInInspector]
    public System.Action onCollide;

    void Start() {
        pather = GetComponent<Pather>();
    }

    void OnTriggerEnter(Collider col) {
        ResourceDeposit deposit = col.gameObject.GetComponent<ResourceDeposit>();
        if (deposit != null) {
            if (pather.gameObject.GetComponent<ResourceCollector>().resourceType == deposit.type) {
                pather.allowMove = false;
                this.deposit = deposit;
                onCollide();
            }
        }
    }
}
