using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    public Vector3 Position { get; }

    public void Attack();
    public void StopAttack();
}
