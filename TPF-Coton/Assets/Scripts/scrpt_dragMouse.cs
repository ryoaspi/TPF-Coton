using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scrpt_dragMouse : MonoBehaviour
{
    public float m_rotDragSpeed = 1000;
    public float m_rotKeySpeed = 1000;

    public void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(Vector3.right * m_rotKeySpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(Vector3.left * m_rotKeySpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up * m_rotKeySpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.down * m_rotKeySpeed * Time.deltaTime);
        }
    }

    void OnMouseDrag()
    {
        float _rotX = Input.GetAxis("Mouse X") * m_rotDragSpeed * Time.deltaTime;
        float _rotY = Input.GetAxis("Mouse Y") * m_rotDragSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, _rotX);
        transform.Rotate(Vector3.right, _rotY);
    }

    public void ResetCard()
    {
        transform.rotation = Quaternion.identity;
    }
}
