using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent (typeof (AudioSource))]  

public class Orb : MonoBehaviour
{
    private SceneController SC;

    private OVRGrabbable grabbableRef;
    private Renderer rendererRef;
    public GameObject buttonCanvas;
    public Button recordButton;
    
    private bool recording;
    private AudioSource aud;
    //private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        SC = GameObject.Find("SceneController").GetComponent<SceneController>();
        grabbableRef = this.GetComponent<OVRGrabbable>();
        rendererRef = this.GetComponent<Renderer>();
        aud = this.GetComponent<AudioSource>();
        recording = false;
        //coroutine = GetMicCount();
        //StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbableRef.isGrabbed) {
            SetColor(Color.green);
            aud.mute = false;
            buttonCanvas.SetActive(true);
        }
        else {
            EndRecording();
            SetColor(Color.yellow);
            aud.mute = true;
            buttonCanvas.SetActive(false);
        }
    }
    /*
    private IEnumerator GetMicCount() {
        while(true) {

            yield return new WaitForSeconds(0.1f);
        }
    }
    */

    public void SetColor(Color c) {
        rendererRef.material.SetColor("_Color", c);
    }
    public void ToggleRecording() {
        if (!recording) {
            StartRecording();
        } else {
            EndRecording();
        }
    }
    public void StartRecording() {
        SetColor(Color.blue);
        recording = true;
    }
    public void EndRecording() {
        SetColor(Color.green);
        recording = false;
    }
}
