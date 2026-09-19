using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour, InteractBehavior
{
    [SerializeField]
    public string SceneName;
    public void onInteract()
    {
        SceneManager.LoadScene(SceneName);
    }
}
