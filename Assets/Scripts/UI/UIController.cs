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

    public void UpdateHealthText()
    {
        healthText.text = "Lives: " + playerHealth.GetHealth();
    }

    public void SetPlayerHealth(PlayerHealth health)
    {
        playerHealth = health;
    }
}
