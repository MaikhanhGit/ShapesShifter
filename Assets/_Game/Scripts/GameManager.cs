using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void OnExit(InputValue input)
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    private void OnRestart(InputValue input)
    {
        Debug.Log("Restart");
        SceneManager.LoadScene(0);
    }
}
