using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;

/// <summary>
/// Is used to control the drone, fetches commands from the command manager 
/// and converts them into physical influences which are applied to the allocated drone model
/// 
/// Parts are used from Erik Nordeus http://www.habrador.com/tutorials/pid-controller/3-stabilize-quadcopter/
/// </summary>
public class DroneController : MonoBehaviour {

    //The propellers
    public GameObject RotorFrontRight;
    public GameObject RotorFrontLeft;
    public GameObject RotorRearLeft;
    public GameObject RotorRearRight;

    //Quadcopter parameters
    [Header("Setupable Values")]
    private float MaxRotorForce = 99.62f; //100
    private float MaxTorque = 5f; //5
    public float Throttle;
    private float MoveFactor = 1.1f; //1.5
    private float MaxThrottle = 150.0f; // 150f
    private float ThrottleSpeedgain = 5.0f;

    //PID
    public Vector3 _pidPitchGains; //(1.9, 0.2, 0.9)
    public Vector3 _pidRollGains; //(3, 0.4, 2)
    public Vector3 _pidYawGains; //(2, 0, 0)

    // Rigidbody of the quadcopter
    Rigidbody _quadcopterRb;

    // Flags
    private bool _flagEngineRunning = true;
    private bool _flagHoldMode = true;
    private bool _flagNetworkMode = false;

   
    //The PID controllers
    public PIDController _pidPitch;
    public PIDController _pidRoll;
    public PIDController _pidYaw;

    //Movement factors
    float _pitchDirection;
    float _rollDirection;
    float _yawDirection;

    // Used to speed up or slow down the rotor animation
    private Animator _animator;

    public float TestMultiplicator;

    [Header("Network-Commands")]
    public CommandManager CmdManager;

    private float _networkcmdThrottle; // [-1,1]
    private float _networkcmdPitch; // [-1,1]
    private float _networkcmdYaw; // [-1,1]
    private float _networkcmdRoll; // [-1,1]

    // Sensor data
    private bool _ledEnabled;

    void Start()
    {
        _ledEnabled = false;

        TestMultiplicator = 3.0f;

        _quadcopterRb = gameObject.GetComponent<Rigidbody>();

        _pidPitch = new PIDController();
        _pidRoll = new PIDController();
        _pidYaw = new PIDController();

        SetupPidGains();

        _networkcmdPitch = 0f;
        _networkcmdThrottle = 0f;
        _networkcmdRoll = 0f;
        _networkcmdYaw = 0f;

        _animator = GetComponent<Animator>();
    }

    private void SetupPidGains()
    {
        Vector3 pitchGains = new Vector3(OptionsManager.GetPIDPitchP(), OptionsManager.GetPIDPitchI(), OptionsManager.GetPIDPitchD());
        SetPitchGains(pitchGains);
        
        Vector3 rollGains = new Vector3(OptionsManager.GetPIDRollP(), OptionsManager.GetPIDRollI(), OptionsManager.GetPIDRollD());
        SetRollGains(rollGains);

        Vector3 yawGains = new Vector3(OptionsManager.GetPIDYawP(), OptionsManager.GetPIDYawI(), OptionsManager.GetPIDYawD());
        SetYawGains(yawGains);
    }


    void Update()
    {
        if (_flagNetworkMode)
        {
            GetNetworkCommands();
        }

        CheckForModes();

        AdjustAnimation();
    }

    /// <summary>
    /// Speeds up or slows down the animation by the value of the throttle
    /// </summary>
    private void AdjustAnimation()
    {
        if (Throttle != 0)
        {
            float throttlePercentage = (Throttle / MaxThrottle);

            _animator.speed = 3.0f * throttlePercentage;
        }
        else
        {
            _animator.speed = 0f;
        }
    }

