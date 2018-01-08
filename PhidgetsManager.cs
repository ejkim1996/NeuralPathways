using UnityEngine; 
using System.Collections;
using System.Collections.Generic;
using Phidgets;
using Phidgets.Events;

public class PhidgetsManager : MonoBehaviour {

	public static PhidgetsManager Instance { get; private set; }
	public bool Attached { get; }

    public int kit1SerialID, kit2SerialID, kit3SerialID;

    private static InterfaceKit ifKit1, ifKit2, ifKit3;

	public bool IsPhidgetConnected = false;

    //Touch sensors - Phidget Analog input[0],[1],[2]
    public bool[] ifKit1Inputs, ifKit2Inputs, ifKit3Inputs = new bool[3];

	//Rotary potentiometer - Phidget Analog input[3]
	[Range(0.0f, 360.0f)]
	public float ifKit1EncoderVal, ifKit2EncoderVal, ifKit3EncoderVal; 
	private static float ifKit1LastEncoderVal, ifKit2LastEncoderVal, ifKit3LastEncoderVal = 0; 

	private static float minTouchVal = 10.0f; // Less than 10 -> turn off 
	public float debounceValue = 20.0f; 

	private void Awake () {

		Instance = this;
	}

	void Start () {
		ifKit1 = new InterfaceKit();
        ifKit2 = new InterfaceKit();
        ifKit3 = new InterfaceKit();

		ifKit1.Attach += new AttachEventHandler(ifKit_Attach);
		ifKit1.Detach += new DetachEventHandler(ifKit_Detach);
		ifKit1.Error += new ErrorEventHandler(ifKit_Error);

		ifKit1.SensorChange += new SensorChangeEventHandler(ifKit1_SensorChange);

        ifKit2.Attach += new AttachEventHandler(ifKit_Attach);
        ifKit2.Detach += new DetachEventHandler(ifKit_Detach);
        ifKit2.Error += new ErrorEventHandler(ifKit_Error);

        ifKit2.SensorChange += new SensorChangeEventHandler(ifKit2_SensorChange);

        ifKit3.Attach += new AttachEventHandler(ifKit_Attach);
        ifKit3.Detach += new DetachEventHandler(ifKit_Detach);
        ifKit3.Error += new ErrorEventHandler(ifKit_Error);

        ifKit3.SensorChange += new SensorChangeEventHandler(ifKit3_SensorChange);

        IsPhidgetConnected = true;
    }

    void Update () {
		if (IsPhidgetConnected) {
			init ();
		} else {
			ifKit1.close ();
            ifKit2.close();
            ifKit3.close();
		}
	}

	void init(){
		try{
			ifKit1.open(kit1SerialID);
            ifKit2.open(kit2SerialID);
            ifKit3.open(kit3SerialID);
			IsPhidgetConnected = true; 
		}catch(PhidgetException ex){
			Debug.Log("Please check connection(s) to the Phidget device! : "+ex);
			IsPhidgetConnected = false; 
		}
	}

	// Analog Inputs Event Handler 
	static void ifKit1_SensorChange(object sender, SensorChangeEventArgs e){
		if (e.Value > minTouchVal) {

			ResetTouchSensors (); 

			if (e.Index == 0) {
				Instance.ifKit1Inputs[0] = true; 
			} else if (e.Index == 1) {
				Instance.ifKit1Inputs[1] = true; 
			} else if (e.Index == 2) {
				Instance.ifKit1Inputs[2] = true; 
			}

		} else {
			ResetTouchSensors ();
		}

		//rotary potentiometer
		if (e.Index == 3) {
			float originEncoderVal = e.Value;

			//if the gap between values is bigger than 'debounceValue', then ignore value. 
			if (Mathf.Abs(originEncoderVal-ifKit1LastEncoderVal) < Instance.debounceValue ) {
				Instance.ifKit1EncoderVal = Map(0.0f, 360.0f, 0.0f, 999.9f, originEncoderVal); 
				//Debug.Log (originEncoderVal +" | " + Mathf.Abs(originEncoderVal-lastEncoderVal)+ " < " + Instance.debounceValue);

			}
			ifKit1LastEncoderVal = originEncoderVal; 
		}

	}

