using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using ProBuilder2.Common;

[Serializable]
public enum DriveType
{
	RearWheelDrive,
	FrontWheelDrive,
	AllWheelDrive
}

public class CarController : MonoBehaviour
{
    [Tooltip("Maximum steering angle of the wheels")]
	public float maxAngle = 30f;
	[Tooltip("Maximum torque applied to the driving wheels")]
	public float maxTorque = 300f;
	[Tooltip("Maximum brake torque applied to the driving wheels")]
	public float brakeTorque = 30000f;
	[Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
	public GameObject wheelShape;

	[Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
	public float criticalSpeed = 5f;
	[Tooltip("Simulation sub-steps when the speed is above critical.")]
	public int stepsBelow = 5;
	[Tooltip("Simulation sub-steps when the speed is below critical.")]
	public int stepsAbove = 1;

	[Tooltip("The vehicle's drive type: rear-wheels drive, front-wheels drive or all-wheels drive.")]
	public DriveType driveType;

    public WheelCollider[] m_Wheels;
    bool inputDisabled;
    bool disappearing;
    Vector3 startingPosition;
    Quaternion startingRotation;

    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Transform camRig;
    [SerializeField] CameraController cameraController;
    [SerializeField] HUDController hudController;
    [SerializeField] ParticleSystem wheelFlamePrefab;
    [SerializeField] Collider collider;
    //[SerializeField] BoxCollider effectTrigger;
    List<ParticleSystem> wheelFlames = new List<ParticleSystem>();

    public Vector3 targetVelocity;

    void Awake() {
        if (cameraController == null) {
            cameraController = FindObjectOfType<CameraController>();
            cameraController.lookAtTarget = camRig.GetChild(0);
            cameraController.positionTarget = camRig.GetChild(1);
            cameraController.sideView = camRig.GetChild(2);
            cameraController.disappearingAnimationView = camRig.GetChild(3);
        }
        if (hudController == null) {
            hudController = FindObjectOfType<HUDController>();
            hudController.speedometer.measuredBody = rigidbody;
        }
    }
	void Start() {
	    startingPosition = transform.position;
        startingRotation = transform.rotation;
	}

	// This is a really simple approach to updating wheels.
	// We simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero.
	// This helps us to figure our which wheels are front ones and which are rear.
	void Update() {
	    if (inputDisabled)
	        return;

	    if (Input.GetKeyDown(KeyCode.R) || transform.position.y <= -30) {
	        RestartCar();
            cameraController.RestartCamera();
            hudController.EnableMovieMode(false);
	    }
        
	    m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);

		float angle = maxAngle * Input.GetAxis("Horizontal");
		float torque = maxTorque * Input.GetAxis("Vertical");
		float handBrake = Input.GetKey(KeyCode.X) ? brakeTorque : 0;

		UpdateWheels(angle, handBrake, torque);
	}

    void FixedUpdate() {
        if (disappearing) {
            UpdateWheels(0, 0, maxTorque);
            rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, targetVelocity, 10);
            return;
        }
        if(Speed_o_Meter.convertSpeedToMph(rigidbody.velocity.magnitude) >= 80)
            SpawnWheelFlames();
        else
            StopWheelFlames();
        if(Speed_o_Meter.convertSpeedToMph(rigidbody.velocity.magnitude) >= 88)
            Disappear();
    }

    void RestartCar() {
        if (disappearing)
            return;
        rigidbody.isKinematic = true;
        rigidbody.velocity = Vector3.zero;
        UpdateWheels(0, 30000, 0);
        transform.position = startingPosition;
        transform.rotation = startingRotation;
        rigidbody.isKinematic = false;
    }

    private void UpdateWheels(float angle, float handBrake, float torque) {
        foreach (WheelCollider wheel in m_Wheels) {
            // A simple car where front wheels steer while rear ones drive.
            if (wheel.transform.localPosition.z > 0)
                wheel.steerAngle = angle;

            if (wheel.transform.localPosition.z < 0) {
                wheel.brakeTorque = handBrake;
            }

            if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive) {
                wheel.motorTorque = torque;
            }

            if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheelDrive) {
                wheel.motorTorque = torque;
            }

            // Update visual wheels if any.
            if (wheelShape) {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                // Assume that the only child of the wheelcollider is the wheel shape.
                Transform shapeTransform = wheel.transform.GetChild(0);
                shapeTransform.position = p;
                shapeTransform.rotation = q;
            }
        }
    }

    void Disappear() {
        disappearing = true;
        hudController.EnableMovieMode(true);
        cameraController.SetCameraDisappearingAnimationPosition();
        targetVelocity = rigidbody.velocity*100;
        inputDisabled = true;
        //collider.isTrigger = true;
        //SpawnWheelFlames();
        Invoke("DestroyCar", 2);
    }

    private void SpawnWheelFlames() {
        if (wheelFlames.Any()) {
            foreach (var flame in wheelFlames)
                flame.Play();
            return;
        }
        foreach (var wheel in m_Wheels) {
                var f = Instantiate(wheelFlamePrefab, wheel.transform);
                //f.transform.position = f.transform.position - new Vector3(0, 0.5f, 0);
                wheelFlames.Add(f);
                f.Play();
        }
    }

    void StopWheelFlames() {
        if (!wheelFlames.Any())
            return;
        foreach (var wheelFlame in wheelFlames) {
            wheelFlame.Stop();
        }
    }

    void DestroyCar() {
        foreach (var flame in wheelFlames) {
            flame.transform.parent = null;
            Destroy(flame.gameObject, 10);
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other) {
        if (!disappearing)
            return;
        Debug.Log("Whoom");
        collider.enabled = false;
        CancelInvoke("DestroyCar");
        DestroyCar();
    }
}
