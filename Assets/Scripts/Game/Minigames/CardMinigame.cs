using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMinigame : Minigame
{
    [SerializeField] private Button _unlock;
    [SerializeField] private Image _card;
    [SerializeField] private Image _light;

    private bool blockInputs = false;

    private void OnEnable()
    {
        base.OnEnable();
        _unlock.onClick.AddListener(Unlock);
        _card.gameObject.SetActive(false);
        _light.color = Color.red;
    }

    private void OnDisable()
    {
        base.OnDisable();
        _unlock.onClick.RemoveListener(Unlock);
    }

    private void Unlock()
    {
        if (!blockInputs)
        {
            StartCoroutine(MinigameWon());
        }
    }

    private IEnumerator MinigameWon()
    {
        blockInputs = true;
        _card.gameObject.SetActive(true);

        for (int i = 0; i < 45; i++)
        {
            yield return new WaitForSeconds(0.025f);
            _card.transform.position = new Vector3(_card.transform.position.x - 5, _card.transform.position.y + 5, _card.transform.position.z);
        }

        yield return new WaitForSeconds(1f);

        _light.color = Color.green;

        yield return new WaitForSeconds(2f);

        WinMinigame();

        yield return null;
    }

    public override string Name()
    {
        return GameManager.Instance.MinigameData.vestibul2Task;
    }
}
