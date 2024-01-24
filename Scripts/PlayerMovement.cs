using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed;
    CharacterController charController;

    void Start() {
        charController = GetComponent<CharacterController>();
    }

    void FixedUpdate() {
        Vector3 move = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) {
            move += new Vector3(1.4142f, 0, 1.4142f);
        }
        if (Input.GetKey(KeyCode.S)) {
            move += new Vector3(-1.4142f, 0, -1.4142f);
        }
        if (Input.GetKey(KeyCode.A)) {
            move += new Vector3(-1, 0, 1);
        }
        if (Input.GetKey(KeyCode.D)) {
            move += new Vector3(1, 0, -1);
        }
        charController.Move(move * speed * Time.deltaTime);
    }
}