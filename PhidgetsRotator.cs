using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhidgetsRotator : MonoBehaviour
{
    //public variables
    public float lerpSpeed = 0.5f;
    public int ifKitNumber = 0;
    public bool rotating = false;


    //private variables
    [Range(0.0f, 360.0f)]
    private float targetRotationY;

    private Quaternion targetRotation;
    private Transform currentRotation;

    private float[] recentEncoderValues = new float[100];
    private float recentAverage;
    private int encoderIteration = 1;

    private float rotationValue;
    private float rotationCheckTime;

    private float encoderValue;

    // Use this for initialization
    void Start()
    {
        //define variables
        targetRotation = Quaternion.identity;
        currentRotation = transform;
        rotationValue = transform.rotation.eulerAngles.y;
        rotationCheckTime = Time.time;
        encoderValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Check whether head is rotating every second
        if (Time.time - rotationCheckTime > 1.0f)
            SetRotationState();

        //Get rotary values from Phidgets ifKit
        GetEncoderValue();

        //set recent average rotation values
        SetRecentAverage();

        //rotate head based on encoder value
        RotateHead();
    }

    //rotate head based on encoderValue
    private void RotateHead()
    {
        if (recentAverage >= 300 && encoderValue < 100)
        {
            targetRotation.eulerAngles = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Lerp(currentRotation.rotation, targetRotation, Time.time * lerpSpeed);
        }
        else
        {
            targetRotation.eulerAngles = new Vector3(0, encoderValue, 0);
            transform.rotation = Quaternion.Lerp(currentRotation.rotation, targetRotation, Time.time * lerpSpeed);
        }
    }

    //set whether head is rotating or not
    private void SetRotationState()
    {
        if (rotationValue.Equals(transform.rotation.eulerAngles.y))
        {
            rotating = false;
        }
        else
        {
            rotating = true;
        }
        rotationCheckTime = Time.time;
        rotationValue = transform.rotation.eulerAngles.y;
    }

    //get encoder value from Phidgets ifKit
    private void GetEncoderValue()
    {
        if (ifKitNumber == 1)
        {
            encoderValue = PhidgetsManager.Instance.ifKit1EncoderVal;
        }
        else if (ifKitNumber == 2)
        {
            encoderValue = PhidgetsManager.Instance.ifKit2EncoderVal;
        }
        else if (ifKitNumber == 3)
        {
            encoderValue = PhidgetsManager.Instance.ifKit3EncoderVal;
        }
    }

    //set recent average rotation value
    private void SetRecentAverage()
    {
        recentAverage = 0;
        if (encoderIteration >= recentEncoderValues.Length)
        {
            encoderIteration = 1;
        }
        recentEncoderValues[encoderIteration] = encoderValue;

        encoderIteration++;
        for (int i = 1; i < recentEncoderValues.Length; i++)
        {
            recentAverage += recentEncoderValues[i];
        }
        recentAverage /= recentEncoderValues.Length;
    }
}
