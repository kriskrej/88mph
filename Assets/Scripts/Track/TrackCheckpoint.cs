using UnityEngine;

public class TrackCheckpoint : MonoBehaviour {
    [SerializeField] int checkpointNumber;

    TrackController trackController;

    void Awake() {
        trackController = FindObjectOfType<TrackController>();
    }

    void OnTriggerEnter(Collider other) {
        if (ColliderIsPlayer(other))
            trackController.ReportEntrance(checkpointNumber);
    }

    bool ColliderIsPlayer(Collider other) {
        return other.CompareTag("Player");
    }
}