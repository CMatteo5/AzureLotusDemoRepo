using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroInfor : MonoBehaviour
{
    public GameObject Cover;
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject text4;
    public GameObject controlText;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartScene()
    {
        Cover.SetActive(true);
        text1.SetActive(true);
        yield return new WaitForSeconds(time);
        text1.SetActive(false);
        text2.SetActive(true);
        yield return new WaitForSeconds(time);
        text2.SetActive(false);
        text3.SetActive(true);
        yield return new WaitForSeconds(time);
        text3.SetActive(false);
        text4.SetActive(true);
        yield return new WaitForSeconds(time);
        text4.SetActive(false);
        Cover.SetActive(false);
        controlText.SetActive(true);
        yield return new WaitForSeconds(time);
        controlText.SetActive(false) ;
    }

    }
