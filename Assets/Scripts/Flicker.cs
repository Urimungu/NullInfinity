using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    float delay;
    public float minTime = 0.3f, maxTime = 10f;
    public float rapidMinTime = 0.05f, rapidMaxTime = 1f;
    public int flickerTimesMin = 3, flickerTimesMax = 10;


    private void FixedUpdate() {
        if (Time.time > delay) {
            StartCoroutine(FlickerEvent(Random.Range(flickerTimesMin, flickerTimesMax)));
            delay = Time.time + Random.Range(minTime, maxTime);
        }
    }

    IEnumerator FlickerEvent(int flickerTimes) {
        for (int i = 0; i < flickerTimes; i++) {
            yield return new WaitForSeconds(Random.Range(rapidMinTime, rapidMaxTime));
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        }
    }
}
