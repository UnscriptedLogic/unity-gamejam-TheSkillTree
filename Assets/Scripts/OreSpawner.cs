using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic;

public class OreSpawner : MonoBehaviour
{
    [SerializeField] private GameObject orePrefab;
    [SerializeField] private int amount;
    [SerializeField] private Vector2 randomSize;
    [SerializeField] private Vector2 spawnArea;

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 position = (Vector2)transform.position + RandomLogic.InArea2D(spawnArea);
            float size = RandomLogic.BetFloats(randomSize.x, randomSize.y);

            GameObject ore = Instantiate(orePrefab, position, Quaternion.identity);
            ore.transform.localScale = new Vector3(size, size, 1f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, spawnArea);
    }
}
