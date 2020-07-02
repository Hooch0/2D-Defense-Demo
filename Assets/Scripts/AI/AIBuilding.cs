using System;
using System.Collections.Generic;
using UnityEngine;

public class AIBuilding : HealthSystem
{
    public GameObject BuiltTower;
    public GameObject DestroyedTower;

    private void Awake()
    {
        Initalize();
    }

     private void Start()
    {
        GameManager.Instance.RequestHealthBar(this, 125.0f);
    }

    protected override void OnDestroyed()
    {
        //This object is not destroyed when they have 0 health, instead they switch graphics.

        IsTargetable = false;
        BuiltTower.SetActive(false);
        DestroyedTower.SetActive(true);

        if (TeamID == Teams.RED)
        {
            GameManager.Instance.LoseScreen();
        }

        if (TeamID == Teams.BLUE)
        {
            GameManager.Instance.WinScreen();
        }

    }
}
