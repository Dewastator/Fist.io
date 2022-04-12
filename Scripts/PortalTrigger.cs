using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    Transform destination;
    public bool AtDestination;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9) //Portal
        {
            if (!AtDestination)
            {
                destination = other.transform.GetComponent<Portal>().destination;
                other.transform.GetComponent<MeshCollider>().enabled = false;
                transform.parent.position = destination.position;
                AtDestination = true;
            }
            
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9) //Portal
        {
            AtDestination = false;
            other.transform.GetComponent<Portal>().destination.transform.GetComponent<MeshCollider>().enabled = true;
        }

    }



}
