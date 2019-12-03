using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public bool DebugToggle;
    public Transform returnPosition;
    private LineRenderer m_LineRenderer;
    private Vector3[] positions = new Vector3[2];
    private bool isOn = false;

    private int layerMask = 8;
    private RaycastHit hit;
    private GameObject hitObj;

    public GameObject TestRaycast;

    private void Awake() {
        m_LineRenderer = this.GetComponent<LineRenderer>();
    }

    private void Update() {

        PerformRaycast();

        if (!isOn) return;
        Vector3 curPos = this.transform.position;
        positions[0] = curPos;
        positions[1] = returnPosition.position;
        m_LineRenderer.SetPositions(positions);
    }

    private void PerformRaycast() {
         Vector3 fromPosition = returnPosition.transform.position;
         Vector3 toPosition = this.transform.position;
         Vector3 direction = toPosition - fromPosition;
        if (Physics.Raycast(returnPosition.position, direction, out hit, layerMask)) {
            if (DebugToggle) TestRaycast.SetActive(false);
            hitObj = hit.collider.gameObject;
        } else {
            if (DebugToggle) TestRaycast.SetActive(true);
            hitObj = null;
        }
    }

    public void TurnOn() {
        isOn = true;
        m_LineRenderer.enabled = true;
    }
    public void TurnOff() {
        isOn = false;
        m_LineRenderer.enabled = false;
    }
    public bool GetStatus() {
        return isOn;
    }
    public GameObject GetHit() {
        return hitObj;
    }
}