    /// <summary>
    /// Checks if a mode should be activated or deactivated 
    /// </summary>
    private void CheckForModes()
    {
        // Checks if the engines should be running
        if (Input.GetKeyDown(KeyCode.T) && !_flagEngineRunning)
        {
            _flagEngineRunning = true;
            Debug.Log("Engine started!");
        }
        else if (Input.GetKeyDown(KeyCode.T) && _flagEngineRunning)
        {
            _flagEngineRunning = false;
            Throttle = 0.0f;
            Debug.Log("Engine Deaktiviert!");
        }

        if (Input.GetKeyDown(KeyCode.N) && !_flagNetworkMode)
        {
            _flagNetworkMode = true;
            Debug.Log("NetworkMode activated!");
        }
        else if (Input.GetKeyDown(KeyCode.N) && _flagNetworkMode)
        {
            _flagNetworkMode = false;
            Debug.Log("NetworkMode deactivated!");
        }

        // Checks if the hold mode should be activated
        if (Input.GetKeyDown(KeyCode.H) && !_flagHoldMode)
        {
            _flagHoldMode = true;
            Debug.Log("Hold mode activated!");
        }
        else if (Input.GetKeyDown(KeyCode.H) && _flagHoldMode)
        {
            _flagHoldMode = false;
            Throttle = 0.0f;
            Debug.Log("Hold mode deactivated");
        }


    }


    void FixedUpdate()
    {
        if (_flagEngineRunning)
        {
            EvaluateControls();

            CalculateRotorForces();
        }
    }



    /// <summary>
    /// Fetches the current commands from the CommandManager 
    /// </summary>
    private void GetNetworkCommands()
    {
        _networkcmdThrottle = CmdManager.GetThrottle();
        _networkcmdPitch = CmdManager.GetPitch();
        _networkcmdRoll = CmdManager.GetRoll();
        _networkcmdYaw = CmdManager.GetYaw();
    }



    void EvaluateControls()
    {
        float rollAxis, pitchAxis, yawAxis, throttleAxis = 0f;

        bool noSteering = true;

        // Fetch values for p,r,y,t by virtual axis wether by network or manual commands
        if (!_flagNetworkMode)
        {
            rollAxis = Input.GetAxis("Horizontal");
            pitchAxis = Input.GetAxis("Vertical");
            yawAxis = Input.GetAxis("HorizontalAD");
            throttleAxis = Input.GetAxis("VerticalWS");
        }
        else 
        {
            // Fetch values for p,r,y,t by network commands
            throttleAxis = _networkcmdThrottle;
            pitchAxis = _networkcmdPitch;
            yawAxis = _networkcmdYaw;
            rollAxis = _networkcmdRoll;
        }


        //Change throttle to move up or down
        if (throttleAxis > 0)
        {
            Throttle += ThrottleSpeedgain * throttleAxis;
        }
        else if (throttleAxis < 0)
        {
            Throttle -= ThrottleSpeedgain * -throttleAxis;
        }
        else if (_flagHoldMode)
        {
            Throttle = _quadcopterRb.mass * Mathf.Abs(Physics.gravity.y) / 4;
        }


        //Steering
        //Move forward or reverse
        _pitchDirection = 0f;

        if (pitchAxis > 0)
        {
            noSteering = false;
            _pitchDirection = 1f * pitchAxis ;
        }
        if (pitchAxis < 0)
        {
            noSteering = false;
            _pitchDirection = -1f * -pitchAxis;
        }

        //Move left or right
        _rollDirection = 0f;

        if (rollAxis > 0)
        {
            noSteering = false;
            _rollDirection = rollAxis;
        }
        if (rollAxis < 0)
        {
            noSteering = false;
            _rollDirection = rollAxis;
        }


        //Rotate around the axis
        _yawDirection = 0f;

        if (yawAxis > 0)
        {
            _yawDirection = yawAxis;
        }
        if (yawAxis < 0)
        {
            _yawDirection = yawAxis;
        }

        Throttle = Mathf.Clamp(Throttle, 0f, noSteering ? MaxThrottle * 0.5f : MaxThrottle);// If the drone is just rising up or downwards the maximum throttle gets shrinked to the half
       

    }

