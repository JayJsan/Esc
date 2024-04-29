using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [Header("Health Configuration")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    [Header("Menu Configuration")]
    [SerializeField] private GameObject menu;
    [Header("Lose Configuration")]
    [SerializeField] private GameObject loseUI;
    // 
    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleMenu();
        }
    }

    public void HandleMenu()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            OpenMenu();
        }
        else
        {
            CloseMenu();
        }
    }


    public void OpenMenu()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
    }

    // return to main menu (scene 0)
    public void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToNextLevel()
    {
        // check if next build index is valid
        // if not, return to main menu
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1 >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoseGame()
    {
        Debug.Log("Lose");
        Time.timeScale = 0f;
        loseUI.SetActive(true);
    }

    public void UpdateHealthText()
    {
        healthText.text = "Lives: " + playerHealth.GetHealth();
    }

    public void SetPlayerHealth(PlayerHealth health)
    {
        playerHealth = health;
    }
}
