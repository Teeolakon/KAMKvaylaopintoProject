using UnityEngine;

public class PhotoShooter : MonoBehaviour
{


    void Awake()
    {
        Debug.Log("PhotoShooter attached to: " + gameObject.name);
    }

    public float shootRange = 100f;
    public float shootCooldown = 1f;

    private float nextFireTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }




  

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            ShootPhoto();
            nextFireTime = Time.time + shootCooldown;
        }
        
    }

    void ShootPhoto()
    {

            // Create a ray starting at the camera’s position, going forward
            Ray ray = new Ray(transform.position, transform.forward);

            // Draw the ray in Scene view for 1 second in red
            Debug.DrawRay(transform.position, transform.forward * shootRange, Color.red, 1f);

            // Perform the raycast
            if (Physics.Raycast(ray, out RaycastHit hit, shootRange))
            {
            // If the collider we hit has the tag "UFO", destroy its GameObject
            if (hit.collider.CompareTag("UFO"))
            {
                
                // 1) Get the UFO’s behaviour component
                var ufoRoot = hit.collider.transform.root;
                var ufo = ufoRoot.GetComponent<UfoBehaviour>();

                // 2) If that UFO has a cow attached, drop it:
                if (ufo != null && ufo.abductedCow != null)
                {
                    // Unparent it:
                    ufo.abductedCow.transform.SetParent(null);

                    // Turn physics back on so it falls:
                    var cowRb = ufo.abductedCow.GetComponent<Rigidbody>();
                    if (cowRb != null) cowRb.isKinematic = false;
                }

                // 3) Now destroy just the UFO:
                Destroy(ufoRoot.gameObject);

            }

        }
    }

    }


