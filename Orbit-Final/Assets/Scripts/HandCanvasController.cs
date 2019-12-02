using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCanvasController : MonoBehaviour
{
    public GameObject DefaultRecordText;
    public Slider RecordSliderIU;
    public GameObject RecordingText;

    private float TimeThreshold;

    public void InitializeRecordSlider(float threshold) {
        TimeThreshold = threshold;
        RecordSliderIU.maxValue = threshold * 100f;
    }

    public void StartRecordSlider() {
        RecordSliderIU.gameObject.SetActive(true);
        RecordSliderIU.value = 0;
    }
    public void SetRecordSlider(float curTime) {
        RecordSliderIU.value = (curTime < TimeThreshold) ? curTime*100f : TimeThreshold*100f;
        if (curTime >= TimeThreshold) RecordingText.SetActive(true);
    }
    public void DeactivateRecordSlider() {
        RecordSliderIU.gameObject.SetActive(false);
        RecordingText.SetActive(false);
    }
}
