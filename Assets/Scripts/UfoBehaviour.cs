using UnityEngine;

public class UfoBehaviour : MonoBehaviour
{
    public float ufoSpeed = 5f;
    public float hoverHeight = 5f; // height at which the ufo hovers above the ground
    public float abductionSpeed = 2f; // how fast to lift the cow
    public GameObject exitZone; // assign this in the inspector to where you want the UFOs to take cows

    private Transform target; // current target for the ufo
    public GameObject abductedCow; // reference to the cow being abducted
    private bool isCowGrabbed = false;

    // UFO states
    private enum UfoState
    {
        Searching,
        MovingTowardsCow,
        AbductingCow,
        CarryingToExit
    }

    private UfoState currentState = UfoState.Searching;

    void Start()
    {
        // try to get the exit zone from the manager if not already set
        if (exitZone == null && ExitZoneManager.Instance != null)
        {
            exitZone = ExitZoneManager.Instance.GetExitZone();
        }

        // Call FindTarget when the UFO is created
        FindTarget();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case UfoState.Searching:
                if (target == null)
                {
                    FindTarget();
                }
                break;

            case UfoState.MovingTowardsCow:
                MoveTowardsCow();
                break;

            case UfoState.AbductingCow:
                AbductCow();
                break;

            case UfoState.CarryingToExit:
                CarryToExit();
                break;
        }
    }

    public void SetTarget(Transform newTarget) // sets the target for the ufo
    {
        target = newTarget;
        currentState = UfoState.MovingTowardsCow;
    }

    void FindTarget()
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Abductable"); // find all objects with the tag "Abductable"
        foreach (GameObject obj in potentialTargets)
        {
            Abductable abductable = obj.GetComponent<Abductable>(); // get the Abductable component of the object
            if (abductable != null && !abductable.isClaimed) // check if the object is not already claimed
            {
                if (abductable.Claim(this.gameObject)) // pass the UFO as the claimer
                {
                    target = obj.transform; // set the target to the object
                    abductedCow = obj; // store reference to the cow
                    currentState = UfoState.MovingTowardsCow; // change state to moving towards cow
                    Debug.Log("UFO " + gameObject.name + " claimed " + obj.name);
                    break; // break the loop after finding the first unclaimed object
                }
            }
        }
    }

    void MoveTowardsCow()
    {
        if (target == null)
        {
            currentState = UfoState.Searching;
            return;
        }

        // position to hover above the cow
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + hoverHeight, target.position.z);

        // move towards that position
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * ufoSpeed * Time.deltaTime;

        // check if we're close enough to the cow to start abducting
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (distanceToTarget < 0.5f)
        {
            currentState = UfoState.AbductingCow;
            Debug.Log("UFO " + gameObject.name + " starting abduction of " + target.name);
        }
    }

    void AbductCow()
    {
        if (target == null)
        {
            currentState = UfoState.Searching;
            return;
        }

        // if we haven't grabbed the cow yet, do so
        if (!isCowGrabbed)
        {
            // get the Rigidbody component
            var cowRb = target.GetComponent<Rigidbody>();
            if (cowRb != null)
            {
                // make it kinematic so it doesn't fall
                cowRb.isKinematic = true;
            }

            // store the cow's initial position before parenting
            Vector3 worldPos = target.position;

            // parent the cow to the UFO
            target.SetParent(transform);

            // reset its position to what it was before parenting to avoid snapping
            target.position = worldPos;

            // mark that it's been grabbed
            isCowGrabbed = true;

            
        }

        // define the beam distance (how close to bring the cow to the UFO)
        float beamDistance = 6f; // adjust this value to control how close the cow gets to the UFO

        // calculate desired position under the UFO in world space
        Vector3 desiredPosition = transform.position + Vector3.down * beamDistance;

        // move the cow smoothly toward that position
        target.position = Vector3.MoveTowards(
            target.position,
            desiredPosition,
            abductionSpeed * Time.deltaTime
        );

        // check if we're close enough to the desired position
        float distanceToTarget = Vector3.Distance(target.position, desiredPosition);
        if (distanceToTarget < 0.1f)
        {
            currentState = UfoState.CarryingToExit;
            Debug.Log($"UFO {name} beginning to carry {target.name}");
        }
    }


    void CarryToExit()
    {
        if (exitZone == null)
        {
            Debug.LogWarning("No exit zone set for UFO " + gameObject.name);
            return;
        }

        // move towards the exit zone
        Vector3 direction = (exitZone.transform.position - transform.position).normalized;
        transform.position += direction * ufoSpeed * Time.deltaTime;

        // check if we're close enough to the exit zone
        float distanceToExit = Vector3.Distance(transform.position, exitZone.transform.position);
        if (distanceToExit < 2f)
        {
            // deliver the cow and destroy both cow and UFO
            if (target != null)
            {
                // detach the cow from the UFO
                target.SetParent(null);

                // call the DeliverToExitZone method to handle the logic of scoring or other events
                Abductable abductable = target.GetComponent<Abductable>();
                if (abductable != null)
                {
                    abductable.DeliverToExitZone();
                }


                // notify the GameManager that a cow has been abducted
                GameManager.Instance.CowAbducted();
                // destroy both the cow and the UFO
                Destroy(target.gameObject);
                Destroy(gameObject); // destroy the UFO immediately
            }
        }
    }



    void OnDestroy()
    {
        // if a cow was grabbed, drop it now:
        if (target != null && isCowGrabbed)
        {
            // 1) un-parent the cow
            target.SetParent(null);

            // 2) restore its physics
            var rb = target.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = false;
        }

        // release the claim on the target when UFO is destroyed
        if (abductedCow != null)
        {
            Abductable abductable = abductedCow.GetComponent<Abductable>();
            if (abductable != null)
            {
                abductable.Release(); // use existing Release() method
            }
        }
    }



}