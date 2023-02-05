using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    [SerializeField] private GameObject wonPage;
    [SerializeField] private GameObject lostPage;

    void Start()
    {
        if (GameManager.hasWon)
        {
            wonPage.SetActive(true);
            GameManager.hasWon = false;
        }

        if (GameManager.hasLost)
        {
            lostPage.SetActive(true);
            GameManager.hasLost = false;
        }
    }
}
