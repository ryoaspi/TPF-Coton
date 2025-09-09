using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrpt_turnAround : MonoBehaviour
{
    public float m_RotationSpeed;

    void Update()
    {
        transform.Rotate(Vector3.up * (m_RotationSpeed * Time.deltaTime));
    }

}
