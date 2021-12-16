using UnityEngine;

public class LevelSpawnPionts : MonoBehaviour
{
    public static LevelSpawnPionts Instance;

    public Transform[] spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
