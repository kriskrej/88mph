using UnityEngine;
using UnityEngine.UI;

public class TrackController : MonoBehaviour {
    [SerializeField] GameObject start;
    [SerializeField] GameObject gate;
    [SerializeField] Text statusText;
    [SerializeField] int numberOfCheckpoints;
    [SerializeField] int numberOfLaps = 4;

    int currentCheckpoint;
    int currentLap;
    float startTime;

    void Awake() {
        currentCheckpoint = 0;
        currentLap = 0;
    }

    void Start() {
        Invoke("StartRace", 3f);
        statusText.text = "";
    }

    public void ReportEntrance(int checkpointNumber) {
        if (EnteredRightCheckpoint(checkpointNumber))
            SwitchToNextCheckpoint();
    }

    void SwitchToNextCheckpoint() {
        currentCheckpoint += 1;
        if (LapIsFinished())
            SwitchToNextLap();
    }

    void SwitchToNextLap() {
        currentLap += 1;
        currentCheckpoint = 0;
        if (RaceIsFinished())
            FinishRace();
        else
            statusText.text = string.Format("Lap {0}", currentLap + 1);
    }

    void FinishRace() {
        statusText.text = string.Format("Time: {0:0.00}", Time.time - startTime);
        gate.SetActive(false);
    }

    bool RaceIsFinished() {
        return currentLap == numberOfLaps;
    }

    bool LapIsFinished() {
        return currentCheckpoint == numberOfCheckpoints;
    }

    bool EnteredRightCheckpoint(int checkpointNumber) {
        return checkpointNumber == currentCheckpoint;
    }

    void StartRace() {
        start.SetActive(false);
        startTime = Time.time;
        statusText.text = "Lap 1";
    }
}