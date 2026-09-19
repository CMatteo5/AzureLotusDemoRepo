using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SayHello : MonoBehaviour, InteractBehavior
{
    public void onInteract()
    {
        UnityEngine.Debug.Log("Hello");
    }
}
