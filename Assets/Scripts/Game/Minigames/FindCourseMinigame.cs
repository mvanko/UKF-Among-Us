using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindCourseMinigame : Minigame
{
    [SerializeField] Text text_kurz;
    [SerializeField] GameObject GamePanel;
    [SerializeField] GameObject[] myObjects;

    private int randomNumber;
    private string find_kurz;
    private string find_lekc;

    private string[] Kurzy = new string[] { "Informaèná bezpeènos", "Znalostné systémy", "Programovanie a údajové štruktúry", "Objektové technológie", "Poèítaèová analıza dát", "Kryptografia" };
    private string[] infBez = new string[] { "Všeobecné informácie", "Bezpeènostné hrozby a riziká", "Zabezpeèenie sieovıch zariadení", "Architektúra, modely a hodnotenie", "Hrozby a útoky", "Konceptuálny model" };
    private string[] znalSys = new string[] { "Úvod do znalostnıch systémov", "Reprezentácia znalostí", "Odvodzovací mechanizmus", "Tvorba expertnıch systémov", "Rozhodovacie stromy", "Strojové uèenie sa" };
    private string[] progUdaj = new string[] { "Algoritmus a programovací jazyk", "Riadiace štruktúry a údajové typy", "Reazce, Polia", "Streamy", "Cyklus v cykle", "Viacnásobné vetvenie" };
    private string[] objektTech = new string[] { "JDBC", "JDBC a reprezentácia údajov", "AbstractTableModel", "Servlety", "HTTP a relácie", "Session a e-shop" };
    private string[] pocAnalyza = new string[] { "Získavanie dát", "Exploraèná analıza", "Inferenèná analıza", "Základné štatistické metódy", "Testy rozdelenia", "Vzahy medzi premennımi" };
    private string[] kryptograf = new string[] { "Úvod do kryptografie", "Princípy základnıch šifier", "Matematické základy kryptografie", "Teória èísel v kryptografii", "Aplikácia teórie èísel", "Rozbitie RSA" };

    void Start()
    {

    }

    private void OnEnable() 
    {
        base.OnEnable();
        for (int i = 0; i < myObjects.Length; i++)
        {
            myObjects[i].GetComponentInChildren<Text>().text = Kurzy[i].ToString();
        }

        randomNumber = Random.Range(0, 5);
        find_kurz = myObjects[randomNumber].GetComponentInChildren<Text>().text;
        text_kurz.text = "Nájdite kurz: " + find_kurz;
        Debug.Log("RANDOM KURZ " + find_kurz);
    }

    IEnumerator WrongAnswer(int button)
    {

        myObjects[button].GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        myObjects[button].GetComponent<Image>().color = Color.white;
    }

    private void setLekcie(string[] array)
    {
        for (int i = 0; i < myObjects.Length; i++)
        {
            myObjects[i].GetComponentInChildren<Text>().text = array[i].ToString();
        }

        randomNumber = Random.Range(0, 5);
        find_lekc = myObjects[randomNumber].GetComponentInChildren<Text>().text;
        text_kurz.text = "Nájdite lekciu: " + find_lekc;
        Debug.Log("RANDOM LEKCIA " + find_lekc);
    }

    public void ButtonClicked(int button)
    {
        string clicked_button = myObjects[button].GetComponentInChildren<Text>().text;
        Debug.Log("CLICKED_BUTTON " + clicked_button);


        if (clicked_button.Equals(find_kurz))
        {
            switch (clicked_button)
            {
                case "Informaèná bezpeènos":
                    setLekcie(infBez);
                    break;
                case "Znalostné systémy":
                    setLekcie(znalSys);
                    break;
                case "Programovanie a údajové štruktúry":
                    setLekcie(progUdaj);
                    break;
                case "Objektové technológie":
                    setLekcie(objektTech);
                    break;
                case "Poèítaèová analıza dát":
                    setLekcie(pocAnalyza);
                    break;
                case "Kryptografia":
                    setLekcie(kryptograf);
                    break;
            }
        }
        else
        {
            StartCoroutine(WrongAnswer(button));
        }

        if (clicked_button.Equals(find_lekc))
        {
            WinMinigame();
        }
    }

    public override string Name()
    {
        return GameManager.Instance.MinigameData.findCourseTask;
    }
}
