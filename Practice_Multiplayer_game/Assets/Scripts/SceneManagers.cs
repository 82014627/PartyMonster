using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public void StartToLogin()
    {
        SceneManager.LoadScene("Login");
    }
    public static void TeachToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
    public static void LoginToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
