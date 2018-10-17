using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void SinglePlayer() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Multiplayer() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}
