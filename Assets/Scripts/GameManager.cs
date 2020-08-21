using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Trying to find Game Manager");
                _instance = FindObjectOfType<GameManager>();
                Debug.Log($"Game Manager - {_instance}");
            }
            //Debug.Log($"Returning Game Manager - {_instance}");
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion


    public GameObject playerPrefab;
    public GameObject mazePrefab;
    public Joystick joystick;

    public bool reloadSceneOnRestart;

    private GameObject _player;
    private Maze _maze;
    public GameObject Player { get => _player; }
    public Maze Maze { get => _maze;  }

    private const float _cellSize = 1f;



    void Start()
    {
        Init();
    }

    void Update()
    {

    }

    void Init()
    {
        _maze = Instantiate(mazePrefab, this.transform.position, Quaternion.identity).GetComponent<Maze>();
        Maze.Create();
        _player = Instantiate(playerPrefab, new Vector3(Maze.startCell.x + _cellSize / 2, Maze.startCell.y + _cellSize / 2, 0), Quaternion.identity);
        _player.GetComponent<PlayerControls>().joystick = this.joystick;
        CameraManager.Instance.SetCameraTarget();
        CameraManager.Instance.Initialize();
    }

    public void Restart()
    {
        if ( reloadSceneOnRestart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Destroy(Maze.gameObject);
            Destroy(_player);
            Init();
        }
        
    }
    public void Win()
    {
        Debug.Log("You win");
        SceneManager.LoadScene("Basic Maze");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
