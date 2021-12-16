using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceptionMinigame : Minigame
{
    [SerializeField] Text text_findRooms;
    [SerializeField] GameObject[] myObjects;

    private List<string> Rooms = new List<string>();
    private List<string> FindRooms = new List<string>();

    private int found = 0;

    private void OnEnable()
    {
        base.OnEnable();
        for (int i = 0; i < myObjects.Length; i++)
        {
            Rooms.Add(myObjects[i].GetComponentInChildren<Text>().text);
            Debug.Log("ROOM LOADING " + myObjects[i].GetComponentInChildren<Text>().text);
            Debug.Log("ROOM COUNT = " + Rooms.Count);
        }

        for (int i = 0; i < 3; i++)
        {
            int randomNumber = Random.Range(0, Rooms.Count-1);
            FindRooms.Add(Rooms[randomNumber]);
            Debug.Log("ADDED " + Rooms[randomNumber]);
            Rooms.RemoveAt(randomNumber);
            
        }
        text_findRooms.text = "Nájdite: " + FindRooms[0] + ", " + FindRooms[1] + ", " + FindRooms[2];

    }
    IEnumerator RightAnswer()
    {
        yield return new WaitForSeconds(0.5f);
        WinMinigame();
    }

    IEnumerator WrongAnswer(int button)
    {
        myObjects[button].GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        myObjects[button].GetComponent<Image>().color = Color.black;
    }

    public void RoomClicked(int button)
    {
        string clicked_room = myObjects[button].GetComponentInChildren<Text>().text;
        Debug.Log("CLICKED_ROOM " + clicked_room);

        if (FindRooms.Contains(clicked_room))
        {
            found++;
            myObjects[button].GetComponent<Image>().color = Color.green;
            Debug.Log("FOUND= " + found);
        } 
        else 
        {
            StartCoroutine(WrongAnswer(button));
        }

        if (found == 3)
        {
            StartCoroutine(RightAnswer());
        }
    }

    public override string Name()
    {
        return GameManager.Instance.MinigameData.receptionTask;
    }
}
