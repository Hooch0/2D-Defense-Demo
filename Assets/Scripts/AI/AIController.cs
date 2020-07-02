using System;
using System.Collections.Generic;
using UnityEngine;


/*Spawn Clutter
*       Current Idea: Max Number of units within a spawn point.
* 
*				- Timed delay between each unit purchases.
*				- Max number of units within a spawn point.
*				- No Collision between units. 
*					-- This one would require a rework/removal of the avoidance system.
*/

public class AIController : HealthSystem, ITargeter
{

    public Vector3 Velocity { get { return _velocity; } }
    public bool ExitedSpawn { get; private set; }

    public float Speed;
    public float MaxMovementX;
    public float MinMovementX;

    public float Damage;
    public float AttackRate;
    public float AttackRange;

    public float AvoidanceDistance = 1.0f;

    public Targeting TargetingSystem;
    public AIAnimator AnimatorSystem;
    public LayerMask IgnoreAvoidance;


    private Vector3 _velocity;
    private Rigidbody2D _rigidbody;
    private Timer _attackDelay;

    private bool _canMove = true;
    private bool _canAttack = true;

    private bool _hasAttacked = false;

    private Vector3 _direction;

    private void Awake()
    {
        Initalize();
        _rigidbody = GetComponent<Rigidbody2D>();
        AnimatorSystem.Initialize(this);
        TargetingSystem.Initialize(this);

        AnimatorSystem.AttackFinished += OnAttackFinished;

        _attackDelay = new Timer(AttackRate, () => { _canAttack = true; _hasAttacked = false; _attackDelay.Stop(); } );

        //This is here in case a unit is placed without seting the team first.
        _direction = TeamID == Teams.RED ? Vector2.right : Vector2.left;

    }

    private void Start()
    {
        GameManager.Instance.RequestHealthBar(this, 100.0f);
    }

    private void Update()
    {
        if (GameManager.Instance.GameOver == true)
        {
            return;
        }

        _attackDelay.Update(Time.deltaTime);
        TargetingSystem.Update(Time.deltaTime);
        AnimatorSystem.Update(Time.deltaTime);

        //If we have a target and are within range, attack.
        if (TargetingSystem.CurrentTarget != null && TargetingSystem.DistanceToTarget <= AttackRange)
        {
            Attack();
        }

        Movement(GetMovementDirection());
    }
    
    private void Attack()
    {

        //Begin attacking
        if (_canAttack == true)
        {
            Attacking?.Invoke();
            _canAttack = false;

        }
    }

    private void Movement(Vector3 direction)
    {

        //Check if we can move, if have a target, if we have exited the spawn or are within the map.

        if (_canMove == true && TargetingSystem.HasTarget == false && (ExitedSpawn == false || CheckInMap(direction) == true))
        {
            _velocity = direction * Speed * Time.deltaTime;

            
            transform.position += Velocity;
            //If we are within the map, then we have exited the spawn.
            if (CheckInMap(direction) == true)
            {
                ExitedSpawn = true;
            }
            return;
        }
        _velocity.x = 0;

    }

    private bool CheckInMap(Vector3 direction)
    {

        Vector3 pos = transform.position + direction * Speed * Time.deltaTime;

        return pos.x > MinMovementX && pos.x < MaxMovementX;
    }

    private void Avoidance()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (GetMovementDirection() * 0.51f),GetMovementDirection(),AvoidanceDistance,~IgnoreAvoidance);
        if (hit == true)
        {

            AIController ai = hit.transform.root.GetComponent<AIController>();

            if (ai != null && ai.Velocity.x == 0 || ai == null)
            {
                //Either we hit an AI that is not moving, or an object with a collider.
                _canMove = false;
                return;
            }
            return;
        }

        _canMove = true;
    }

    private void OnAttackFinished()
    {
        //Attack animation has finished, deal damage and start attack delay.
        if (_hasAttacked == false)
        {
            _attackDelay.Start();
            if (TargetingSystem.HasTarget == true)
            {
                TargetingSystem.CurrentTarget.Health -= Damage;
            }
            _hasAttacked = true;
        }
    }

    protected override void OnDestroyed()
    {
        //Cleanup
        AnimatorSystem.AttackFinished -= OnAttackFinished;
        Destroy(gameObject);
    }

    public Vector3 GetMovementDirection()
    {
        return _direction;
    }

    public void ChangeMovementDirection(Vector3 direction)
    {
        _direction = direction;
    }

    public void SetTeam(Teams teamid)
    {
        TeamID = teamid;
        _direction = TeamID == Teams.RED ? Vector2.right : Vector2.left;

        //Set graphic direction
        Vector3 scale = transform.localScale;
        scale.x *= -GetMovementDirection().x;
        transform.localScale = scale;

    }


}
