using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed;
    public bool isPlayers;
    public GameEvent OnAIHitted, OnPlayerHitted;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 && isPlayers)
            return;
        if (other.gameObject.layer == 10 && !isPlayers)
            return;

        if (other.gameObject.layer == 7 && !isPlayers)
        {
            OnPlayerHitted.Raise();
            Destroy(gameObject);

        }
        if (other.gameObject.tag == "AI")
        {
            OnAIHitted.Raise();
            Destroy(gameObject);

        }

    }
}
