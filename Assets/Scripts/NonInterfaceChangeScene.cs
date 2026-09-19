using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonInterfaceChangeScene : MonoBehaviour
{
    [SerializeField]
    public string SceneName;
    public void OnTriggerEnter()
    {
        SceneManager.LoadScene(SceneName);
    }
}
