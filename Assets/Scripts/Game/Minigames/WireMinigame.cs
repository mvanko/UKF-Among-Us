using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireMinigame : Minigame
{
    [SerializeField] private List<GameObject> wires;
    [SerializeField] private Transform leftPanel;

    private string[] correctArray = { "Blue", "Green", "Red", "Yellow" };
    private List<string> currentArray = new List<string>();

    private void OnEnable()
    {
        base.OnEnable();
        Clear();
    }

    private void Clear()
    {
        currentArray.Clear();

        foreach(GameObject gameObject in wires)
        {
            gameObject.SetActive(false);
            gameObject.GetComponent<Image>().color = Color.white;
        }

        for (int i = leftPanel.childCount - 1; i >= 0; i--)
        {
            leftPanel.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void LeftClick(GameObject o)
    {
        foreach(GameObject gameObject in wires) 
        { 
            if(o.name == gameObject.name)
            {
                gameObject.SetActive(true);
                gameObject.transform.SetAsLastSibling();
            }
        }

        currentArray.Add(o.name);

        if(currentArray.Count == correctArray.Length)
        {
            StartCoroutine(CheckWinOrLose());
        }
    }

    private IEnumerator CheckWinOrLose()
    {
        yield return new WaitForSeconds(0.5f);

        bool win = true;
        for(int i = 0; i < currentArray.Count; i++) 
        {
            if(currentArray[i] != correctArray[i])
            {
                win = false;
                break;
            }
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
        return GameManager.Instance.MinigameData.engineRoomTask;
    }
}
