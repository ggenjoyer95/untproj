using UnityEngine;
using UnityEngine.UI;

public class Turn : MonoBehaviour
{
    [SerializeField] private Text turnText;

    private void Start()
    {
        GameManager.instance.Message += SetTurn;
    }

    private void SetTurn(Player player)
    {
        string playerName = GameManager.instance.GetPlayerName((int)player);
        turnText.text = "Ход: " + playerName;
    }
}
