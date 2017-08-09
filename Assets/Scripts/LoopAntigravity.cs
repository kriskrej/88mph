using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LoopAntigravity : MonoBehaviour {
    [SerializeField] Transform loopCenter;
    [SerializeField] float loopGravity = 10f;
    [SerializeField] float worldGravityFactor = 2f;
    [SerializeField] float accelerationBonusFactor = 2f;

    Rigidbody car;
    bool applyForce = false;
    Vector3 force = Vector3.zero;

    void Awake() {
        car = FindObjectOfType<CarController>().GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other) {
        SetPlayerGravity(other, true);
    }

    void SetPlayerGravity(Collider other, bool loopGravity) {
        if (!ColliderIsPlayer(other))
            return;
        applyForce = loopGravity;
        car.useGravity = !loopGravity;
    }

    void OnTriggerExit(Collider other) {
        SetPlayerGravity(other, false);
    }

    void FixedUpdate() {
        ApplyForce();
    }

    void ApplyForce() {
        if (!applyForce)
            return;
        AddWorldGravity();
        AddLoopGravity();
        AddAccelerationBonus();
    }

    void AddAccelerationBonus() {
        car.AddForce((car.transform.forward + car.transform.up) * accelerationBonusFactor);
    }

    private void AddWorldGravity() {
        car.AddForce(Vector3.down * worldGravityFactor, ForceMode.Acceleration);
    }

    private void AddLoopGravity() {
        var directionY = car.transform.position.y - loopCenter.position.y;
        var directionZ = car.transform.position.z - loopCenter.position.z;
        car.AddForce(new Vector3(0f, directionY, directionZ).normalized * loopGravity, ForceMode.Acceleration);
    }

    bool ColliderIsPlayer(Collider other) {
        return other.CompareTag("Player");
    }
}