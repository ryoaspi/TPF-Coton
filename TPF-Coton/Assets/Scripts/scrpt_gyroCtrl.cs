using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scrpt_gyroCtrl : MonoBehaviour
{
    Gyroscope _gyro;
    Quaternion _convertgyro;
    bool _switchView = true;
    public float m_rotintensity = 10;

    public void Start()
    {
        _gyro = Input.gyro;

        _gyro.enabled = true;
    }

    public void Update()
    {
        _convertgyro = _gyro.attitude;

        Camera.main.transform.eulerAngles = transform.eulerAngles;

        if (_switchView)
        {
            transform.eulerAngles = new Vector3((_convertgyro.x * m_rotintensity) - 45, (_convertgyro.y * m_rotintensity), 0);
        }

        else
        {
            transform.eulerAngles = new Vector3((_convertgyro.x * m_rotintensity) - 45, (_convertgyro.y * m_rotintensity) + 180, 0);
        }
    }

    public void Switch()
    {
        _switchView = !_switchView;
    }


}
