using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {

    [SerializeField] GameObject speedometer;
    [SerializeField] GameObject movieBars;

    public void EnableMovieMode(bool enabled) {
        speedometer.SetActive(!enabled);
        movieBars.SetActive(enabled);
    }
}
