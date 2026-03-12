using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("UI Icons")]
    public RawImage SafeIcon;
    public RawImage ChasedIcon;

    [Header("Lighting")]
    public Light ChasedLight;

    [Header("AI")]
    public AITarget enemyAI;

    void Start()
    {
        SetSafe();
    }

    void Update()
    {
        if (enemyAI != null && enemyAI.isChasing)
        {
            SetChased();
        }
        else
        {
            SetSafe();
        }
    }

    void SetChased()
    {
        SafeIcon.enabled = false;
        ChasedIcon.enabled = true;
        ChasedLight.enabled = true;
    }

    void SetSafe()
    {
        SafeIcon.enabled = true;
        ChasedIcon.enabled = false;
        ChasedLight.enabled = false;
    }
}