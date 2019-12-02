using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    #region Globals
    public bool m_InputActive = true;   // PUBLIC - either enables or disables all inputs
    private bool micConnected;          // tracks if mic is connected to Oculus or not
    private int minFreq, maxFreq;       // the minimum and maximum frequency of the current microphone
    private List<GameObject> m_Stars = new List<GameObject>();  // Tracks all stars in the world
    #endregion

    #region Inputs
    // Keep track of controllers and their orbs
    private Dictionary<OVRInput.Controller, bool> m_ControllerSets = new Dictionary<OVRInput.Controller, bool>();
    // Component for audio recording - useless at the moment, but probably important to keep
    private AudioSource m_AudioSource;
    #endregion

    private void Awake() {
        // Set the tracking space to roomscale
        UnityEngine.XR.XRDevice.SetTrackingSpaceType(UnityEngine.XR.TrackingSpaceType.RoomScale);
        // get the AudioSource component
        m_AudioSource = this.GetComponent<AudioSource>();
        // Genreate controller sets
        m_ControllerSets = CreateControllerSets();
    }

    private void Update() {
        
        // Block if inputs should be disabled
        if (!m_InputActive) return;
        
        // Check the status of any microphone attached to our oculus
        CheckMicStatus();
    }

    private Dictionary<OVRInput.Controller, bool> CreateControllerSets() {
        Dictionary<OVRInput.Controller, bool> newSets = new Dictionary<OVRInput.Controller, bool>()
        {
            { OVRInput.Controller.LTouch, false },
            { OVRInput.Controller.RTouch, false }
        };
        return newSets;
    }

    private void CheckMicStatus() {
        // Check if there is any mic connected to Oculus
        micConnected = Microphone.devices.Length > 0;
        if (micConnected) {
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);   //Get the default microphone recording capabilities 
            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...  
            //...meaning 44100 Hz can be used as the recording sampling rate 
            if(minFreq == 0 && maxFreq == 0) maxFreq = 44100; 
        }
    }

    public bool GetMicStatus() {
        return micConnected;
    }
    public List<int> GetFrequencies() {
        List<int> frequencies = new List<int>();
        frequencies.Add(minFreq);
        frequencies.Add(maxFreq);
        return frequencies;
    }
    public int GetMinFrequency() {
        return minFreq;
    }
    public int GetMaxFrequency() {
        return maxFreq;
    }

    public void AddStar(GameObject star) {
        m_Stars.Add(star);
    }
    public void RemoveStar(GameObject star) {
        m_Stars.Remove(star);
    }
    public void SetControllerStatus(OVRInput.Controller controller, bool status) {
        m_ControllerSets[controller] = status;
    }

    public KeyValuePair<OVRInput.Controller, bool> CanRecord() {

        KeyValuePair<OVRInput.Controller, bool> toReturn = new KeyValuePair<OVRInput.Controller, bool>(OVRInput.Controller.None, false);

        foreach(KeyValuePair<OVRInput.Controller, bool> pair in m_ControllerSets) {
            //Use pair.Key to get the key
            //Use pair.Value for value
            if (pair.Value == true) {
                toReturn = pair;
                break;
            }
        }

        return toReturn;

        /*

        // default to false
        bool status = false;
        // attempt to check if that controller corresponds to any that exist - also output the status if such a controller exists
        bool found = m_ControllerSets.TryGetValue(check, out status);

        // send back response
        confirmation.Add(found);
        confirmation.Add(status);
        return confirmation;
        */

    }

    /*
    #region Events
    public static UnityAction OnLeftGripUp = null;
    public static UnityAction OnLeftGripDown = null;
    public static UnityAction OnLeftTriggerUp = null;
    public static UnityAction OnLeftTriggerDown = null;
    public static UnityAction OnRightGripUp = null;
    public static UnityAction OnRightGripDown = null;
    public static UnityAction OnRightTriggerUp = null;
    public static UnityAction OnRightTriggerDown = null;
    public static UnityAction<OVRInput.Controller, GameObject> OnControllerSource = null;
    #endregion

    #region Anchors
    public GameObject m_LeftAnchor, m_RightAnchor;
    #endregion

    #region Input
    private Dictionary<OVRInput.Controller, GameObject> m_ControllerSets = null;
    private OVRInput.Controller m_InputSource = OVRInput.Controller.None;
    private OVRInput.Controller m_Controller = OVRInput.Controller.None;
    private bool m_InputActive = true;
    #endregion

    private void Awake() {
        //OVRManager.HMDMounted += PlayerFound;
        //OVRManager.HMDMounted += PlayerLost;

        // We link each controller to their anchors
        m_ControllerSets = CreateControllerSets();
    }

    
    //private void OnDestroy() {
    //    OVRManager.HMDMounted -= PlayerFound;
    //    OVRManager.HMDMounted -= PlayerLost;
    //}

    // Update is called once per frame
    void Update()
    {
        // Check for active input
        if (!m_InputActive) {
            return;
        }

        // Check if controller exists
        CheckForControllers();

        // Check for input source 
        CheckInputSource();

        // Check for actual Input
        Input();

    }

    private void CheckForControllers() {
        OVRInput.Controller controllerCheck = m_Controller;

        // Left Remote
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch)) {
            controllerCheck = OVRInput.Controller.LTouch;
        }

        // Right Remote 
        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTouch)) {
            controllerCheck = OVRInput.Controller.RTouch;
        } 

         // Update
        m_Controller = UpdateSource(controllerCheck, m_Controller);
    }

    private void CheckInputSource() {
        // Update
        m_InputSource = UpdateSource(OVRInput.GetActiveController(), m_InputSource);
    }

    private void Input() {
        
        #region Right Hand Inputs
        // Right Grip Down
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger)) {
            if (OnRightGripDown != null) {
                OnRightGripDown();
            }
        }

        // Right Grip up
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger)) {
            if(OnRightGripUp != null) {
                OnRightGripUp();
            }
        }

        // Right Trigger Down
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) {
            if (OnRightTriggerDown != null) {
                OnRightTriggerDown();
            }
        }

        // Right Trigger Up
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger)) {
            if (OnRightTriggerUp != null) {
                OnRightTriggerUp();
            }
        }
        #endregion

        #region Left Hand Inputs
        // Left Grip Down
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger)) {
            if (OnLeftGripDown != null) {
                OnLeftGripDown();
            }
        }

        // Left Grip Up
        if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger)) {
            if (OnLeftGripUp != null) {
                OnLeftGripUp();
            }
        }

        // Left Trigger Down
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) {
            if (OnLeftTriggerDown != null) {
                OnLeftTriggerDown();
            }
        }

        // Left Trigger Up
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger)) {
            if (OnLeftTriggerUp != null) {
                OnLeftTriggerUp();
            }
        }
        #endregion

    }

    private OVRInput.Controller UpdateSource(OVRInput.Controller check, OVRInput.Controller previous) {
        // If values are same, return
        if (check == previous) {
            return previous;
        }
        // Get controller object
        GameObject controllerObject = null;
        m_ControllerSets.TryGetValue(check, out controllerObject); 

        // Send out event
        if (OnControllerSource != null && controllerObject != null) {
            OnControllerSource(check, controllerObject);
        }
        // return new value
        return check;
    }

    private Dictionary<OVRInput.Controller, GameObject> CreateControllerSets(){
        Dictionary<OVRInput.Controller, GameObject> newSets = new Dictionary<OVRInput.Controller, GameObject>()
        {
            { OVRInput.Controller.LTouch, m_LeftAnchor },
            { OVRInput.Controller.RTouch, m_RightAnchor}
        };
        return newSets;
    }

    */

}
