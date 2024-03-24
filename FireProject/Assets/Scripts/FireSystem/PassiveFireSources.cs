using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switch fire source depending on DamageType.
/// <para>This is just a glorified gameobject enabler/disabler - will probably change this later.</para>
/// <para>Must be initialized using Initialize()</para>
/// </summary>
public class PassiveFireSources : MonoBehaviour
{
    public FireSource passiveLevel1, passiveLevel2, passiveLevel3;
    public Transform rescaleTransform;
    public Vector3 unlitScale = Vector3.one;
    public Vector3 litScale = Vector3.one;
    public Vector3 overdriveAddition = Vector3.one;
    public BasicSpritePlayer splayer;

    public DamageType CurrentState { get; private set; }

    private float targetT;
    private float currentT;
    private float overdrive;

    public void Initialize(IAttacker attacker, IFlammable damagable)
    {
        passiveLevel1.Initialize(attacker, damagable);
        passiveLevel2.Initialize(attacker, damagable);
        passiveLevel3.Initialize(attacker, damagable);
    }
    private void DisableAll()
    {
        passiveLevel1.SetActive(false);
        passiveLevel2.SetActive(false);
        passiveLevel3.SetActive(false);
    }

    private void Update()
    {
        float t = Time.deltaTime * (targetT > currentT ? 3f : 1f);
        currentT = Mathf.Lerp(currentT, targetT, t);
        overdrive = Mathf.Lerp(overdrive, 0, Time.deltaTime * 1f);

        splayer.delay = Mathf.Lerp(0.075f, .3f, t);

        Vector3 overdriveContribution = Vector3.Lerp(Vector3.zero, overdriveAddition, overdrive);

        rescaleTransform.localScale = Vector3.LerpUnclamped(unlitScale, litScale, currentT) +overdriveContribution;
    }

    /// <summary>
    /// Switch to the input damage type.
    /// </summary>
    /// <param name="type">None means to deactivate everything, otherwise specify the type.</param>
    public void Switch(DamageType type)
    {
        if (!Utilities.IsFireType(type)) return;

        if(type== DamageType.ClearFire)
        {
            DisableAll();
        }
        else if(type== DamageType.FirePassive_Lvl1)
        {
            DisableAll();
            passiveLevel1.SetActive(true);
        }
        else if (type == DamageType.FirePassive_Lvl2)
        {
            DisableAll();
            passiveLevel2.SetActive(true);
        }
        else if (type == DamageType.FirePassive_Lvl3)
        {
            DisableAll();
            passiveLevel3.SetActive(true);
        }
        CurrentState = type;
    }

    public void Rescale(float t)
    {
        targetT = t;
    }
    public void SpreadScale(float additionalT)
    {
        currentT += additionalT;
        overdrive = 1;
    }

    /// <summary>
    /// Spreads the active fire.
    /// </summary>
    public void Spread()
    {
        DamageType type = CurrentState;

        if (!Utilities.IsFireType(type)) return;

        if (type == DamageType.FirePassive_Lvl1)
        {
            passiveLevel1.Damage(1);
        }
        else if (type == DamageType.FirePassive_Lvl2)
        {
            passiveLevel2.Damage(1);
        }
        else if (type == DamageType.FirePassive_Lvl3)
        {
            passiveLevel3.Damage(1);
        }
    }
}
