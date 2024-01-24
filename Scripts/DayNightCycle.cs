using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    Vector3 rotation;
    [SerializeField]
    float speed;
    [SerializeField]
    Vector2 rotationDelta;

    void Update() {
        rotation += new Vector3(rotationDelta.x, rotationDelta.y, 0) * speed * Time.deltaTime;
        if (rotation.x < 0) { rotation.x = 360 + rotation.x % 360; }
        else if (rotation.x >= 360) { rotation.x = rotation.x % 360; }
        if (rotation.y < 0) { rotation.y = 360 + rotation.y % 360; }
        else if (rotation.y >= 360) { rotation.y = rotation.y % 360; }
        transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
    }
}
