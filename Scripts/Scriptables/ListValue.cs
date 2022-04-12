using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListValue", menuName = "Scriptables/ListValue")]
public class ListValue : ScriptableObject
{
    public List<GameObject> list = new List<GameObject>();
}
