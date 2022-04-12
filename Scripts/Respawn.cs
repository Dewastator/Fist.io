using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 12)
        {
            other.gameObject.transform.parent.position = Vector3.zero;
        }
    }
}
