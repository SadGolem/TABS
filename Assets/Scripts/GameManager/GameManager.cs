using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private GameObject window;
    [SerializeField] private EnemySpawn spawner;
    private bool isPlayerTurn = true; // Переменная для определения, чей сейчас ход
    private int hod = 0;

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        RandomTurn();
    }
    public void GameEndResult()
    {
        if (hod >= 6)
        {
            UnitManager unitManager = UnitManager.instance;
            if (unitManager.GetFriendUnits().Count == 0)
            {

                window.SetActive(true);
                textMeshProUGUI.text = "Enemie is win";
                Time.timeScale = 0;
            }
            else if (unitManager.GetEnemyUnits().Count == 0)
            {
                window.SetActive(true);
                textMeshProUGUI.text = "You are win";
                Time.timeScale = 0;
            }
        }
    }
    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

    // Метод для изменения хода между игроком и ботом
    public void EndTurn()
    {
        if (hod < 8)
        {
            isPlayerTurn = !isPlayerTurn; // Переключаем переменную между игроком и ботом
            if (!isPlayerTurn)
            {
                // Ход бота
                BotTurn();
                hod++;
            }
            else
            {
                hod++;
                // Ход игрока
            }
        }
    }

    // Метод для реализации хода бота
    private void BotTurn()
    {

        spawner.SpawnEnemyUnit();

        EndTurn(); // Это может вызвать ход игрока
    }

    private void RandomTurn()
    {
        // Рандомно определяем, кто начнет первым
        isPlayerTurn = (Random.Range(0, 2) == 0); // 0 - начинает игрок, 1 - начинает бот
        if (isPlayerTurn)
            EndTurn();
        else
        {
            BotTurn();
        }
    }
}
