using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool GameOver { get; private set; } = false;

    public Transform Spawn;
    public UnitPurchaseInfoSO PurchaseInfo;
    public int MaxUnitsInSpawnArea = 5;
    public SpawnArea SpawnAreaControl;


    [Header("UI Elements")]
    public Healthbar HealthbarPrefab;
    public RectTransform MainCanvas;
    public GameObject WinScreenPanel;
    public GameObject LoseScreenPanel;
    public Text GoldUI;
    public Text NameText;
    public Text CostText;
    public Image PurchaseImage;

    [Header("Economy Settings")]
    public int StartGold = 600;
    public int CurrentGold = 0;
    public int MaxGold = 999999;
    public float GoldRate = 4;
    public int GoldGain = 100;

    private Timer _gainTimer;

    private List<AIController> _playerUnits = new List<AIController>();

    private void Awake()
    {
        CurrentGold = StartGold;

        _gainTimer = new Timer(GoldRate, () => { CurrentGold += GoldGain; _gainTimer.Stop(); _gainTimer.Start(); });
        _gainTimer.Start();

        SetPurchaseInfo();
    }

    private void OnEnable()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        _gainTimer.Update(Time.deltaTime);
        GoldUI.text = "Gold: " + CurrentGold.ToString();
    }

    private void SetPurchaseInfo()
    {
        NameText.text = "Buy " + PurchaseInfo.Name;
        CostText.text = "Cost: " + PurchaseInfo.Cost.ToString();
        PurchaseImage.sprite = PurchaseInfo.Image;
    }

    public void RequestHealthBar(ITargetable user, float heightOffset)
    {
    
        //Health bar could be stored in an object pool for increased performance, but the scale of this game is small so it is not needed.
        Healthbar bar = Instantiate(HealthbarPrefab,Vector3.zero,Quaternion.identity,MainCanvas.transform).GetComponent<Healthbar>();
        bar.SetHealthBar(user,MainCanvas,heightOffset);
    }

    public void WinScreen()
    {
        GameOver = true;
        WinScreenPanel.SetActive(true);
    }

    public void LoseScreen()
    {
        GameOver = true;

        LoseScreenPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void BuySwordman()
    {
        if (SpawnAreaControl.CurrentUnitsInSpawn < MaxUnitsInSpawnArea && CurrentGold >= PurchaseInfo.Cost)
        {
            AIController ai = Instantiate(PurchaseInfo.UnitPrefab,Spawn.position,Quaternion.identity).GetComponent<AIController>();
            ai.SetTeam(Teams.RED);

            //When the AI is destroyed, remove it from the players list of units.
            ai.Destroyed += () => { _playerUnits.Remove(ai); };

            _playerUnits.Add(ai);
            ai.name = PurchaseInfo.name + " : [" + _playerUnits.Count +"]";

            CurrentGold -= PurchaseInfo.Cost;
        }
    }

    //Movement Control
    public void MoveBackwards()
    {
        foreach(AIController units in _playerUnits)
        {
            //Move towards friendly tower.
            if (units.ExitedSpawn == true)
            {
                units.ChangeMovementDirection(Vector3.left);
            }
        }
    }

    public void StopMovement()
    {
        foreach(AIController units in _playerUnits)
        {
            //Stop Movement
            if (units.ExitedSpawn == true)
            {
                units.ChangeMovementDirection(Vector3.zero);
            }
        }
    }

    public void MoveForwards()
    {
        foreach(AIController units in _playerUnits)
        {
            //Move towards enemy tower.
            if (units.ExitedSpawn == true)
            {
                units.ChangeMovementDirection(Vector3.right);
            }
        }
    }

}

public enum Teams { RED, BLUE }
