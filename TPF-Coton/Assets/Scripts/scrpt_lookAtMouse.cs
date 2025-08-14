using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrpt_lookAtMouse : MonoBehaviour
{
    bool _switchView = true;

    void Update()
    {
        Vector3 _mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        transform.LookAt(Camera.main.ScreenToWorldPoint(_mouse), Vector3.up);

        if (_switchView)
        {
            transform.GetChild(0).gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.GetChild(0).gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void Switch()
    {
        _switchView = !_switchView;
    }
}