    static void ifKit2_SensorChange(object sender, SensorChangeEventArgs e)
    {
        if (e.Value > minTouchVal)
        {

            ResetTouchSensors();

            if (e.Index == 0)
            {
                Instance.ifKit2Inputs[0] = true;
            }
            else if (e.Index == 1)
            {
                Instance.ifKit2Inputs[1] = true;
            }
            else if (e.Index == 2)
            {
                Instance.ifKit2Inputs[2] = true;
            }

        }
        else
        {
            ResetTouchSensors();
        }

        //rotary potentiometer
        if (e.Index == 3)
        {
            float originEncoderVal = e.Value;

            //if the gap between values is bigger than 'debounceValue', then ignore value. 
            if (Mathf.Abs(originEncoderVal - ifKit2LastEncoderVal) < Instance.debounceValue)
            {
                Instance.ifKit2EncoderVal = Map(0.0f, 360.0f, 0.0f, 999.9f, originEncoderVal);
                //Debug.Log (originEncoderVal +" | " + Mathf.Abs(originEncoderVal-lastEncoderVal)+ " < " + Instance.debounceValue);

            }
            ifKit2LastEncoderVal = originEncoderVal;
        }

    }

    static void ifKit3_SensorChange(object sender, SensorChangeEventArgs e)
    {
        if (e.Value > minTouchVal)
        {

            ResetTouchSensors();

            if (e.Index == 0)
            {
                Instance.ifKit3Inputs[0] = true;
            }
            else if (e.Index == 1)
            {
                Instance.ifKit3Inputs[1] = true;
            }
            else if (e.Index == 2)
            {
                Instance.ifKit3Inputs[2] = true;
            }

        }
        else
        {
            ResetTouchSensors();
        }

        //rotary potentiometer
        if (e.Index == 3)
        {
            float originEncoderVal = e.Value;

            //if the gap between values is bigger than 'debounceValue', then ignore value. 
            if (Mathf.Abs(originEncoderVal - ifKit3LastEncoderVal) < Instance.debounceValue)
            {
                Instance.ifKit3EncoderVal = Map(0.0f, 360.0f, 0.0f, 999.9f, originEncoderVal);
                //Debug.Log (originEncoderVal +" | " + Mathf.Abs(originEncoderVal-lastEncoderVal)+ " < " + Instance.debounceValue);

            }
            ifKit3LastEncoderVal = originEncoderVal;
        }

    }

    private static void ResetTouchSensors(){
            Instance.ifKit1Inputs[0] = false;
            Instance.ifKit1Inputs[1] = false;
            Instance.ifKit1Inputs[2] = false;

            Instance.ifKit2Inputs[0] = false;
            Instance.ifKit2Inputs[1] = false;
            Instance.ifKit2Inputs[2] = false;

            Instance.ifKit3Inputs[0] = false;
            Instance.ifKit3Inputs[1] = false;
            Instance.ifKit3Inputs[2] = false;

    }

	//Map
	public static float Map(float from, float to, float from2, float to2, float value){
		if(value <= from2){
			return from;
		}else if(value >= to2){
			return to;
		}else{
			return (to - from) * ((value - from2) / (to2 - from2)) + from;
		}
	}

	static void ifKit_Attach(object sender, AttachEventArgs e)
	{
		Debug.Log("Attached : " +e.Device.SerialNumber.ToString()); 
		Instance.IsPhidgetConnected = true;
	}

	//Detach event handler...Display the serial number of the detached InterfaceKit to the console
	static void ifKit_Detach(object sender, DetachEventArgs e)
	{	
		string msg = "Detached : " + e.Device.SerialNumber.ToString();
		Debug.Log (msg);
		Instance.IsPhidgetConnected = false;
		ifKit1.close ();
	}

	//Error event handler...Display the error description to the console
	static void ifKit_Error(object sender, ErrorEventArgs e)
	{
		Debug.Log("Error : " + e.Description);
	}

	//close the Phidgets when the app closed. 
	void OnApplicationQuit(){
		ifKit1.close (); 
	}


}
