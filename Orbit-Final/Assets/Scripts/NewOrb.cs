using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class NewOrb : MonoBehaviour
{
    #region Publics
    public GameObject m_PointLight;
    public OVRInput.RawAxis1D m_StartAudioButton;
    #endregion

    #region Privates
    private OVRInput.Controller m_GrabbedBy = OVRInput.Controller.None;
    private DistanceGrabbable m_DistanceGrabbable;
    private Light m_Light;
    private ParticleSystem m_ParticleSystem;
    private ParticleSystem.MainModule m_ParticlesMain;
    private AudioSource m_AudioSource;
    private bool vibrationStarted = false;
    private int minFreq, maxFreq;
    private bool isRecording = false;

    private Game m_Game;
    #endregion

    private void Awake() {
        m_DistanceGrabbable = this.GetComponent<DistanceGrabbable>();
        m_Light = m_PointLight.GetComponent<Light>();
        m_ParticleSystem = this.GetComponent<ParticleSystem>();
        m_ParticlesMain = m_ParticleSystem.main;
        m_AudioSource = this.GetComponent<AudioSource>();

        m_Game = GameObject.Find("OVRPlayerController").GetComponent<Game>();
    }

    private void Update() {

        // check if currently grabbed - if not grabbed, return and set color to default yellow
        CheckGrabbed();
        if (m_GrabbedBy == OVRInput.Controller.None) {
            SetColor(Color.white);
            return;
        }

        // Set color to something else to indicate being grabbed - if recording, blue, otherwise, yellow
        if (isRecording) {
            SetColor(Color.blue);
        } else {
            SetColor(Color.white);
        }
        // If vibrations haven't started, start them
        if (!vibrationStarted) {
            vibrationStarted = true;
            StartCoroutine(SetVibration());
        }

        // Check mic status - if no mic attached, end here
        if (!m_Game.GetMicStatus()) {
            return;
        }
        // If mice is attached, update frequencies
        minFreq = m_Game.GetMinFrequency();
        maxFreq = m_Game.GetMinFrequency();

        // If a downpress on the trigger was detected by the controller holding the star, and we're not recording, initialize recording
        if (OVRInput.Get(m_StartAudioButton, m_GrabbedBy) > 0.1f && !isRecording) {
            // Check if we can even start recording - check with Game if the other hand is recording
            KeyValuePair<OVRInput.Controller, bool> checkIfRecording = m_Game.CanRecord();
            if (checkIfRecording.Key == OVRInput.Controller.None) {
                // we've got the go-ahead, start recording
                StartRecording();
            }
            
        }
        // If a letting go of the trigger was detected by the controller holding the star, and we're currently recording, stop recording
        if (OVRInput.Get(m_StartAudioButton, m_GrabbedBy) <= 0.1f && isRecording) {
            EndRecording();
        }
    }

    private void CheckGrabbed() {
        if (m_DistanceGrabbable.grabbedBy) {
            m_GrabbedBy = m_DistanceGrabbable.grabbedBy.GetController();
        }
        else {
            m_GrabbedBy = OVRInput.Controller.None;
        }
    }

    private IEnumerator SetVibration() {
        VibrationManager.singleton.TriggerVibration(5000,2,255,m_GrabbedBy);
        yield return new WaitForSeconds(5);
        vibrationStarted = false;
    }

    private void StartRecording() {
        isRecording = true;
        m_Game.SetControllerStatus(m_GrabbedBy, true);
        m_AudioSource.clip = Microphone.Start(null, true, 20, maxFreq);
    }

    private void EndRecording() {
        isRecording = false;
        m_Game.SetControllerStatus(m_GrabbedBy, false);
        Microphone.End(null); //Stop the audio recording
		m_AudioSource.Play(); //Playback the recorded audio
    }

    public void SetColor(Color c) {
        m_Light.color = c;
        m_ParticlesMain.startColor = c;
    }
}
