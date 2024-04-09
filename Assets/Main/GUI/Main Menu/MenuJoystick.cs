using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuJoystick : MonoBehaviour
{
    public float inputDelay = 0.1f;

    private float lastInputTime;
    private int currentIndex = 0;
    public Button[] buttons;
    private RoundContorllerNew _controller;

    // Define the navigation map
    private Dictionary<int, Dictionary<string, int>> navigationMap = new Dictionary<int, Dictionary<string, int>>()
{
    {0, new Dictionary<string, int>() {{"down", 1}, {"up", 4}}},
    {1, new Dictionary<string, int>() {{"down", 2}, {"right", 3}, {"left", 3}, {"up", 0}}},
    {2, new Dictionary<string, int>() {{"up", 1}, {"right", 4}, {"left", 4}}},
    {3, new Dictionary<string, int>() {{"down", 4}, {"right", 1}, {"left", 1}, {"up", 0}}},
    {4, new Dictionary<string, int>() {{"down", 0}, {"right", 2}, {"left", 2}, {"up", 3}}},
};

    void Start()
    {
        //SelectButton(currentIndex);
        _controller = Camera.main.GetComponent<RoundContorllerNew>();
        _controller.RoundStart = false; 

    }

    void Update()
    {
        float verticalInput = Input.GetAxisRaw("P1JoystickVertical");
        float horizontalInput = Input.GetAxisRaw("P1JoystickHorizontal");

        //Debug.Log(verticalInput);

        if (Time.time - lastInputTime > inputDelay)
        {
            string direction = null;
            if (verticalInput > 0) direction = "up";
            else if (verticalInput < 0) direction = "down";
            else if (horizontalInput > 0) direction = "right";
            else if (horizontalInput < 0) direction = "left";

            if (direction != null && navigationMap[currentIndex].ContainsKey(direction))
            {
                currentIndex = navigationMap[currentIndex][direction];
                //SelectButton(currentIndex);
                lastInputTime = Time.time;
            }
        }
    }

    void SelectButton(int index)
    {
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



