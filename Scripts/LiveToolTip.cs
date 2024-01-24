using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LiveToolTip : MonoBehaviour
{
    Canvas canvas;
    [SerializeField]
    TextMeshProUGUI text;

    void Start() {
        canvas = FindAnyObjectByType<Canvas>();
        MoveToMouse();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Destroy(this.gameObject);
        }
        MoveToMouse();
    }

    void MoveToMouse() {
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out movePos
        );
        movePos = new Vector2(movePos.x + 100, movePos.y + 25);
        transform.position = canvas.transform.TransformPoint(movePos);
    }

    public void SetText(string text) {
        this.text.text = text;
    }
}
