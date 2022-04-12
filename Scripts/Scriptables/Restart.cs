using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName ="Restart", menuName = "Scriptables/Restart")]
public class Restart : ScriptableObject
{
    public void LoadScene(int index) => SceneManager.LoadScene(index);

}
