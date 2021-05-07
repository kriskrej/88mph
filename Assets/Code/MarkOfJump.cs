using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkOfJump : MonoBehaviour {

    [SerializeField] private float jumpFactor = 1.2f;

    void OnTriggerEnter(Collider c) {
        var rb = c.attachedRigidbody;
        if (rb.CompareTag("Player")) {
            rb.velocity = rb.velocity + new Vector3(0,jumpFactor,0);
        }
    }
	
}
