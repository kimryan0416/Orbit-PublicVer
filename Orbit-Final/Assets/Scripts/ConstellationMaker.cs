using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConstellationMaker : MonoBehaviour
{
    private bool buttonpressed = false;
    private bool starsInPosition = false;
    public GameObject linePrefab;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public GameObject star4;
    public GameObject star5;
    public GameObject star6;
    public GameObject star7;
    public float speed;
    public float drawLine;
    private float journeyLength_1;
    private float journeyLength_2;
    private float journeyLength_3;
    private float journeyLength_4;
    private float journeyLength_5;
    private float journeyLength_6;
    private float journeyLength_7;
    //origin: (1,7,30)
    private Vector3 end1 = new Vector3(1, 12, 70);
    private Vector3 end2 = new Vector3(-2.90916f, 10.11745f, 70);
    private Vector3 end3 = new Vector3(-3.87464f, 5.8874f, 70);
    private Vector3 end4 = new Vector3(-1.16942f, 2.49516f, 70);
    private Vector3 end5 = new Vector3(3.16942f, 2.49516f, 70);
    private Vector3 end6 = new Vector3(5.48746f, 5.8874f, 70);
    private Vector3 end7 = new Vector3(4.90916f, 10.11745f, 70);

    //2:(-2.90916,10.11745,30)
    //3:(-3.87464,5.8874,30)
    //4:(-1.16942,2.49516,30)
    //5:(3.16942,2.49516,30)
    //6:(5.48746,5.8874,30)
    //7:(4.90916,10.11745,30)

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
    
    startTime = Time.time;
        // Calculate the journey length.
        journeyLength_1 = Vector3.Distance(star1.transform.position, end1);
        journeyLength_2 = Vector3.Distance(star2.transform.position, end2);
        journeyLength_3 = Vector3.Distance(star3.transform.position, end3);
        journeyLength_4 = Vector3.Distance(star4.transform.position, end4);
        journeyLength_5 = Vector3.Distance(star5.transform.position, end5);
        journeyLength_6 = Vector3.Distance(star6.transform.position, end6);
        journeyLength_7 = Vector3.Distance(star7.transform.position, end7);

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Two))
        {
            buttonpressed = true;
        }
        if (buttonpressed)
        {
            StartCoroutine(MyCoroutine());
        }
    }

    void moveStar(GameObject star, float journeylength, Vector3 end)
    {
        float distCovered = (Time.time - startTime) * speed;

        float fractionOfJourney = distCovered / journeylength;

        star.transform.position = Vector3.Lerp(star.transform.position, end, fractionOfJourney);
    }

    IEnumerator MyCoroutine()
    {
        //This is a coroutine
        moveStar(star1, journeyLength_1, end1);
        moveStar(star2, journeyLength_2, end2);
        moveStar(star3, journeyLength_3, end3);
        moveStar(star4, journeyLength_4, end4);
        moveStar(star5, journeyLength_5, end5);
        moveStar(star6, journeyLength_6, end6);
        moveStar(star7, journeyLength_7, end7);

        yield return new WaitForSeconds(5);

        GameObject lineGen = Instantiate(linePrefab);
        LineRenderer lrend = lineGen.GetComponent<LineRenderer>();
        lrend.SetPosition(0, star1.transform.position);
        lrend.SetPosition(1, star4.transform.position);
        lrend.SetPosition(2, star7.transform.position);
        lrend.SetPosition(3, star3.transform.position);
        lrend.SetPosition(4, star6.transform.position);
        lrend.SetPosition(5, star2.transform.position);
        lrend.SetPosition(6, star5.transform.position);
        lrend.SetPosition(7, star1.transform.position);
        /*LineRenderer lrend1 = lineGen.GetComponent<LineRenderer>();
        lrend1.SetPosition(0, star4.transform.position);
        lrend1.SetPosition(1, star7.transform.position);
        LineRenderer lrend2 = lineGen.GetComponent<LineRenderer>();
        lrend2.SetPosition(0, star7.transform.position);
        lrend2.SetPosition(1, star3.transform.position);
        LineRenderer lrend3 = lineGen.GetComponent<LineRenderer>();
        lrend3.SetPosition(0, star3.transform.position);
        lrend3.SetPosition(1, star6.transform.position);
        LineRenderer lrend4 = lineGen.GetComponent<LineRenderer>();
        lrend4.SetPosition(0, star6.transform.position);
        lrend4.SetPosition(1, star2.transform.position);
        LineRenderer lrend5 = lineGen.GetComponent<LineRenderer>();
        lrend5.SetPosition(0, star2.transform.position);
        lrend5.SetPosition(1, star5.transform.position);*/
    }

}
