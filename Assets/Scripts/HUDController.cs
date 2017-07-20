using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {

    public Speed_o_Meter speedometer;
    [SerializeField] GameObject movieBars;

    public void EnableMovieMode(bool enabled) {
        speedometer.gameObject.SetActive(!enabled);
        movieBars.SetActive(enabled);
    }
}
