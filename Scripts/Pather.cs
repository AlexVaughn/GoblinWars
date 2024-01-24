using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pather : MonoBehaviour
{
    Vector3 targetPosition;
    [SerializeField]
    float moveSpeed;
    [HideInInspector]
    public bool allowMove;
    [HideInInspector]
    public bool locationReached;

    public void StartMove(Vector3 move) {
        targetPosition = move;
        allowMove = true;
        locationReached = false;
        StartCoroutine(MoveToPosition());
    }

    public void ContinueMove() {
        allowMove = true;
        if (!locationReached) {
            StartCoroutine(MoveToPosition());
        }
    }

    public void CancelMove() {
        locationReached = false;
        allowMove = false;
        targetPosition = transform.position;
    }

    IEnumerator MoveToPosition() {
        while (allowMove && !locationReached) {
            locationReached = Vector3.Distance(transform.position, targetPosition) <= 0.01f;
            transform.position = Vector3.MoveTowards(transform.position,
                targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}