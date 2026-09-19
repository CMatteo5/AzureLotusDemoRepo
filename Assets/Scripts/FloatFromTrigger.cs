using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatFromTrigger : MonoBehaviour, InteractBehavior
{
    public GameObject target;
    public GameObject BlockerRef;
    public GameObject RotatingFellas;
    bool begin;
    private void Start()
    {
        BlockerRef.SetActive(false);
    }

    private void Update()
    {
        if (begin)
        {
            StartCoroutine(roatationRedeads());
        }
    }
    public void onInteract()
    {
        //Debug.Log("Coroutine Starting");
        StartCoroutine(goUp());
    }

    private IEnumerator goUp()
    {
        target.GetComponent<PlayerMovment>().gravityOn=false;
        yield return new WaitForSeconds(.01f);
        target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + .5f, target.transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractAddon floatInteraction = GetComponent<InteractAddon>();
        floatInteraction.triggerInteraction();
        BlockerRef.SetActive(true);
        begin = true;
    }
    private IEnumerator roatationRedeads()
    {
        yield return new WaitForSeconds(.01f);
        RotatingFellas.transform.rotation = new Quaternion(RotatingFellas.transform.rotation.x, RotatingFellas.transform.rotation.y + 0.1f, RotatingFellas.transform.rotation.z, RotatingFellas.transform.rotation.w);
    }
}
