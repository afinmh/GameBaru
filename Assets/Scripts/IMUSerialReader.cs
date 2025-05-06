using System;
using System.IO.Ports;
using UnityEngine;

public class IMUSerialReader : MonoBehaviour
{
    public string portName = "COM9"; // Sesuaikan dengan port yang digunakan
    public int baudRate = 38400; // Sesuaikan dengan baudrate di Arduino
    private SerialPort serial;

    public float gx, gy, gz;
    public int potValue;
    private int rawButton1;
    public int rawButton2;

    public bool button1SwitchState { get; private set; } = false;
    public bool button2SwitchState { get; private set; } = false;

    private int lastButton1 = 1;
    private int lastButton2 = 1;

    void Start()
    {
        serial = new SerialPort(portName, baudRate);
        serial.ReadTimeout = 100;
        serial.WriteTimeout = 100;
        try
        {
            serial.Open();
            Debug.Log("Serial port opened successfully.");
        }
        catch (Exception e)
        {
            Debug.LogWarning("Serial open failed: " + e.Message);
        }
    }

    void Update()
    {
        if (serial != null && serial.IsOpen)
        {
            try
            {
                serial.Write(".");
                string line = ReadSerialLine();
                if (!string.IsNullOrEmpty(line))
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 6)
                    {
                        if (
                            float.TryParse(parts[0], out gx) &&
                            float.TryParse(parts[1], out gy) &&
                            float.TryParse(parts[2], out gz) &&
                            int.TryParse(parts[3], out potValue) &&
                            int.TryParse(parts[4], out rawButton1) &&
                            int.TryParse(parts[5], out rawButton2))
                        {
                            // Toggle logic for button1 (switch)
                            if (rawButton1 == 0 && lastButton1 == 1)
                                button1SwitchState = !button1SwitchState;
                            lastButton1 = rawButton1;

                            // Toggle logic for button2 (switch)
                            if (rawButton2 == 0 && lastButton2 == 1)
                                button2SwitchState = !button2SwitchState;
                            lastButton2 = rawButton2;

                            Debug.Log($"gx: {gx}, gy: {gy}, gz: {gz}, Pot: {potValue}, B1: {button1SwitchState}, B2: {button2SwitchState}");
                        }
                        else
                        {
                            Debug.LogWarning("Failed to parse sensor data: " + line);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Invalid data format received: " + line);
                    }
                }
            }
            catch (TimeoutException)
            {
                // Do nothing on timeout
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Serial read failed: " + ex.Message);
            }
        }
    }

    private string ReadSerialLine()
    {
        try
        {
            if (serial.BytesToRead > 0)
                return serial.ReadLine();
            return string.Empty;
        }
        catch (TimeoutException)
        {
            return string.Empty;
        }
    }

    void OnApplicationQuit()
    {
        if (serial != null && serial.IsOpen)
        {
            serial.Close();
            Debug.Log("Serial port closed.");
        }
    }
}
