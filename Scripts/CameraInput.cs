using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInput : MonoBehaviour
{

    [SerializeField]
    float minScroll;
    [SerializeField]
    float maxScroll;

    void Update() {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0) {
            float size = GetComponent<Camera>().orthographicSize;
            size = Mathf.Clamp(size - scroll*10, minScroll, maxScroll);
            GetComponent<Camera>().orthographicSize = size;
        }
    }
}