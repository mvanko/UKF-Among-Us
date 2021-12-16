using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class WaitingRoomLight : MonoBehaviour
{
    Light2D playerLight;

    private float innerRadius_old = 0.1f;
    private float innerRadius_new = 35f;
    private float outerRadius_old = 23f;
    private float outerRadius_new = 50f;
    private string sceneName;

    private void Awake()
    {
        playerLight = GetComponent<Light2D>();
        sceneName = SceneManager.GetActiveScene().name;

    }

    private void OnEnable()
    {
        if (sceneName == "Waiting Room")
        {
            playerLight.pointLightInnerRadius = innerRadius_new;
            playerLight.pointLightOuterRadius = outerRadius_new;
        }
        else
        {
            playerLight.pointLightInnerRadius = innerRadius_old;
            playerLight.pointLightOuterRadius = outerRadius_old;
        }
    }
}
