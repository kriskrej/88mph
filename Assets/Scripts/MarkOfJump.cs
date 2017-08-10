using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkOfJump : MonoBehaviour {

    public Rigidbody measuredBody;
    [SerializeField] private float jumpFactor = 1.2f;

    void OnTriggerEnter() {
        measuredBody.velocity = measuredBody.velocity + new Vector3(0,jumpFactor,0);
    }
	
}
