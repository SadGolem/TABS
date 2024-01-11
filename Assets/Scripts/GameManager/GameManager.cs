using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject blockWindow;
    [SerializeField] private EnemySpawn spawner;
    [SerializeField] private Button[] buttons;
    private bool isPlayerTurn = true; // Переменная для определения, чей сейчас ход
    private int hod = 0;
    public bool isNotLastTurn = true;

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        RandomTurn();
        Time.timeScale = 1.0f;
    }
    public void GameEndResult()
    {
        if (hod >= 7)
        {
            UnitManager unitManager = UnitManager.instance;
            Debug.Log("friends:" + unitManager.GetFriendUnits().Count);
            Debug.Log("enemies:" + unitManager.GetEnemyUnits().Count);
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
    private void Update()
    {
        if (hod >= 7)
        {
            isNotLastTurn = false;
            GameEndResult(); }
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
                OffUI();
                BotTurn();
               
            }
            else
            {
                OnUI();
                // Ход игрока 
            }
            
        }
        else
            OffUI();
    }

    // Метод для реализации хода бота
    private void BotTurn()
    {
        if (hod != 7)
        {
            spawner.SpawnEnemyUnit();
            hod++;
            EndTurn(); // Это может вызвать ход игрока
        }
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

    private void OffUI()
    {
        blockWindow.SetActive(true);
        foreach (Button btn in buttons)
        {
            btn.interactable = false;
        }
    }

    private void OnUI()
    {
        blockWindow.SetActive(false);
        foreach (Button btn in buttons)
        {
            btn.interactable = true;
            
        }
    }
}