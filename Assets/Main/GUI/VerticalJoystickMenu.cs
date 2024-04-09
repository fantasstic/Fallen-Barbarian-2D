using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalJoystickMenu : MonoBehaviour
{
    public float inputDelay = 0.1f;

    private float lastInputTime;
    private int currentIndex = 0;
    public Button[] buttons;

    void Start()
    {
        //buttons = GetComponentsInChildren<Button>();
        SelectButton(currentIndex);
    }

    void Update()
    {
        float verticalInput = Input.GetAxisRaw("P1JoystickVertical");
        float horizontalInput = Input.GetAxisRaw("P1JoystickHorizontal");


        if (Time.time - lastInputTime > inputDelay)
        {
            if (verticalInput != 0)
            {
                int newIndex = currentIndex - (int)Mathf.Sign(verticalInput);
                newIndex = Mathf.Clamp(newIndex, 0, buttons.Length - 1);

                if (newIndex != currentIndex)
                {
                    currentIndex = newIndex;
                    SelectButton(currentIndex);
                    lastInputTime = Time.time;
                }
            }
        }
    }

    void SelectButton(int index)
    {
        Debug.Log("Select");
        for (int i = 0; i < buttons.Length; i++)
        {
            ColorBlock colors = buttons[i].colors;
            colors.normalColor = Color.white;
            buttons[i].colors = colors;
        }

        ColorBlock selectedColors = buttons[index].colors;
        selectedColors.normalColor = Color.gray;
        buttons[index].colors = selectedColors;
    }
}
