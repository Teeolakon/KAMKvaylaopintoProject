using UnityEngine;

public class ExitZone : MonoBehaviour
{
    public int deliveredCows = 0; // counter for delivered cows

    // This function is called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is abductable
        if (other.CompareTag("Abductable"))
        {
            Abductable abductable = other.GetComponent<Abductable>();
            if (abductable != null)
            {
                // Process the delivery
                deliveredCows++;
                Debug.Log("Cow delivered to exit zone! Total: " + deliveredCows);

                // Call the delivery method on the abductable
                abductable.DeliverToExitZone();

                // You can decide what happens with the cow:
                // Option 1: Destroy it
                // Destroy(other.gameObject);

                // Option 2: Deactivate it
                other.gameObject.SetActive(false);

                // Option 3: Move it somewhere else (e.g., reset position but keep inactive)
                // other.transform.position = new Vector3(0, -100, 0);
            }
        }
    }
}