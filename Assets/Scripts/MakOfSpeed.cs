using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakOfSpeed : MonoBehaviour {

    public Rigidbody measuredBody;
    [SerializeField] private float speedFactor = 1.2f;

    void OnTriggerEnter() {
        measuredBody.velocity = measuredBody.velocity*speedFactor;
    }
	
}
