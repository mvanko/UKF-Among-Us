using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToiletMinigame : Minigame
{
    [SerializeField] private Button _clockwiseButton;
    [SerializeField] private Button _antiClockwiseButton;

    [SerializeField] private Image _bulb;
    [SerializeField] private Sprite _bulbUnscrewed;

    private const int moveValue = 15;
    private const int minClicksToScrew = 4;
    private const int maxClicksToScrew = 6;

    private int clicksToScrew = 0;
    private int currentClicks = 0;
    private bool bulbUnscrewed = false;
    private bool blockInputs = false;

    private void OnEnable()
    {
        base.OnEnable();
        _clockwiseButton.onClick.AddListener(ScrewBulb);
        _antiClockwiseButton.onClick.AddListener(UnscrewBulb);
        clicksToScrew = Random.Range(minClicksToScrew, maxClicksToScrew + 1);
    }

    private void OnDisable()
    {
        base.OnDisable();
        _clockwiseButton.onClick.RemoveListener(ScrewBulb);
        _antiClockwiseButton.onClick.RemoveListener(UnscrewBulb);
    }

    private void ScrewBulb()
    {
        if (!blockInputs)
        {
            if (bulbUnscrewed)
            {
                _bulb.transform.position = new Vector3(_bulb.transform.position.x, _bulb.transform.position.y + moveValue, _bulb.transform.position.z);
                currentClicks++;

                if (currentClicks >= clicksToScrew)
                {
                    StartCoroutine(MinigameWon());
                }
            }
            else if (currentClicks > 0)
            {
                _bulb.transform.position = new Vector3(_bulb.transform.position.x, _bulb.transform.position.y + moveValue, _bulb.transform.position.z);
                currentClicks--;
            }
        }
    }
    
    private void UnscrewBulb()
    {
        if (!blockInputs)
        {
            if (!bulbUnscrewed)
            {
                _bulb.transform.position = new Vector3(_bulb.transform.position.x, _bulb.transform.position.y - moveValue, _bulb.transform.position.z);
                currentClicks++;

                if (currentClicks >= clicksToScrew)
                {
                    StartCoroutine(UpdateBulb());
                }
            }
            else if (currentClicks > 0)
            {
                _bulb.transform.position = new Vector3(_bulb.transform.position.x, _bulb.transform.position.y - moveValue, _bulb.transform.position.z);
                currentClicks--;
            }
        }
    }

    private IEnumerator UpdateBulb()
    {
        blockInputs = true;
        bulbUnscrewed = true;
        currentClicks = 0;

        _bulb.gameObject.SetActive(false);
        _bulb.sprite = _bulbUnscrewed;

        yield return new WaitForSeconds(0.8f);

        _bulb.gameObject.SetActive(true);
        blockInputs = false;
        yield return null;
    }

    private IEnumerator MinigameWon()
    {
        blockInputs = true;

        yield return new WaitForSeconds(0.5f);

        _bulb.color = Color.green;

        yield return new WaitForSeconds(1f);

        WinMinigame();

        yield return null;
    }

    public override string Name()
    {
        return GameManager.Instance.MinigameData.toiletTask;
    }
}
