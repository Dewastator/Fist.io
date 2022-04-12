using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject player;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpawnPlayer()
    {
        player.transform.position = Positions.Instance.respawnPoints[Random.Range(0, Positions.Instance.respawnPoints.Count - 1)].position;
        player.SetActive(true);
    }
}
