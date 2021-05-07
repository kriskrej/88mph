using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakOfSpeed : MonoBehaviour {

    [SerializeField] private float speedFactor = 1.2f;

    void OnTriggerEnter(Collider c) {
        var rb = c.attachedRigidbody;
        if (rb.CompareTag("Player")) {
            rb.velocity = rb.velocity * speedFactor;
        }
    }
	
}
