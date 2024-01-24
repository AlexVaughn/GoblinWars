using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDeposit: MonoBehaviour
{
    public ResourceType type;
    [SerializeField]
    int resource;

    public int GetTake(int amount) {
        if (amount >= resource) {
            int actual = resource;
            resource = 0;
            StartCoroutine(DestroyThis());
            return actual;
        }
        else {
            resource -= amount;
            return amount;
        }
    }

    IEnumerator DestroyThis() {
        yield return null;
        Destroy(gameObject);
    }
}
