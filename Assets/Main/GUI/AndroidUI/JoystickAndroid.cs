using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickAndroid : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Direction Thresholds")]
    [SerializeField] private float forwardThreshold = 0.5f;
    [SerializeField] private float backwardThreshold = -0.5f;
    [SerializeField] private float upThreshold = 0.5f;
    [SerializeField] private float downThreshold = -0.5f;
    [SerializeField] private float bottomRightThreshold = 0.5f;
    [SerializeField] private float bottomLeftThreshold = -0.5f;
    [SerializeField] private CustomInterface _interface;

    private Image outerCircle;//Outer boundary of joystick.
    private float bgImageSizeX, bgImageSizey;
    private Image innerCircle;//Inner circle of joystick.
    public JoystickDirection joyStickDirection;
    /// <summary>
    /// This defines how far joystick inner circle can move with respect to outer circle.
    /// Since inner circle needs to move only half size distance of outer circle default value is its half size i.e 0.5
    /// </summary>
    float offsetFactorWithBgSize = 0.5f;
    public static event Action<Vector2> onJoyStickMoved;

    public Vector2 InputDirection { set; get; }
    public bool isMovingForward;
    public bool isMovingBackward;
    public bool isMovingUp;
    public bool isMovingDown;
    public bool isMovingBottomRight;
    public bool isMovingBottomLeft;


    private bool _boolName;

    private void Start()
    {

        outerCircle = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        innerCircle = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        bgImageSizeX = outerCircle.rectTransform.sizeDelta.x;
        bgImageSizey = outerCircle.rectTransform.sizeDelta.y;
    }


    /// <summary>
    /// Unity function called when we are tapping and moving finger in screen.
    /// </summary>
    /// <param name="ped"></param>
    public void OnDrag(PointerEventData ped)
    {
        Vector2 tappedpOint;
        //This if statment gives local position of the pointer at "out touchPoint"
        //if we press or touched inside the area of outerCircle
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle
            (outerCircle.rectTransform, ped.position, ped.pressEventCamera, out tappedpOint))
        {

            //Getting tappedPoint position in fraction where  maxmimum value would be in denominator of below fraction.
            tappedpOint.x = (tappedpOint.x / (bgImageSizeX * offsetFactorWithBgSize));
            tappedpOint.y = (tappedpOint.y / (bgImageSizey * offsetFactorWithBgSize));

            SetJoyStickDirection(tappedpOint.x, tappedpOint.y);//Updates InputDirection value.
                                                               //Limit value of InputDirection between 0 and 1.
            InputDirection = InputDirection.magnitude > 1 ? InputDirection.normalized : InputDirection;
            //Updating position of inner circle of joystick.
            innerCircle.rectTransform.anchoredPosition =
                new Vector3(InputDirection.x * (outerCircle.rectTransform.sizeDelta.x * offsetFactorWithBgSize),
                    InputDirection.y * (outerCircle.rectTransform.sizeDelta.y * offsetFactorWithBgSize));

            Debug.Log(InputDirection);

            isMovingForward = InputDirection.x > forwardThreshold && InputDirection.y > downThreshold && InputDirection.y < upThreshold;
            isMovingBackward = InputDirection.x < backwardThreshold && InputDirection.y > downThreshold && InputDirection.y < upThreshold;
            isMovingUp = InputDirection.y > upThreshold;
            isMovingDown = InputDirection.y < downThreshold && InputDirection.x < bottomRightThreshold && InputDirection.x > bottomLeftThreshold;
            isMovingBottomRight = InputDirection.x > bottomRightThreshold && InputDirection.y < downThreshold;
            isMovingBottomLeft = InputDirection.x < bottomLeftThreshold && InputDirection.y < downThreshold;


            onJoyStickMoved?.Invoke(InputDirection);

        }
    }

    private void Update()
    {
        if (isMovingBackward)
            _interface.BackButtonPressed = true;
        else
            _interface.BackButtonPressed = false;
        if (isMovingUp)
            _interface.Button1Pressed = true;
        else
            _interface.Button1Pressed = false;
        if (isMovingForward)
            _interface.ForwardButtonPressed = true;
        else
            _interface.ForwardButtonPressed = false;
        if (isMovingDown)
            _interface.CrouchButtonPressed = true;
        else
            _interface.CrouchButtonPressed = false;
        if (isMovingBottomRight)
            _interface.RollForwardButtonPressed = true;
        else
            _interface.RollForwardButtonPressed = false;
        if (isMovingBottomLeft)
            _interface.RollBackButtonPressed = true;
        else
            _interface.RollBackButtonPressed = false;
    }

    public GameObject joyStickparent;
    /// <summary>
    /// Unity function called when we tapped on the screen.
    /// Here we enable joystick at initial press point.
    /// </summary>
    public virtual void OnPointerDown(PointerEventData ped)
    {

        Vector2 initMousePos = ped.pressEventCamera.ScreenToWorldPoint(Input.mousePosition);
        joyStickparent.SetActive(true);

        joyStickparent.transform.position = initMousePos;
        OnDrag(ped);

    }

    /// <summary>
    /// Unity function called when mouse button is not pressed or no touch detected in touch screen device.
    ///Disabling joystick and resetting joystick innerCircle to zero position.
    /// </summary>
    public virtual void OnPointerUp(PointerEventData ped)
    {

        InputDirection = Vector2.zero;
        innerCircle.rectTransform.anchoredPosition = Vector3.zero;
        isMovingForward = false;
        isMovingBackward = false;
        isMovingUp = false;
        isMovingDown = false;
        isMovingBottomRight = false;
        isMovingBottomLeft = false;
        //joyStickparent.gameObject.SetActive(false);
        onJoyStickMoved?.Invoke(InputDirection);
    }

    /// <summary>
    /// Changes input direction value based on joy stick direction we have set.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetJoyStickDirection(float x, float y)
    {
        if (joyStickDirection == JoystickDirection.Both)
        {
            // For both horizonatal and vertical directional joystick
            InputDirection = new Vector3(x, y);
        }
        else if (joyStickDirection == JoystickDirection.Vertical)
        {
            //for y directional joystick
            InputDirection = new Vector3(0, y);
        }
        else if (joyStickDirection == JoystickDirection.Horizontal)
        {
            //for x dirctional joystick
            InputDirection = new Vector3(x, 0);
        }
    }

}
public enum JoystickDirection
{
    Horizontal,
    Vertical,
    Both
}

