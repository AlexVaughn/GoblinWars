using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Placeable))]
[RequireComponent(typeof(Targetable))]
public class Building : Die
{
    void Start() {
        GetComponent<Targetable>().Init(this);
    }

    public override void DoDie() {
        gameObject.SetActive(false);
    }
}