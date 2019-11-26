using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUpText : MonoBehaviour
{
    public float floatUpDuration;
    public float floatUpPositionY;
    public float keepAliveDuration;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float totalDuration;
    private float timer;

    // Start is called before the first frame update
    void Start() {
        startPosition = transform.position;
        targetPosition = new Vector2(
            startPosition.x, startPosition.y + floatUpPositionY);
        totalDuration = floatUpDuration + keepAliveDuration;
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;
        if (timer >= totalDuration) {
            Destroy(gameObject);
        } else if (timer <= floatUpDuration) {
            float time = Mathf.Min(1, timer / floatUpDuration);
            transform.position = Vector2.Lerp(startPosition, targetPosition, time);
        }
    }
}
