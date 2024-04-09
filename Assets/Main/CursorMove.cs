using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMove : MonoBehaviour
{
    public float cursorSpeed = 5f;

    private void Update()
    {
        
        // Получаем значения горизонтальной и вертикальной осей геймпада
        float horizontalInput = Input.GetAxis("P1RightHorizontal");
        float verticalInput = Input.GetAxis("P1RightVertical");
        Debug.Log(verticalInput);
        Debug.Log(horizontalInput);


        // Вычисляем вектор направления движения курсора
        Vector3 moveDirection = new Vector3(horizontalInput, -verticalInput, 0f).normalized;

        // Перемещаем курсор в соответствии с вводом геймпада
        transform.Translate(moveDirection * cursorSpeed * Time.deltaTime);
    }

}
