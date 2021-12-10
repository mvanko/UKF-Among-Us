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

    private string[] Kurzy = new string[] { "Informa�n� bezpe�nos�", "Znalostn� syst�my", "Programovanie a �dajov� �trukt�ry", "Objektov� technol�gie", "Po��ta�ov� anal�za d�t", "Kryptografia" };
    private string[] infBez = new string[] { "V�eobecn� inform�cie", "Bezpe�nostn� hrozby a rizik�", "Zabezpe�enie sie�ov�ch zariaden�", "Architekt�ra, modely a hodnotenie", "Hrozby a �toky", "Konceptu�lny model" };
    private string[] znalSys = new string[] { "�vod do znalostn�ch syst�mov", "Reprezent�cia znalost�", "Odvodzovac� mechanizmus", "Tvorba expertn�ch syst�mov", "Rozhodovacie stromy", "Strojov� u�enie sa" };
    private string[] progUdaj = new string[] { "Algoritmus a programovac� jazyk", "Riadiace �trukt�ry a �dajov� typy", "Re�azce, Polia", "Streamy", "Cyklus v cykle", "Viacn�sobn� vetvenie" };
    private string[] objektTech = new string[] { "JDBC", "JDBC a reprezent�cia �dajov", "AbstractTableModel", "Servlety", "HTTP a rel�cie", "Session a e-shop" };
    private string[] pocAnalyza = new string[] { "Z�skavanie d�t", "Explora�n� anal�za", "Inferen�n� anal�za", "Z�kladn� �tatistick� met�dy", "Testy rozdelenia", "Vz�ahy medzi premenn�mi" };
    private string[] kryptograf = new string[] { "�vod do kryptografie", "Princ�py z�kladn�ch �ifier", "Matematick� z�klady kryptografie", "Te�ria ��sel v kryptografii", "Aplik�cia te�rie ��sel", "Rozbitie RSA" };

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
        text_kurz.text = "N�jdite kurz: " + find_kurz;
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
        text_kurz.text = "N�jdite lekciu: " + find_lekc;
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
                case "Informa�n� bezpe�nos�":
                    setLekcie(infBez);
                    break;
                case "Znalostn� syst�my":
                    setLekcie(znalSys);
                    break;
                case "Programovanie a �dajov� �trukt�ry":
                    setLekcie(progUdaj);
                    break;
                case "Objektov� technol�gie":
                    setLekcie(objektTech);
                    break;
                case "Po��ta�ov� anal�za d�t":
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
