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
        currentText = string.Empty;

        for (int i = middlePanelFirstRow.childCount-1; i >= 0; i--)
        {
            Destroy(middlePanelFirstRow.GetChild(i).gameObject);
        }

        for (int i = middlePanelSecondRow.childCount - 1; i >= 0; i--)
        {
            Destroy(middlePanelSecondRow.GetChild(i).gameObject);
        }

        for (int i = bottomPanel.childCount - 1; i >= 0; i--)
        {
            Transform child = bottomPanel.GetChild(i);
            child.SetSiblingIndex(Random.Range(0, bottomPanel.childCount - 1));
            child.gameObject.SetActive(true);
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

        if(currentText == correctText)
        {
            Debug.Log("WIN!");

            for (int i = middlePanelFirstRow.childCount - 1; i >= 0; i--)
            {
                middlePanelFirstRow.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
            }

            for (int i = middlePanelSecondRow.childCount - 1; i >= 0; i--)
            {
                middlePanelSecondRow.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
            }

            yield return new WaitForSeconds(1f);

            WinMinigame();
        }
        else
        {
            Debug.Log("LOSE!");

            for (int i = middlePanelFirstRow.childCount - 1; i >= 0; i--)
            {
                middlePanelFirstRow.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
            }

            for (int i = middlePanelSecondRow.childCount - 1; i >= 0; i--)
            {
                middlePanelSecondRow.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
            }

            yield return new WaitForSeconds(1f);

            CloseMinigame();
        }

        yield return null;
    }
}
