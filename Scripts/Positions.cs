using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions : MonoBehaviour
{
    [SerializeField]
    public List<Transform> positions = new List<Transform>();
    public static Positions Instance;
    public List<Transform> respawnPoints = new List<Transform>();

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public Transform GetPosition(Transform caller)
    {
        var randTransform = positions[Random.Range(0, positions.Count - 1)];

        if (randTransform != caller)
            return randTransform;
        else
        {
            while (randTransform == caller)
            {
                randTransform = positions[Random.Range(0, positions.Count - 1)];
            }
        }
            
        return randTransform;
    }

    public void RemoveItem(Transform item)
    {
        positions.Remove(item);
    }

    //public void AddItem(Transform item)
    //{
    //    if (positions.Contains(item))
    //        RemoveItem(item);
    //    positions.Add(item);
    //}

    
}
