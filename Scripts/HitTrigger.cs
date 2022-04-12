using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{
    public GameEvent OnAIHitted, OnPlayerHitted;
    GameObject ai, player;
    int damage;
    private void Awake()
    {
        if (transform.parent.gameObject.layer == 10)
            ai = transform.parent.parent.gameObject;
        else
            player = transform.parent.parent.gameObject;

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "AI")
        {
            if(transform.parent.gameObject.layer == 10)                
                damage = ai.GetComponent<AIController>().attackDamage;
            else
                damage = player.GetComponent<Player>().attackDamage;

            other.gameObject.GetComponent<AIController>().TakeDamage(damage);
        }
        if (other.gameObject.layer == 7)
        {
            int damage = ai.GetComponent<AIController>().attackDamage;
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
