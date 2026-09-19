using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Code generated from ChatGPT on 5/4/25
public class InteractAddon : MonoBehaviour
{
    private InteractBehavior interface1;

    void Start()
    {
        interface1 = GetComponent<InteractBehavior>();
    }
    public void triggerInteraction()
    {
        if (interface1 != null)
        {
            interface1.onInteract();
        }
    }

}
