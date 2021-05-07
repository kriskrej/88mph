using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LoopAntigravity : MonoBehaviour {
    [SerializeField] Transform loopCenter;
    [SerializeField] float loopGravity = 10f;
    [SerializeField] float worldGravity = 2f;
    [SerializeField] float accelerationBonusFactor = 2f;

    Rigidbody car;
    bool applyForce;

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
        AddForce(car.transform.forward * accelerationBonusFactor);
    }

    private void AddWorldGravity() {
        AddForce(Vector3.down * worldGravity);
    }

    private void AddLoopGravity() {
        var directionY = car.transform.position.y - loopCenter.position.y;
        var directionZ = car.transform.position.z - loopCenter.position.z;
        AddForce(new Vector3(0f, directionY, directionZ).normalized * loopGravity);
    }

    void AddForce(Vector3 force) {
        car.AddForce(force, ForceMode.Acceleration);
    }

    bool ColliderIsPlayer(Collider other) {
        return other.CompareTag("Player");
    }
}