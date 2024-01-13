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
    private bool isPlayerTurn = true; // ���������� ��� �����������, ��� ������ ���
    private int hod = 0;
    private int maxHod = 8;
    public bool isNotLastTurn = true;

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        PlayerPrefs.SetInt("MaxSpawnCount", 7);
        RandomTurn();
        Time.timeScale = 1.0f;
    }
    public void GameEndResult()
    {
        if (hod >= maxHod)
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
        if (hod >= maxHod + 1)
        {
            isNotLastTurn = false;
            GameEndResult(); }
    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

    // ����� ��� ��������� ���� ����� ������� � �����
    public void EndTurn()
    {
        if (hod < maxHod + 1)
        {
            if (hod >= 7)
            {
                isPlayerTurn = true;
                hod++;
            }
            isPlayerTurn = !isPlayerTurn; // ����������� ���������� ����� ������� � �����
            if (!isPlayerTurn)
            {
                // ��� ����
                OffUI();
                BotTurn();
               
            }
            else
            {
                OnUI();
                // ��� ������ 
            }
            
        }
        else
            OffUI();
    }

    // ����� ��� ���������� ���� ����
    private void BotTurn()
    {
        
        if (hod < 7)
        {
            hod++;
            EndTurn();
            spawner.SpawnEnemyUnit();
            
             // ��� ����� ������� ��� ������
        }
    }

    private void RandomTurn()
    {
        // �������� ����������, ��� ������ ������
        isPlayerTurn = (Random.Range(0, 2) == 0); // 0 - �������� �����, 1 - �������� ���
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

    public void OnUI()
    {
        blockWindow.SetActive(false);
        foreach (Button btn in buttons)
        {
            btn.interactable = true;
            
        }
    }

    public void GetPlusThreeUnits()
    {
        foreach (Button btn in buttons)
        {
            btn.interactable = true;

        }
        blockWindow.SetActive(false);
        maxHod = 10;
        isNotLastTurn = true;
    }
}