using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour, InteractBehavior
{
    public GameObject pointLight;
    public VideoFlashExpandFade videoFlash;
    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onInteract()
    {
        //Debug.Log("Door Interacted");
        videoFlash.PlayVideoWithEffect();
        audioSource.Play();
        StartCoroutine(wait());
        if (pointLight != null)
            pointLight.SetActive(!pointLight.activeSelf);
        this.gameObject.SetActive(false);
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
    }

   
}