    void CalculateRotorForces()
    {
        
        // Pitch
        // Berechne die Regelabweichung fuer das nach vorne bzw. nach hinten kippen
        float pitchError = GetPitchError();

        // Roll
        // Berechne die Regelabweichung fuer das seitliche kippen nach links und rechts
        float rollError = GetRollError() * -1f;

        // Yaw
        // Berechne die Regelabweichung fuer das drehen um die eigene Achse
        float yawError = GetYawError();

        // Bei entsprechend starken Auftriebs (Throttle ueber 100.0f), muessen die Werte zur Regelsteuerung auf das doppelte 
        // erhoeht werden, um ein Uebersteuern zu verhindern
        Vector3 PID_pitch_gains_adapted = Throttle > 100f ? _pidPitchGains * 2f : _pidPitchGains;
    

        // Erhaelt die entsprechenden Werte der Regelsteuerung, um den berechneten Regelabweichungen 
        // entgegen zu wirken
        float PID_pitch_output = _pidPitch.GetFactorFromPIDController(PID_pitch_gains_adapted, pitchError);
        float PID_roll_output = _pidRoll.GetFactorFromPIDController(_pidRollGains, rollError);
        float PID_yaw_output = _pidYaw.GetFactorFromPIDController(_pidYawGains, yawError);

        // Berechnet die Kraefte, die auf die einzelnen Rotoren ausgewirkt werden muessen
        // Rotor Vorne Rechts
        float rotorForceFrontRight = Throttle + (PID_pitch_output + PID_roll_output);

        // Berechnet die Steuerung
        // Die _pitchDirection[-1;1] laesst die Drohne im negativen Bereich nach hinten kippen und im positiven nach vorne
        // die Kraefte werden automatisch entsprechend auf die Rotoren verteilt und entsprechend der jeweiligen Position 
        // subtrahiert oder addiert
        rotorForceFrontRight -= _pitchDirection*Throttle*MoveFactor;
        rotorForceFrontRight -= _rollDirection*Throttle;

        // Rotor Vorne Links
        var rotorForceFrontLeft = Throttle + (PID_pitch_output - PID_roll_output);

        rotorForceFrontLeft -= _pitchDirection*Throttle*MoveFactor;
        rotorForceFrontLeft += _rollDirection*Throttle;

        // Rotor Hinten Rechts
        float rotorForceRearRight = Throttle + (-PID_pitch_output + PID_roll_output);

        rotorForceRearRight += _pitchDirection * Throttle * MoveFactor;
        rotorForceRearRight -= _rollDirection * Throttle;


        // Rotor Hinten links
        float rotorForceRearLeft = Throttle + (-PID_pitch_output - PID_roll_output);

        rotorForceRearLeft += _pitchDirection * Throttle * MoveFactor;
        rotorForceRearLeft += _rollDirection * Throttle;

        // Berechnet nun im Anschluss noch die Rotorkraft fuer das Drehen um die eigene Achse
        Vector3 yawForce = transform.up*_yawDirection*Throttle*MaxTorque;

        // Sowie die Abweichung vom Regelwert
        Vector3 yawErrorForce = transform.up*Throttle*PID_yaw_output*-1f;

        // Schlussendlich werden die einzelnen Kraefte auf die einzelnen Rotorpositionen am Dronenmodell uebertragen
        ApplyRotorForces(rotorForceFrontRight, rotorForceFrontLeft, rotorForceRearRight, rotorForceRearLeft, yawForce, yawErrorForce);

    }



    private void ApplyRotorForces(float rotorForceFrontRight, float rotorFrontLeft, float rotorForceRearRight, float rotorForceRearLeft, Vector3 yawForce, Vector3 yawErrorForce)
    {
        //Clamp the rotorforces, so it doesnt overextend the maximum
        float adjustedRotorForceFR = Mathf.Clamp(rotorForceFrontRight, 0f, MaxRotorForce);
        float adjustedRotorForceFL = Mathf.Clamp(rotorFrontLeft, 0f, MaxRotorForce);
        float adjustedRotorForceBR = Mathf.Clamp(rotorForceRearRight, 0f, MaxRotorForce);
        float adjustedRotorForceBL = Mathf.Clamp(rotorForceRearLeft, 0f, MaxRotorForce);

        //Add the force to the propellers
        AddForceToPropeller(RotorFrontRight, adjustedRotorForceFR);
        AddForceToPropeller(RotorFrontLeft, adjustedRotorForceFL);
        AddForceToPropeller(RotorRearRight, adjustedRotorForceBR);
        AddForceToPropeller(RotorRearLeft, adjustedRotorForceBL);

        // Add Torque to the rigidbody, so it spins on the y-axis
        _quadcopterRb.AddTorque(yawForce);
        _quadcopterRb.AddTorque(yawErrorForce);
    }



