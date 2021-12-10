using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Foodminigame : Minigame
{
    [SerializeField] private List<GameObject> food;

    [SerializeField] private Transform leftPanel;

    private string[] correctFoodRecipe = { "Bread", "Butter", "Ham", "Cheese" };
    private List<string> currentFoodRecipe = new List<string>();

    private void OnEnable()
    {
        base.OnEnable();
        Clear();
    }

    private void Clear()
    {
        currentFoodRecipe.Clear();

        foreach(GameObject gameObject in food)
        {
            gameObject.SetActive(false);
            gameObject.GetComponent<Image>().color = Color.white;
        }

        for (int i = leftPanel.childCount - 1; i >= 0; i--)
        {
            leftPanel.GetChild(i).SetSiblingIndex(Random.Range(0, leftPanel.childCount - 1));
        }

        for (int i = leftPanel.childCount - 1; i >= 0; i--)
        {
            leftPanel.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void FoodClick(GameObject o)
    {
        foreach(GameObject gameObject in food) 
        { 
            if(o.name == gameObject.name)
            {
                gameObject.SetActive(true);
                gameObject.transform.SetAsLastSibling();
            }
        }

        o.SetActive(false);

        currentFoodRecipe.Add(o.name);

        if(currentFoodRecipe.Count == correctFoodRecipe.Length)
        {
            StartCoroutine(CheckWinOrLose());
        }
    }

    private IEnumerator CheckWinOrLose()
    {
        yield return new WaitForSeconds(0.5f);

        bool win = true;
        for(int i = 0; i < currentFoodRecipe.Count; i++) 
        {
            if(currentFoodRecipe[i] != correctFoodRecipe[i])
            {
                win = false;
                break;
            }
        }

        foreach(GameObject gameObject in food)
        {
            gameObject.GetComponent<Image>().color = win ? Color.green : Color.red;
        }

        yield return new WaitForSeconds(1f);

        if (win)
        {
            WinMinigame();
        }
        else
        {
            Clear();
        }

        yield return null;
    }

    public override string Name()
    {
        return GameManager.Instance.MinigameData.foodTask;
    }
}
