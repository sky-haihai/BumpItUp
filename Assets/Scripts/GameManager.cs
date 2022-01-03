using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("LeftSide", LoadSceneMode.Additive);
        SceneManager.LoadScene("RightSide", LoadSceneMode.Additive);
    }
}