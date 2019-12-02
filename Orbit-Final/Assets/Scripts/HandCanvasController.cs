using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCanvasController : MonoBehaviour
{
    //public bool isHeld;
    //public GameObject m_HeldUI;

    public GameObject DefaultRecordText;
    public Slider RecordSliderIU;
    public GameObject RecordingText;

    /*
    public GameObject DefaultDeleteText;
    public Slider DeleteSliderUI;
    public GameObject DeletingText;
    */

    private float TimeThreshold;

    private void Update() {
        /*
        if (isHeld) {
            m_HeldUI.SetActive(true);
        }
        else {
            m_HeldUI.SetActive(false);
        }
        */
    }

    public void InitializeRecordSlider(float threshold) {
        TimeThreshold = threshold;
        RecordSliderIU.maxValue = threshold * 100f;
    }
    /*
    public void InitializeDeleteSlider(float threshold) {
        TimeThreshold = threshold;
        DeleteSliderUI.maxValue = threshold * 100f;
    }
    */

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
        RecordSliderIU.value = 0;
    }
    /*
    public void StartDeleting() {
        DeleteSliderUI.gameObject.SetActive(true);
        DeleteSliderUI.value = 0;
    }
    public void SetDeleteSlider(float curTime) {
        DeleteSliderUI.value = (curTime < TimeThreshold) ? curTime * 100f : TimeThreshold*100f;
        if (curTime >= TimeThreshold) DeletingText.SetActive(true);
    }
    public void DeactivateDeleteSlider() {
        DeleteSliderUI.gameObject.SetActive(false);
    }
    */
}
