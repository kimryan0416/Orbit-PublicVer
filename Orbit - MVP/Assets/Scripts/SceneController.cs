   
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    private bool micConnected = false;      //A boolean that flags whether there's a connected microphone 
    private int minFreq, maxFreq;    //The minimum and maximum available recording frequencies  

    // Start is called before the first frame update
    void Start()
    {
        /*
        //Check if there is at least one microphone connected  
        //Throw a warning message at the console if there isn't 
        if(Microphone.devices.Length <= 0)  Debug.LogWarning("Microphone not connected!");
        
        //At least one microphone is present 
        else {  
            micConnected = true;    //Set 'micConnected' to true    
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);   //Get the default microphone recording capabilities 
            
            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...  
            //...meaning 44100 Hz can be used as the recording sampling rate 
            if(minFreq == 0 && maxFreq == 0)    maxFreq = 44100; 
        }  
        */
    }

    // Update is called once per frame
    void Update()
    {
        micConnected = Microphone.devices.Length > 0;
        if (micConnected) {
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);   //Get the default microphone recording capabilities 
            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...  
            //...meaning 44100 Hz can be used as the recording sampling rate 
            if(minFreq == 0 && maxFreq == 0)    maxFreq = 44100; 
        }
    }

    public (bool, int, int) GetMicDetails() {
        return (micConnected, minFreq, maxFreq);
    }
}
