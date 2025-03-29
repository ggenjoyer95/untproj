using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private InputField playersInput;
    [SerializeField] private List<GameObject> playerPanels;
    [SerializeField] private Text errorText;

    private void Awake()
    {
        for (int i = 0; i < playerPanels.Count; i++)
        {
            string savedNameKey = (i + 1).ToString();
            if (PlayerPrefs.HasKey(savedNameKey))
            {
                PlayerPrefs.DeleteKey(savedNameKey);
            }
        }

        for (int i = 0; i < playerPanels.Count; i++)
        {
            InputField nameInput = playerPanels[i].GetComponentInChildren<InputField>();
            nameInput.text = "Player" + (i + 1);
        }
        for (int i = 0; i < playerPanels.Count; i++)
        {
            InputField nameInput = playerPanels[i].GetComponentInChildren<InputField>();
            string savedNameKey = (i + 1).ToString();
            nameInput.text = PlayerPrefs.GetString(savedNameKey);
        }

        if (errorText != null)
        {
            errorText.text = "";
        }
    }

    public void OnPlayersInputChanged(string text)
    {
        foreach (GameObject panel in playerPanels)
        {
            panel.SetActive(false);
        }

        int number;
        if (int.TryParse(text, out number))
        {
            if (number >= 2 && number <= 4)
            {
                for (int i = 0; i < number; i++)
                {
                    playerPanels[i].SetActive(true);
                }
            }
        }
        else
        {
        }
    }

    public void StartGame()
    {
        
        if (errorText != null)
        {
            errorText.text = "";
        }

        string inputText = playersInput.text;
        int numberOfPlayers;

        if (!int.TryParse(inputText, out numberOfPlayers))
        {
            if (errorText != null)
                errorText.text = "Некорректный ввод! Нужно ввести 2, 3 или 4.";
            return;
        }

        if (numberOfPlayers < 2 || numberOfPlayers > 4)
        {
            if (errorText != null)
                errorText.text = "Введите число 2, 3 или 4!";
            return;
        }

        PlayerPrefs.SetInt("players", numberOfPlayers);

        for (int i = 0; i < numberOfPlayers; i++)
        {
            InputField nameInput = playerPanels[i].GetComponentInChildren<InputField>();
            string playerName = nameInput.text;

            PlayerPrefs.SetString((i + 1).ToString(), playerName);
        }

        SceneManager.LoadScene(1);
    }
}
