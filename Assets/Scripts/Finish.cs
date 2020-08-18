using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public enum Sides
    {     
        Top,
        Right,
        Left,
        Bottom
    }

    [NonSerialized]
    public Sides side;

    private void Start()
    {
        EdgeCollider2D collider = gameObject.AddComponent<EdgeCollider2D>();
        collider.isTrigger = true;

        Vector2[] points = new Vector2[2];

        switch (side)
        {
            case Sides.Top:
                points[0] = new Vector2(0, 1);
                points[1] = new Vector2(1, 1);
                break;
            case Sides.Right:
                points[0] = new Vector2(1, 0);
                points[1] = new Vector2(1, 1);
                break;
            case Sides.Left:
                points[0] = new Vector2(0, 0);
                points[1] = new Vector2(0, 1);
                break;
            case Sides.Bottom:
                points[0] = new Vector2(0, 0);
                points[1] = new Vector2(1, 0);
                break;
            default:
                break;
        }

        collider.points = points;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Win();
        }
    }
}
