using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OculusSampleFramework;

public class Custom_Controller : MonoBehaviour
{
    public Game m_Game;
    public OVRInput.RawAxis1D RecordAndPointButton;
    public OVRInput.RawButton PlayButton;
    public OVRInput.RawButton CreateDeleteButton;

    public HandCanvasController m_HandCanvasController;

    public float HoldTimeThreshold;

    private bool RecordAndPoint_TimeStarted = false;
    private float RecordAndPoint_StartTime = 0.0f;

    private OVRInput.Controller m_Controller;
    private DistanceGrabber m_DistanceGrabber;
    private DistanceGrabbable HeldObject;
    private NewOrb HeldObject_NewOrb;

    private void Awake()
    {
        m_DistanceGrabber = this.GetComponent<DistanceGrabber>();
        m_Controller = m_DistanceGrabber.GetController();
        m_HandCanvasController.InitializeRecordSlider(HoldTimeThreshold);
    }

    // Update is called once per frame
    private void Update()
    {
        // Check mic status - if no mic attached, end here
        if (!m_Game.GetMicStatus()) {
            return;
        }

        // Check if the hand is currently holding something
        // This is entirely controlled by DistanceGrabber - we won't do the work for that
        CheckHolding();

        // Depending on if it's holding something, the behavior of the hand changes
        if (HeldObject != null) {

            HeldObject_NewOrb = HeldObject.GetComponent<NewOrb>();

            // If an object is being held, there are several things to do:
            // 1) If RecordAndPoint (default = Index Trigger) clicked AND held for some time, then a recording starts or begins to overwrite
            // 2) If RecordAndPoint (default = Index Trigger) is let go, then the recording stops
            // 3) If PlayButton (default = A/X button) is clicked AND if there's an audio clip inside, it is toggled on/off
            // 4) If CreateDeletetButton (default = B/Y button) is clicked and held for some time, then the star itself is deleted

            // 1)
            if (OVRInput.Get(RecordAndPointButton, m_Controller) > 0.1f) {      RecordAndPoint_Down(); }

            // 2)
            if (OVRInput.Get(RecordAndPointButton, m_Controller) <= 0.1f) {     RecordAndPoint_Up();    }

            // 3) 
            if (OVRInput.GetDown(PlayButton, m_Controller) && HeldObject_NewOrb.GetAudioClip() != null) {   HeldObject_NewOrb.ToggleAudio();   }

            // 4) - NOT IMPLEMENTED YET
        } 
        else {
            // If an object is NOT being held, then there are several things to do:
            // 1) If RecordAndPoint (default = Index Trigger) is clicked and held, a raycast is sent out for easier detection
            // 2) PlayButton doesn't do anything
            // 3) If CreateDeleteButton (default = B/Y button) is clicked and held for some time, then a new star is instantiated in front of the player.
        }
    }

    private void CheckHolding() {
        HeldObject = (DistanceGrabbable)m_DistanceGrabber.grabbedObject;
    }

    private void RecordAndPoint_Down() {
        if (!RecordAndPoint_TimeStarted) {
            RecordAndPoint_StartTime = Time.time;
            RecordAndPoint_TimeStarted = true;
            m_HandCanvasController.StartRecordSlider();
            return;
        }
        float TimeDiff = Time.time - RecordAndPoint_StartTime;
        m_HandCanvasController.SetRecordSlider(TimeDiff);
        
        if (RecordAndPoint_TimeStarted && TimeDiff >= HoldTimeThreshold) { 
            if (!HeldObject_NewOrb.CheckRecordingStatus()) {
                if (m_Game.CanRecord().Key == OVRInput.Controller.None) {
                    // we've got the go-ahead, start recording
                    HeldObject_NewOrb.StartRecording();
                }
            }
        }
    }
    private void RecordAndPoint_Up() {
        RecordAndPoint_StartTime = 0.0f;
        RecordAndPoint_TimeStarted = false;
        m_HandCanvasController.DeactivateRecordSlider();
        if (HeldObject_NewOrb.CheckRecordingStatus()) {
            HeldObject_NewOrb.EndRecording();
        }
    }
    private void ToggleAudio() {

    }
}
