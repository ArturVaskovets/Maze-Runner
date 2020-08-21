using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputDisabler : MonoBehaviour
{
    public GameObject[] gameObjects;
    private void Awake()
    {
#if !UNITY_ANDROID && !UNITY_IOS
        foreach (GameObject gObject in gameObjects)
        {
            gObject.SetActive(false);
        }
        
#endif
    }



}
