using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class activateObject: MonoBehaviour
{
    public GameObject[] targets;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        int count =0;
        foreach (var target in targets)
        {
            targets[count].SetActive(true);
            count++;
        }
    }
}
