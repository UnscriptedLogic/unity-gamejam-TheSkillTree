using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.MathUtils;

public interface IDamageable
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    int Team { get; }

    void ModifyHealth(MathLogic.ModificationType modificationType, float amount, int team);
}
