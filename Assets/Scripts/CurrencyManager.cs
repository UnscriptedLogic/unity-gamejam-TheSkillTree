using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnscriptedLogic.MathUtils.MathLogic;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private int cash;
    [SerializeField] private TextMeshProUGUI cashTMP;
    public Action<int, int> OnCurrencyModified;

    public static CurrencyManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cashTMP.text = $"Points: {cash}";
    }

    public bool HasCash(float amount) => cash >= amount;

    public void ModifyCash(ModificationType modificationType, int amount)
    {
        switch (modificationType)
        {
            case ModificationType.Add:
                cash += amount;
                break;
            case ModificationType.Subtract:
                cash -= amount;
                break;
            case ModificationType.Set:
                cash = amount;
                break;
            case ModificationType.Divide:
                cash /= amount;
                break;
            case ModificationType.Multiply:
                cash *= amount;
                break;
            default:
                break;
        }

        OnCurrencyModified?.Invoke(amount, cash);
        cashTMP.text = $"Points: {cash}";
    }
}
