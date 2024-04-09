using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMove : MonoBehaviour
{
    public float cursorSpeed = 5f;

    private void Update()
    {
        
        // �������� �������� �������������� � ������������ ���� ��������
        float horizontalInput = Input.GetAxis("P1RightHorizontal");
        float verticalInput = Input.GetAxis("P1RightVertical");
        Debug.Log(verticalInput);
        Debug.Log(horizontalInput);


        // ��������� ������ ����������� �������� �������
        Vector3 moveDirection = new Vector3(horizontalInput, -verticalInput, 0f).normalized;

        // ���������� ������ � ������������ � ������ ��������
        transform.Translate(moveDirection * cursorSpeed * Time.deltaTime);
    }

}
