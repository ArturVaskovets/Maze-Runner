using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    private Vector2 _move_direction;

    public float speed;

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
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
    }

    private void FixedUpdate()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        _move_direction = new Vector2(xAxis, yAxis);
        Move(_move_direction);
    }

    private void Move(Vector2 direction)
    {
        _rb2D.velocity = direction * speed;
    }
}
