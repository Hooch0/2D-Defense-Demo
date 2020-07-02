using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Targeting
{

    public ITargetable CurrentTarget { get; private set; }
    public bool HasTarget { get { return CurrentTarget != null && CurrentTarget.Equals(null) == false; }}
    public float DistanceToTarget { get { return HasTarget == false ? 0 :  _distance.distance; } }
    public float TargetRange;

    private ITargeter _user;
    private RaycastHit2D _distance;

    public void Initialize(ITargeter user)
    {
        _user = user;
    }

    public void Update(float deltaTime)
    {

        if (HasTarget == false)
        {
            CheckForTarget();
            return;
        }

        if (DistanceToTarget > TargetRange)
        {

            CurrentTarget.Destroyed -= OnTargetDestroyed;
            CurrentTarget = null;
        }

    }

    private void CheckForTarget()
    {

        //Check all contacts within the TargetRange
        RaycastHit2D[] hit = Physics2D.RaycastAll(_user.GetTransform().position,_user.GetMovementDirection(), TargetRange );

        foreach(RaycastHit2D hits in hit)
        {
            ITargetable target; 

            bool hasTarget = hits.transform.root.TryGetComponent<ITargetable>( out target);
            
            //If the target is not targetable, the check the next
            if (hasTarget == false)
            {
                continue;
            }
            
            //If the target is targetable and is not on the same team, then set as the current target.
            if (target.IsTargetable == true && target.TeamID != _user.TeamID )
            {

                _distance = hits;
                CurrentTarget = target;
                CurrentTarget.Destroyed += OnTargetDestroyed;
                return;
            }
        }
    }

    private void OnTargetDestroyed()
    {
        CurrentTarget.Destroyed -= OnTargetDestroyed;
        CurrentTarget = null;
    }

}
