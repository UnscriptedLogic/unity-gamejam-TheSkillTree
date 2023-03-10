using System;
using System.Collections.Generic;
using UnityEngine;

public class UINavigator : MonoBehaviour
{
    [SerializeField] private List<GameObject> pages;

    private Stack<GameObject> navigator = new();

    public static UINavigator instance;
    public Stack<GameObject> Navigator => navigator;

    [Header("Debug")]
    [SerializeField] private bool enableScriptModify;
    [SerializeField] private int pageToShow;

    private void Awake()
    {
        instance = this;
    }

    public static GameObject Push(string pageName)
    {
        if (PageExists(pageName, out int i))
        {
            instance.pages[i].SetActive(true);
            instance.navigator.Push(instance.pages[i]);
            return instance.pages[i];
        }

        Debug.Log($"The page with name {pageName} is not found");
        return null;
    }

    public static GameObject PushPageWithIndex(int index)
    {
        if (index < instance.pages.Count)
        {
            instance.pages[index].SetActive(true);
            instance.navigator.Push(instance.pages[index]);
            return instance.pages[index];
        }

        Debug.Log($"The page with the index {index} is not found");
        return null;
    }

    public static GameObject Pop()
    {
        if (instance.navigator.Count > 0)
        {
            GameObject page = instance.navigator.Peek();
            instance.navigator.Pop().SetActive(false);
            return page;
        }

        return null;
    }

    public static GameObject PopWithoutDisable()
    {
        if (instance.navigator.Count > 0)
        {
            GameObject page = instance.navigator.Peek();
            instance.navigator.Pop();
            return page;
        }

        return null;
    }

    public static void PopUntil(string pageName)
    {
        while (instance.navigator.Count > 0)
        {
            if (instance.navigator.Peek().name == pageName)
            {
                return;
            }

            Pop();
        }

        Debug.Log($"The page with name {pageName} is not found. Navigation stack is empty");
    }

    public static void PopAndPush(string pageName)
    {
        if (PageExists(pageName, out int index))
        {
            Pop();
            PushPageWithIndex(index);
        }
    }

    public static void PopAll()
    {
        while (instance.navigator.Count > 0)
        {
            Pop();
        }
    }

    public static string GetTopPageName()
    {
        if (instance.navigator.Count > 0)
        {
            return instance.navigator.Peek().name;
        }

        return null;
    }

    public static GameObject GetTopPage()
    {
        if (instance.navigator.Count > 0)
        {
            return instance.navigator.Peek();
        }

        return null;
    }

    public static bool PageExists(string pageName, out int index)
    {
        for (int i = 0; i < instance.pages.Count; i++)
        {
            if (instance.pages[i].name == pageName)
            {
                index = i;
                return true;
            }
        }

        index = -1;
        return false;
    }
}