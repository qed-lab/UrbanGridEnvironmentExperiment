using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFollowTarget : MonoBehaviour
{
    [SerializeField] private GameObject targetToFollow;
    [SerializeField] private float speed = 12.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          if (targetToFollow != null)
        {
            // follow the target
            transform.position = Vector3.MoveTowards(transform.position, targetToFollow.transform.position, speed * Time.deltaTime);

            // Calculate the direction from this object to the target
            Vector3 direction = targetToFollow.transform.position - transform.position;

            // Calculate the rotation required to look at the target
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Apply the rotation to the object
            transform.rotation = rotation;
        }
        
    }
}
