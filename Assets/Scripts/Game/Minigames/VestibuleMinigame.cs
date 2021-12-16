using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VestibuleMinigame : Minigame
{
    [SerializeField] private Button _takeCard;
    [SerializeField] private Image _card;
    [SerializeField] private Sprite _cardTaken;

    private bool blockInputs = false;

    private void OnEnable()
    {
        base.OnEnable();
        _takeCard.onClick.AddListener(TakeCard);
    }

    private void OnDisable()
    {
        base.OnDisable();
        _takeCard.onClick.RemoveListener(TakeCard);
    }

    private void TakeCard()
    {
        if (!blockInputs)
        {
            StartCoroutine(MinigameWon());
        }
    }

    private IEnumerator MinigameWon()
    {
        blockInputs = true;
        _card.rectTransform.SetAsLastSibling();
        _card.color = Color.green;

        for (int i = 0; i < 25; i++)
        {
            yield return new WaitForSeconds(0.05f);
            _card.transform.position = new Vector3(_card.transform.position.x + 6, _card.transform.position.y - 5, _card.transform.position.z);
        }


        yield return new WaitForSeconds(0.5f);

        _card.gameObject.SetActive(false);
        _card.sprite = _cardTaken;
        _card.transform.position = new Vector3(_card.transform.position.x, _card.transform.position.y + 100, _card.transform.position.z);
        _card.transform.localScale = new Vector3(_card.transform.localScale.x + 2, _card.transform.localScale.y + 2, _card.transform.localScale.z);
        _card.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        WinMinigame();

        yield return null;
    }

    public override string Name()
    {
        return GameManager.Instance.MinigameData.vestibul1Task;
    }


}
