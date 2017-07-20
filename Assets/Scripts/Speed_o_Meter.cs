using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speed_o_Meter : MonoBehaviour {

    public Rigidbody measuredBody;
    [SerializeField] Image needle;
    [SerializeField] Text speedText;
    [SerializeField] SpeedUnits speedUnits;

	void FixedUpdate () {
	    if (!measuredBody)
	        return;
        var speed = measuredBody.velocity.magnitude;
	    switch (speedUnits) {
            case SpeedUnits.MetersPerSecond:
                speedText.text = speed.ToString("F2") + "m/s";
                break;
            case SpeedUnits.KilometersPerHour:
                speed = convertSpeedToKph(speed);
                speedText.text = speed.ToString("F2") + "km/h";
                break;
            case SpeedUnits.MilesPerHour:
                speed = convertSpeedToMph(speed);
                speedText.text = speed.ToString("F2") + "mph";
                break;

        }
	   
        var newRotation = Quaternion.Euler(0, 0, Mathf.Clamp(-speed*2+120, -120, 120));
	    needle.rectTransform.rotation = Quaternion.RotateTowards(needle.rectTransform.rotation, newRotation, 20);
	}
    
    public static float convertSpeedToKph(float speedInMPS) {
        return speedInMPS * 3.6f;
    }

    public static float convertSpeedToMph(float speedInMPS) {
        return speedInMPS * 2.237f;
    }

    enum SpeedUnits {
        MetersPerSecond, KilometersPerHour, MilesPerHour
    }
}
