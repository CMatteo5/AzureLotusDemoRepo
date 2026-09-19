using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDeadTrigger : MonoBehaviour
{
    public RawImage scary;
    private AudioSource source;
    public GameObject target;

    private void Start()
    {
        //source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        InteractAddon redeadInteraction = target.GetComponent<InteractAddon>();
        redeadInteraction.triggerInteraction();
    }

    
}
