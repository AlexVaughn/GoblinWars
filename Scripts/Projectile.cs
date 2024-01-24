using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    System.Action onArrival;
    Vector3 targetPosition;
    [SerializeField]
    float moveSpeed;

    public void MoveToPosition(Vector3 targetPosition, System.Action onArrival) {
        this.onArrival = onArrival;
        this.targetPosition = targetPosition;
        StartCoroutine(Move());
    }

    IEnumerator Move() {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) {
            transform.position = Vector3.MoveTowards(transform.position,
                targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        onArrival();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
