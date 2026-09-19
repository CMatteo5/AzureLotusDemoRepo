using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class RedeadScare : MonoBehaviour, InteractBehavior
{
    // Start is called before the first frame update
    public GameObject target;
    public void onInteract()
    {
            StartCoroutine(movement());
        //gameObject.SetActive(false);
    }

    private IEnumerator movement()
    {
        while (transform.position.x > -10)
        {
            //source.Play();
            //scary.enabled = true;
            
            yield return new WaitForSeconds(.01f);
            transform.position = new Vector3(transform.position.x - .005f, transform.position.y, transform.position.z);
            //scary.enabled = false;
            //source.Stop();
        }
    }
}
