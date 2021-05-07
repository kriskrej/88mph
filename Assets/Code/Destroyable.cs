using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour {
    [SerializeField] List<GameObject> destroyableFragments;
    [SerializeField] float initialDurability;

    float durability;

    void Awake() {
        durability = initialDurability;
    }

    void OnCollisionEnter(Collision collision) {
        durability -= collision.impulse.magnitude;
        if (durability < 0f) {
            foreach (var fragment in destroyableFragments) {
                Destroy(fragment.GetComponent<FixedJoint>());
                fragment.GetComponent<Rigidbody>().WakeUp();    
            }
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }

    }
}
