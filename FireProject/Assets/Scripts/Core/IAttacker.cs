using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Characterizes an object that can attack (be an attack source).
/// </summary>
public interface IAttacker
{
    public Vector3 Position { get; }

    public void Attack();
    public void StopAttack();
}