    void AddForceToPropeller(GameObject propellerObj, float propellerForce)
    {
        Vector3 propellerUp = propellerObj.transform.up;

        Vector3 propellerPos = propellerObj.transform.position;

        _quadcopterRb.AddForceAtPosition(propellerUp * propellerForce, propellerPos);

        //Debug
        //Debug.DrawRay(propellerPos, propellerUp * 1f, Color.red);
    }

    private float GetYawError()
    {
        return _quadcopterRb.angularVelocity.y;
    }

    //Pitch is rotation around x-axis
    //Returns positive if pitching forward
    private float GetPitchError()
    {
        float xAngle = transform.eulerAngles.x;

        //Make sure the angle is between 0 and 360
        xAngle = WrapAngle(xAngle);

        //This angle going from 0 -> 360 when pitching forward
        //So if angle is > 180 then it should move from 0 to 180 if pitching back
        if (xAngle > 180f && xAngle < 360f)
        {
            xAngle = 360f - xAngle;

            //-1 so we know if we are pitching back or forward
            xAngle *= -1f;
        }

        return xAngle;
    }

    //Roll is rotation around z-axis
    //Returns positive if rolling left
    private float GetRollError()
    {
        float zAngle = transform.eulerAngles.z;

        //Make sure the angle is between 0 and 360
        zAngle = WrapAngle(zAngle);

        //This angle going from 0-> 360 when rolling left
        //So if angle is > 180 then it should move from 0 to 180 if rolling right
        if (zAngle > 180f && zAngle < 360f)
        {
            zAngle = 360f - zAngle;

            //-1 so we know if we are rolling left or right
            zAngle *= -1f;
        }

        return zAngle;
    }

    //Wrap between 0 and 360 degrees
    float WrapAngle(float inputAngle)
    {
        //The inner % 360 restricts everything to +/- 360
        //+360 moves negative values to the positive range, and positive ones to > 360
        //the final % 360 caps everything to 0...360
        return ((inputAngle % 360f) + 360f) % 360f;
    }


    private void CalculateRotorPositions()
    {
        // Used to find the 4 corners of the box collider of the Quadcopter Manual New Object and sets the 4 rotorpositions to the corners
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        Vector3 boxColliderCenter = boxCollider.center;

        Vector3 boxColliderSize = boxCollider.size;

        Vector3 size = boxColliderSize;

        Vector3 vr_vertPositionLocal = new Vector3(boxColliderCenter.x + size.x / 2, 0.0f, boxColliderCenter.z + size.z / 2);

        RotorFrontRight.transform.position = boxCollider.transform.TransformPoint(vr_vertPositionLocal);

        Vector3 hl_vertPositionLocal = new Vector3(boxColliderCenter.x - size.x / 2, 0.0f, boxColliderCenter.z - size.z / 2);

        RotorRearLeft.transform.position = boxCollider.transform.TransformPoint(hl_vertPositionLocal);

        Vector3 vl_vertPositionLocal = new Vector3(vr_vertPositionLocal.x - size.x, 0.0f, vr_vertPositionLocal.z);

        RotorFrontLeft.transform.position = boxCollider.transform.TransformPoint(vl_vertPositionLocal);

        Vector3 hr_vertPositionLocal = new Vector3(hl_vertPositionLocal.x + size.x, 0.0f, hl_vertPositionLocal.z);

        RotorRearRight.transform.position = boxCollider.transform.TransformPoint(hr_vertPositionLocal);
    }

    public void SetPitchGains(Vector3 gainValues)
    {
        _pidPitchGains = gainValues;
    }

    public void SetRollGains(Vector3 gainValues)
    {
        _pidRollGains = gainValues;
    }

    public void SetYawGains(Vector3 gainValues)
    {
        _pidYawGains = gainValues;
    }


        
}
