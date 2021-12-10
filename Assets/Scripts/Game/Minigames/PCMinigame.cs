using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PCMinigame : Minigame
{
    [SerializeField] private PCMinigameTextItem textItemPrototype;

    [SerializeField] private Transform middlePanelFirstRow;
    [SerializeField] private Transform middlePanelSecondRow;
    [SerializeField] private Transform bottomPanel;

    private const int maxNumberOfTextsPerRow = 3;
    private const int NumberOfTexts = 5;
    private const string correctText = "Dobrý deò, Chcel by som sa spýta, kedy sa budú vypláca prospechové a motivaèné štipendia. Vopred ïakujem za odpoveï";
    private string currentText = string.Empty;

    private void OnEnable()
    {
        base.OnEnable();
        Clear();
    }

    private void Clear()
    {
        currentText = string.Empty;

        for (int i = middlePanelFirstRow.childCount - 1; i >= 0; i--)
        {
            Destroy(middlePanelFirstRow.GetChild(i).gameObject);
        }

        for (int i = middlePanelSecondRow.childCount - 1; i >= 0; i--)
        {
            Destroy(middlePanelSecondRow.GetChild(i).gameObject);
        }

        for (int i = bottomPanel.childCount - 1; i >= 0; i--)
        {
            bottomPanel.GetChild(i).SetSiblingIndex(Random.Range(0, bottomPanel.childCount - 1));
        }

        for (int i = bottomPanel.childCount - 1; i >= 0; i--)
        {
            bottomPanel.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void ItemClicked(Text text)
    {
        Transform transform = middlePanelFirstRow.childCount < maxNumberOfTextsPerRow ? middlePanelFirstRow : middlePanelSecondRow;
        Instantiate(textItemPrototype, transform).Setup(text.text);
        text.gameObject.transform.parent.gameObject.SetActive(false);

        currentText += text.text;

        if(middlePanelFirstRow.childCount + middlePanelSecondRow.childCount == NumberOfTexts)
        {
            StartCoroutine(CheckWinOrLose());
        }
    }

    private IEnumerator CheckWinOrLose()
    {
        yield return new WaitForSeconds(0.5f);

        bool win = currentText == correctText;

        for (int i = middlePanelFirstRow.childCount - 1; i >= 0; i--)
        {
            middlePanelFirstRow.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = win ? Color.green : Color.red;
        }

        for (int i = middlePanelSecondRow.childCount - 1; i >= 0; i--)
        {
            middlePanelSecondRow.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = win ? Color.green : Color.red;
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
        return GameManager.Instance.MinigameData.pcTask;
    }
}
