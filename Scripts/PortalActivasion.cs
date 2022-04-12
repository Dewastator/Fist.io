using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalActivasion : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            transform.GetChild(0).GetComponent<MeshCollider>().enabled = true;
        }
    }
}
