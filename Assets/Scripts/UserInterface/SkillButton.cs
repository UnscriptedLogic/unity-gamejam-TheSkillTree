using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SkillButton : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private bool enableByDefault;
    [SerializeField] private int pointCost;

    [SerializeField] private string skillName;

    [TextArea(5, 10)]
    [SerializeField] private string skillDesc;
    [SerializeField] private Sprite skillIcon;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform connectedSkill;
    [SerializeField] private Gradient obtainedcolor;
    [SerializeField] private Gradient unobtainedcolor;
    [SerializeField] private Gradient obtainablecolor;

    [Header("Components")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI skillNameTMP;
    [SerializeField] private TextMeshProUGUI skillDescTMP;
    [SerializeField] private Image skillIconImage;
    [SerializeField] private Image border;

    private CurrencyManager currencyManager;
    private bool isBought;

    public bool IsBought => isBought;

    private void Start()
    {
        if (!Application.isPlaying)
            return;

        skillNameTMP.text = $"{skillName} [{pointCost}]";
        skillDescTMP.text = skillDesc;

        if (enableByDefault)
        {
            border.color = obtainablecolor.Evaluate(0f);
            lineRenderer.colorGradient = obtainablecolor;
            button.interactable = true;
        } else
        {
            border.color = unobtainedcolor.Evaluate(0f);
            lineRenderer.colorGradient = unobtainedcolor;
            button.interactable = false;
        }

        currencyManager = CurrencyManager.instance;
        currencyManager.OnCurrencyModified += OnCurrencyModified;

        button.onClick.AddListener(() =>
        {
            if (isBought) return;

            if (!enableByDefault)
                if (!connectedSkill.GetComponent<SkillButton>().isBought) return;
            
            if (!currencyManager.HasCash(pointCost)) return;

            border.color = obtainedcolor.Evaluate(0f);
            lineRenderer.colorGradient = obtainedcolor;
            isBought = true;

            currencyManager.ModifyCash(UnscriptedLogic.MathUtils.MathLogic.ModificationType.Subtract, pointCost);

            button.interactable = false;
        });
    }

    private void OnCurrencyModified(int arg1, int arg2)
    {
        if (isBought) return;

        border.color = unobtainedcolor.Evaluate(0f);
        lineRenderer.colorGradient = unobtainedcolor;
        button.interactable = false;

        if (!currencyManager.HasCash(pointCost))
        {
            return;
        }

        if (!enableByDefault)
        {
            if (!connectedSkill.GetComponent<SkillButton>().isBought)
                return;
        }

        button.interactable = currencyManager.HasCash(pointCost);

        border.color = obtainablecolor.Evaluate(0f);
        lineRenderer.colorGradient = obtainablecolor;
    }

    private void Update()
    {
        if (Application.isPlaying)
            return;

        name = skillName;
        skillNameTMP.text = $"{skillName} [{pointCost}]";
        skillDescTMP.text = skillDesc;

        if (skillIcon != null)
        {
            skillIconImage.sprite = skillIcon;
        }

        if (connectedSkill != null && lineRenderer != null)
        {
            lineRenderer.positionCount = 3;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, new Vector3(transform.position.x, connectedSkill.position.y, 0f));
            lineRenderer.SetPosition(2, new Vector3(connectedSkill.position.x, connectedSkill.position.y, 0f));
        }

        border.color = obtainedcolor.Evaluate(0f);
        lineRenderer.colorGradient = obtainedcolor;
    }
}
