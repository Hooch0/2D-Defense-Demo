using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour, ITargetable
{
    public Action Destroyed { get; set; }
    public Action<float> Damaged { get; set; }
    public Action Attacking { get; set; }


    public Teams TeamID { get { return _teamID; } set { _teamID = value; }}
    public bool IsTargetable { get; set; } = true;
    public float MaxHealth { get { return _maxHealth; } }
    public float Health  
    { 
        get 
        { 
            return _health; 
        } 
        set
        {
            if (value < _health)
            {
                Damaged?.Invoke(value);
                OnDamaged();
            }

            if (value <= 0)
            {
                _health = 0;
                Destroyed?.Invoke();

                Cleanup();
                OnDestroyed();
                return;
            }


            _health = value;
        } 
    }

    private float _health = 0;

    [SerializeField]
    private Teams _teamID = Teams.RED;
    
    [SerializeField]
    private float _maxHealth = 0;


    protected void Initalize()
    {
         Health = _maxHealth;
    }

    protected virtual void OnDestroyed()
    {

    }

    protected virtual void OnDamaged()
    {

    }

    public Transform GetTransform()
    {
        return transform;
    } 

    private void Cleanup()
    {
        Destroyed = null;
        Damaged = null;
        Attacking = null;
    }
}
