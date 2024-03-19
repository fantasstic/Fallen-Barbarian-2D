using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UFE3D;
using UnityEngine.EventSystems;

public class AndroidMove : MonoBehaviour
{
    public string horizontalAxisName = "Horizontal";
    public string verticalAxisName = "Vertical";

    void Update()
    {
        // if(Input.GetAxis("Horizontal"))
        Debug.Log(Input.GetAxisRaw("P1KeyboardVertical"));

    }
}
