using System;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 2f;
    [SerializeField] private RectTransform _joystick, _joystickButton;
    
    private bool _isJoystickHeld;
    
    public static float HorizontalAxis { get; set; }
    public static float VerticalAxis { get; set; }

    private void Start()
    {
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#elif !UNITY_EDITOR
        _joystick.gameObject.SetActive(false);
        _joystickButton.gameObject.SetActive(false);
#endif
    }

    public void OnJoystickPressed()
    {
        _isJoystickHeld = true;
    }
    
    public void OnJoystickLifted()
    {
        _isJoystickHeld = false;
    }

    private void Update()
    {
        if (_isJoystickHeld)
        {
            _joystickButton.position = new Vector3(
                Mathf.Clamp(Input.mousePosition.x, _joystick.position.x-_joystick.sizeDelta.x, 
                    _joystick.position.x+_joystick.sizeDelta.x),
                Mathf.Clamp(Input.mousePosition.y, _joystick.position.y-_joystick.sizeDelta.y, 
                    _joystick.position.y+_joystick.sizeDelta.y));
            HorizontalAxis = Mathf.Clamp((_joystickButton.position.x - _joystick.position.x) / _joystick.sizeDelta.x * _sensitivity, -1f, 1f);
            VerticalAxis = Mathf.Clamp((_joystickButton.position.y - _joystick.position.y) / _joystick.sizeDelta.y * _sensitivity, -1f, 1f);
        }
        else
        {
            _joystickButton.position = _joystick.position;
            HorizontalAxis = 0;
            VerticalAxis = 0;
        }
    }
}
