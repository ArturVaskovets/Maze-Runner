using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Singleton
    private static CameraManager _instance;
    public static CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Trying to find Camera Manager");
                _instance = FindObjectOfType<CameraManager>();
                Debug.Log($"Camera Manager - {_instance}");
            }
            //Debug.Log($"Returning Camera Manager - {_instance}");
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

    public Camera mainCamera;
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera globalCamera;

    private GameObject _cameraPivot;

    private void Start()
    {
        Invoke("CameraViewToggle", 0.5f);
    }

    public void Initialize()
    {
        if (_cameraPivot != null)
            Destroy(_cameraPivot.gameObject);
        _cameraPivot = new GameObject("Camera Pivot");
        _cameraPivot.transform.position = new Vector3((GameManager.Instance.maze.width - 1f) / 2f, (GameManager.Instance.maze.height - 1f) / 2f, -10f);

        globalCamera.Follow = _cameraPivot.transform;
        SetCameraSize();
    }
    public void SetCameraSize()
    {
        float wUnitsPerPixel = (float)GameManager.Instance.maze.width / Screen.width;
        float hUnitsPerPixel = (float)GameManager.Instance.maze.height / Screen.height;

        if (wUnitsPerPixel > hUnitsPerPixel)
            globalCamera.m_Lens.OrthographicSize = 0.5f * Screen.height * wUnitsPerPixel;
        else
            globalCamera.m_Lens.OrthographicSize = 0.5f * Screen.height * hUnitsPerPixel;

    }

    public void SetCameraTarget()
    {
        playerCamera.Follow = GameManager.Instance.Player.transform;
    }

    public void CameraViewToggle()
    {
        int temp = playerCamera.Priority;
        playerCamera.Priority = globalCamera.Priority;
        globalCamera.Priority = temp;
    }
}
