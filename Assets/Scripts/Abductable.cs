using UnityEngine;

public class Abductable : MonoBehaviour
{
    public bool isClaimed = false; //is the object taken by the ufo
    private GameObject claimedBy; //which ufo has claimed this object
    private Vector3 originalPosition; //original position of the object
    private bool wasAbducted = false; //was the object abducted

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position; //store the original position
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Claim(GameObject claimer) //claims the object for the ufo
    {
        if (isClaimed) return false; //if the object is already claimed, return false
        isClaimed = true; //set the object as claimed
        claimedBy = claimer; //store which ufo claimed this
        return true; //return true to indicate that the object was successfully claimed
    }

    public void Release() //releases the claim on the object
    {
        isClaimed = false;
        claimedBy = null;
        wasAbducted = true;
    }

    public bool WasAbducted() //returns if the object was abducted
    {
        return wasAbducted;
    }

    public void ResetPosition() //resets the object to its original position
    {
        transform.position = originalPosition;
        wasAbducted = false;
    }

    // Optional: Add scoring or other game mechanics here
    public void DeliverToExitZone()
    {
        // Add points or trigger game events when a cow is successfully delivered
        Debug.Log(gameObject.name + " was successfully delivered to the exit zone!");

        // You could increase a score here
        // ScoreManager.instance.AddPoints(10);

        // Or trigger a game event
        // GameManager.instance.CowDelivered();
    }

    public void ReleaseClaim()
    {
        isClaimed = false;
        claimedBy = null;
        // Don't set wasAbducted = true since it wasn't actually abducted
    }
}