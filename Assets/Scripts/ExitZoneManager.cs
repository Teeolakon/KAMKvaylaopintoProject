using UnityEngine;

public class ExitZoneManager : MonoBehaviour
{
    // Singleton instance
    public static ExitZoneManager Instance { get; private set; }

    [SerializeField] private GameObject exitZone;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetExitZone()
    {
        return exitZone;
    }
}