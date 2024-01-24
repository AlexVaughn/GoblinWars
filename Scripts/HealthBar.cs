using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Transform cover;
    MeshRenderer bgMesh;
    MeshRenderer coverMesh;

    void Awake() {
        cover = transform.Find("HealthBar/Cover");
        coverMesh = cover.GetComponent<MeshRenderer>();
        bgMesh = transform.Find("HealthBar/Background").GetComponent<MeshRenderer>();
    }

    public void SetBar(int health, int maxHealth) {
        float new_y = Mathf.Clamp((float)health / (float)maxHealth, 0f, 1f);
        cover.localScale = new Vector3(cover.localScale.x, new_y, cover.localScale.z);
    }

    public void SetBarVisibility(bool enable) {
        bgMesh.enabled = enable;
        coverMesh.enabled = enable;
    }
}
