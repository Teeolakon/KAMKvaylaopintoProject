using UnityEngine;

public class UfoTriggerZone : MonoBehaviour
{
    private bool hasTriggered = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered &&  other.CompareTag("Player"))
        {
            hasTriggered = true;
            UfoSpawner spawner = FindAnyObjectByType<UfoSpawner>();
            if (spawner != null)
            {
                spawner.ActivateSpawning();
            }
        }
    }
}
