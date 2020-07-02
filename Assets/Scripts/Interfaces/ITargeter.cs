using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargeter : ITargetable
{
    Vector3 GetMovementDirection();
}
