using UnityEngine;

public class MarkOfSpeed : MonoBehaviour {

    [SerializeField] private float speedFactor = 1.2f;

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player"))
            return;
        other.attachedRigidbody.velocity *= speedFactor;
        //measuredBody.velocity = measuredBody.velocity*speedFactor;
    }
	
}
