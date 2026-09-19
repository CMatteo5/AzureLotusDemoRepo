using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeTitleScene1 : MonoBehaviour, InteractBehavior
{
    public GameObject sound;
    [SerializeField]
    public string SceneName;
    public void onInteract()
    {
        StartCoroutine(StartScene());
    }

    private IEnumerator StartScene()
    {
        sound.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneName);
    }
}
