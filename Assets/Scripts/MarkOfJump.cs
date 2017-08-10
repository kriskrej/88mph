using UnityEngine;

public class MarkOfJump : MonoBehaviour {

    [SerializeField] private float jumpFactor = 1.2f;

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player"))
            return;
        other.attachedRigidbody.velocity += new Vector3(0, jumpFactor, 0);
        //measuredBody.velocity = measuredBody.velocity + new Vector3(0,jumpFactor,0);
    }
	
}
