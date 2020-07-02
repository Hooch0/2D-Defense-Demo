using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    public UnitPurchaseInfoSO PurchaseInfo;

    //Example: If cost of swordman is 200 for the player, it will be 250 for the AI
    public float DifficultyCost = 1.25f; 

    public int MaxUnitsInSpawnArea = 5;
    public SpawnArea SpawnAreaControl;
    public Transform Spawn;

    public int StartGold = 600;
    public int CurrentGold = 0;
    public int MaxGold = 999999;
    public float GoldRate = 4;
    public int GoldGain = 100;

    private Timer _gainTimer;

     private void Awake()
    {
        CurrentGold = StartGold;

        _gainTimer = new Timer(GoldRate, () => { CurrentGold += GoldGain; _gainTimer.Stop(); _gainTimer.Start(); });
        _gainTimer.Start();
    }


    private void Update()
    {
        _gainTimer.Update(Time.deltaTime);

        //Attempt to spawn untis each frame.
        //Just a simple way to have a spawner.
        SpawnUnit();
    }

    private void SpawnUnit()
    {
        int cost = Convert.ToInt32(PurchaseInfo.Cost*DifficultyCost);
        if (SpawnAreaControl.CurrentUnitsInSpawn < MaxUnitsInSpawnArea && CurrentGold >= cost)
        {
            Instantiate(PurchaseInfo.UnitPrefab,Spawn.position,Quaternion.identity).GetComponent<AIController>().SetTeam(Teams.BLUE);
            CurrentGold -= cost;
        }
    }

}
