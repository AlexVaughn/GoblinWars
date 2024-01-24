using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDropOff : MonoBehaviour
{
    public ResourceType resourceType;
    Bank bank;

    void Start() {
        bank = GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>().bank;
    }

    public void Receive(ResourceType resourceType, int count) {
        bank.AddResource(resourceType, count);
    }
}
