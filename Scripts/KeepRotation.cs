using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepRotation : MonoBehaviour
{
    void Start() {
    // Set rotation using Euler angles
    gameObject.transform.eulerAngles = new Vector3(0f, 45f, 90f);

    // // Or set rotation using a Quaternion
    // gameObject.transform.rotation = Quaternion.Euler(x, y, z);
    }
}
