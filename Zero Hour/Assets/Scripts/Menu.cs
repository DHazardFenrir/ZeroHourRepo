using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;
using System;
public class Menu : MonoBehaviour
{
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject dieMenu;
    [SerializeField] Button startButton;
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject bossHUD;   
    [SerializeField] Button exitButton;
    [SerializeField] Button retryButton;
    public GameObject scene;
   
    void Awake()
    {
        
        if (!startMenu.activeSelf)
        {
            StartGame();
        }
    }
    public void StartGame()
    {
        startMenu.SetActive(false);
        playerHUD.SetActive(true);
        dieMenu.SetActive(false);
        scene.SetActive(true);
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

  public void ShowGameOver()
    {
        startMenu.SetActive(false);
        playerHUD.SetActive(false);
        dieMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Restart()
    {
      Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       
    }
}
