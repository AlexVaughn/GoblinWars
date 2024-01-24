using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChild : MonoBehaviour
{
    [SerializeField]
    LightEmitter lightEmitter;

    public void RemoveDarkness() {
        lightEmitter.RemoveDarknessInRadius();
    }
}
