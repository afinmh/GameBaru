using System;
using System.IO.Ports;
using UnityEngine;

public class IMUSerialReader : MonoBehaviour
{
    public string portName = "COM9";
    public int baudRate = 115200;
    private SerialPort serial;
    public float pitch;
    public float yaw;

    private const int bufferSize = 256; // Ukuran buffer yang cukup besar untuk membaca data serial

    void Start()
    {
        // Membuka koneksi serial
        serial = new SerialPort(portName, baudRate);
        serial.ReadTimeout = 100; // Timeout baca data
        serial.WriteTimeout = 100; // Timeout tulis data
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
                string line = ReadSerialLine(); // Membaca satu baris data dari serial
                if (!string.IsNullOrEmpty(line))
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        // Ekstrak nilai numerik dari format "Pitch: -3.38" dan "Yaw: -11.31"
                        pitch = ExtractFloat(parts[0]);
                        yaw = ExtractFloat(parts[1]);
                    }
                    else
                    {
                        Debug.LogWarning("Invalid data format received: " + line);
                    }
                }
            }
            catch (TimeoutException)
            {
                // Timeout terjadi jika tidak ada data yang dibaca
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Serial read failed: " + ex.Message);
            }
        }
    }

    // Fungsi untuk membaca satu baris data dari serial
    private string ReadSerialLine()
    {
        try
        {
            string line = string.Empty;
            if (serial.BytesToRead > 0)
            {
                line = serial.ReadLine();
            }
            return line;
        }
        catch (TimeoutException)
        {
            // Timeout saat membaca
            return string.Empty;
        }
    }

    // Fungsi untuk mengekstrak nilai float dari string yang memiliki format "Label: Value"
    private float ExtractFloat(string data)
    {
        try
        {
            // Mengambil substring setelah tanda ":" dan mengonversinya ke float
            string valueString = data.Substring(data.IndexOf(':') + 1).Trim();
            return float.Parse(valueString);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Failed to extract float from: " + data + " - Error: " + ex.Message);
            return 0f; // Kembalikan nilai default jika terjadi kesalahan
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
