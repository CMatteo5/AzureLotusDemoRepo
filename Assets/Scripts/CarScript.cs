using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour, InteractBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void onInteract()
    {
        StartCoroutine(movement());
        //gameObject.SetActive(false);
    }

    private IEnumerator movement()
    {
        while (transform.position.y < .76)
        {
            //source.Play();
            //scary.enabled = true;

            yield return new WaitForSeconds(.01f);
            transform.position = new Vector3(transform.position.x , transform.position.y + .005f, transform.position.z);
            //scary.enabled = false;
            //source.Stop();
        }
    }
}
