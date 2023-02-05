using External.CustomSlider;
using System;
using UnityEngine;
using UnscriptedLogic.MathUtils;

[RequireComponent(typeof(BoxCollider2D))]
public class Node : MonoBehaviour, IInspectable, IDamageable
{
    [SerializeField] protected int team;
    [SerializeField] protected float health;
    [SerializeField] protected NodeSO nodeSO;
    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] WorldSpaceCustomSlider healthbar;

    protected float currentHealth;
    protected Node parentNode;

    public string Name => nodeSO.name;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => health;
    public int Team => team;
    public Sprite Icon => nodeSO.Icon;
    public LineRenderer LineRenderer => lineRenderer;


    public virtual void Start()
    {
        currentHealth = health;
        GlobalStatManager.instance.OnNodeDestroyed += OnNodeDamaged;

        healthbar.SetLimits(currentHealth, health);
    }

    protected virtual void OnNodeDamaged(Node node)
    {
        if (node == parentNode)
        {
            Destroy(gameObject);
            GlobalStatManager.instance.NodeDamaged(this);
        }
    }

    public virtual void ModifyHealth(MathLogic.ModificationType modificationType, float amount, int team)
    {
        if (team == this.team) return;

        switch (modificationType)
        {
            case MathLogic.ModificationType.Add:
                currentHealth += amount;
                break;
            case MathLogic.ModificationType.Subtract:
                currentHealth -= amount;
                break;
            case MathLogic.ModificationType.Set:
                currentHealth = amount;
                break;
            case MathLogic.ModificationType.Divide:
                currentHealth /= amount;
                break;
            case MathLogic.ModificationType.Multiply:
                currentHealth *= amount;
                break;
            default:
                Debug.Log("Modification type not found");
                break;
        }

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
            GlobalStatManager.instance.NodeDamaged(this);
            return;
        }

        healthbar.SetValue(currentHealth);
    }

    public void SetLink(Node parent)
    {
        parentNode = parent;
        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, parentNode.transform.position);
    }

    public virtual string GetInspectDesc()
    {
        string desc = nodeSO.name;
        desc += $"\nHP: {currentHealth}/{health}";
        return desc;
    }
}
