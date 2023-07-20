using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnscriptedLogic;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private int startAmount;
    [SerializeField] private TextMeshProUGUI currencyTMP;
    private IntHandler currencyHandler;

    public IntHandler CurrencyHandler => currencyHandler;

    public static CurrencyManager instance { get; private set; }

    private void Awake()
    {
        currencyHandler = new IntHandler(startAmount);
        currencyHandler.OnModified += CurrencyHandler_OnModified;
        instance = this;
    }

    private void CurrencyHandler_OnModified(object sender, IntHandlerEventArgs e)
    {
        currencyTMP.text = $"Points: {e.currentValue.ToString()}";
    }
}
