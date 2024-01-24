using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseMessage : MonoBehaviour
{
    Canvas canvas;
    TextMeshProUGUI text;

    void Start() {
        canvas = FindAnyObjectByType<Canvas>();
        text = GetComponent<TextMeshProUGUI>();
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out movePos
        );
        transform.position = canvas.transform.TransformPoint(movePos);
        StartCoroutine(MoveUpAndDestroy());
    }

    IEnumerator MoveUpAndDestroy() {
        float startTime = Time.time;
        while (Time.time - startTime < 1.25f) {
            Color color = text.color;
            color.a -= 1f * Time.deltaTime;
            text.color = color;
            transform.position += new Vector3(2, 20, 0) * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}