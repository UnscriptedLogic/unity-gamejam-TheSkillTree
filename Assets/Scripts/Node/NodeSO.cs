using UnityEngine;

[CreateAssetMenu(fileName = "New NodeSO", menuName = "ScriptableObjects/New Node")]
public class NodeSO : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private string buildableLayer;
    [SerializeField] private int cost;

    public Sprite Icon => icon;
    public GameObject NodePrefab => nodePrefab;
    public string BuildableLayer => buildableLayer;
    public int Cost => cost;
}
