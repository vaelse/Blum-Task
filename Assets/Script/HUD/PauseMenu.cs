using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject player;
    InputSystem input;
    public static bool isMenuActive = false;

    void Awake()
    {
        input = new InputSystem();
    }

    private void Update()
    {
        if (input.Player.Restart.triggered)
        {
            if (!isMenuActive)
            {
                Pause();
            }
            else if (isMenuActive && player.GetComponent<PlayerDamaged>().currentHealth > 0)
            {
                UnPause();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        isMenuActive = true;
        Time.timeScale = 0;
    }

    void UnPause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isMenuActive = false;
    }
    public void Restart()
    {
        isMenuActive = false;
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }
}
