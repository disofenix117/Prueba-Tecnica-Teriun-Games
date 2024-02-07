using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    PlayerMovement Pj;
    // Start is called before the first frame update
    void Start()
    {
        Pj = GetComponent<PlayerMovement>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("floor"))
        {

            Pj.GroundedState(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("floor"))
        {
            Pj.GroundedState(false);

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("floor"))
        {
            Pj.GroundedState(true);

        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            Pj.GroundedState(true);

        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            Pj.GroundedState(false);

        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            Pj.GroundedState(true);

        }

    }

}
