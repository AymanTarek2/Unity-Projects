using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class Communication : MonoBehaviour
{
    SerialPort serial = new SerialPort("COM3", 115200);
    private int angle;

    public GameObject arm;
    private ServoArmController servoArmController;
    public void Start()
    {
        serial.Open();
        serial.WriteTimeout = 5000;
        
        this.servoArmController = this.arm.GetComponent<ServoArmController>();
    }

    public void Update()
    {
        if (serial.IsOpen)
        {
            angle = (int) servoArmController.getCurrentRotation();
            string angleStr = angle.ToString();
            serial.Write(angleStr + ',');
            Debug.Log(angleStr);
        }
        else
        {
            Debug.Log("serial is closed");
        }
    }


}

