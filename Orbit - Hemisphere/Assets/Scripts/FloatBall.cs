using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloatBall : MonoBehaviour
{
    //public GameObject Environment;
    public Camera maincam; 
    public GameObject sphere;
    public GameObject pointLight; 
    public float VelocityDecrease;
    public float shrinkSmooth;
    //public float shrinkLight; 
    private Light lt;
    private float originalRange;
    public Vector3 C = Vector3.zero; // Sphere center
    public float R; // SPHERE RADIUS 20
    private bool hitBoundary = false;
    private bool NotDoneYet = true;
    float OriginalDist;
    public float smoothTime = 0.5f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        lt = pointLight.GetComponent<Light>();
        originalRange = lt.range;
        Debug.Log("first pos" + sphere.transform.position);
        OriginalDist = Vector3.Distance(maincam.transform.position, sphere.transform.position);
        //Vector3 rand = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
        sphere.GetComponent<Rigidbody>().velocity = new Vector3(0, 2, 0);  //RANDOMIZE VELOCITY
        //sphere.GetComponent<Rigidbody>().velocity = rand;

    }

    void Update()
    {
        checkBoundary(sphere);
        if (!hitBoundary)
        {
            //Debug.Log("moving");
            sphere.GetComponent<Rigidbody>().velocity = sphere.GetComponent<Rigidbody>().velocity * VelocityDecrease;
            sphere.transform.localScale = Vector3.Lerp(sphere.transform.localScale, Vector3.one*0.1f, shrinkSmooth * Time.deltaTime);
            float shrinkLight = (Vector3.Distance(maincam.transform.position, sphere.transform.position) - OriginalDist) /(OriginalDist);
            if (shrinkLight < 1)
            {
                lt.range = originalRange;
            }
            if(shrinkLight > 1 && Vector3.Distance(C, sphere.transform.position) > R / 2)
            {
                lt.range = originalRange/shrinkLight;
            }
            //lt.range = originalRange * 1/shrinkLight;
            if (((Vector3.Distance(C, sphere.transform.position) > R/2))&& NotDoneYet)
            {
                Vector3 intial_Campos = maincam.transform.position;
                Quaternion intial_Camrot = maincam.transform.rotation;
                StartCoroutine(FollowOrb(sphere.transform.position, intial_Campos, intial_Camrot));
                NotDoneYet = false;
                Debug.Log("should only run once");
            };
               
        }
       
    }

    void checkBoundary(GameObject sphere)
    {
        Vector3 vel = sphere.GetComponent<Rigidbody>().velocity;

        if (((Vector3.Distance(C, sphere.transform.position) > R))||(sphere.transform.position.y <0))
        {
            Debug.Log("hitBoundary!");
            Debug.Log("CHECK1 cur pos:" + sphere.transform.position);
            hitBoundary = true;
            StartCoroutine(MakeBounce(sphere, vel, 0.05f, 0.2f, 3f, false, false));
        }
    }

    private IEnumerator FollowOrb(Vector3 curPos, Vector3 camPos, Quaternion camRot)
    {
        //Vector3 offset = maincam.transform.position - sphere.transform.position;
        Vector3 offset = new Vector3(0, 0, 7);
        if (maincam.transform.position != curPos + offset)
        {
            maincam.transform.position = curPos + offset;
            maincam.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        //Debug.Log("Cam " + maincam.transform.position);
        yield return new WaitForSeconds(2);
       // Debug.Log("Done");
        maincam.transform.position = camPos;
        //maincam.transform.rotation = camRot;
        //Debug.Log("CamFinal " + maincam.transform.position);
        float elapsedTime = 0;
        while (elapsedTime < 500000)
        {
            //maincam.transform.position = Vector3.Lerp(curPos + offset, camPos, elapsedTime);
            maincam.transform.rotation= Quaternion.Lerp(Quaternion.Euler(0, 180, 0), camRot, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    private IEnumerator MakeBounce(GameObject sphere, Vector3 vel, float time1, float time2, float time3, bool called1, bool called2)
    {
        
        float elapsedTime = 0;
        if (!called1)
        {
            while (elapsedTime < time1)
            {
                //transform.position = Vector3.Lerp(startingPos, newPosition, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                sphere.GetComponent<Rigidbody>().velocity = -2f * vel;
                yield return null;
            }
            Debug.Log("loop1 dur: " + elapsedTime);
            called1 = true;
        }
        //sphere.GetComponent<Rigidbody>().velocity = Vector3.SmoothDamp(-0.5f * vel, Vector3.zero, ref velocity, smoothTime);
         if (!called2)
        {
            while (elapsedTime < time2+time1)
            {
                sphere.GetComponent<Rigidbody>().velocity = Vector3.Lerp(-2f * vel, -0.5f * vel, (elapsedTime / time2));
              
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            called2 = true;
            Debug.Log("loop2 speed: " + sphere.GetComponent<Rigidbody>().velocity + "pos " + sphere.transform.position);
        }
       while (elapsedTime < time3+time2+time1)
        {
            sphere.GetComponent<Rigidbody>().velocity = Vector3.Lerp(-0.5f * vel, Vector3.zero, (elapsedTime / time2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("loop3 speed: " + sphere.GetComponent<Rigidbody>().velocity + "pos " + sphere.transform.position);
        yield return null; 
    }

}