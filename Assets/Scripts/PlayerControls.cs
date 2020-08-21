using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    private Vector2 _move_direction;

    public float speed;
    public Joystick joystick;
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        Cursor.visible = false;
        //#if !UNITY_IOS && !UNITY_ANDROID --- another way
#if UNITY_STANDALONE
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }

    void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.V))
        {
            CameraManager.Instance.CameraViewToggle();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.Restart();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.Quit();
        }
#elif UNITY_ANDROID || UNITY_IOS

#endif
    }

    private void FixedUpdate()
    {
#if UNITY_STANDALONE
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
#elif UNITY_ANDROID || UNITY_IOS
        float xAxis = joystick.Horizontal;
        float yAxis = joystick.Vertical;
#endif
        _move_direction = new Vector2(xAxis, yAxis);
        Move(_move_direction);
    }

    private void Move(Vector2 direction)
    {
        _rb2D.velocity = direction * speed;
    }
}
