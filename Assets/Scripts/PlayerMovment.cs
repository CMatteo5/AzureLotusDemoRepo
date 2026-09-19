using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField]
    float speed = 2f;

    CharacterController characterController;

    float rotationX;
    float rotationY;
    public Camera playerCamera;
    float lookSpeed = 2f;

    [SerializeField]
    float lookLimitX = 5f;

    public bool gravityOn;
    public GameObject note;
    public GameObject carNote;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        //Find the direction that the player is moving
        //Input.GetAxisRaw returns -1 and 1, think about like a circle. Input.GetAxis is a more smooth, using decimals based on how long you've been holding it stuff like that.
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (gravityOn)
        {
            move += Physics.gravity;
        }
        else
        {
            StartCoroutine(goUp());

            //UnityEngine.Debug.Log("ehhlo");
        }

        //Adjust the Direction based on the rotation translating from relative space to world space
        move = transform.TransformDirection(move);

        //Actually moves the player and uses speed
        characterController.Move(move * speed * Time.deltaTime);

        //Detects mouse input, multiplies it at the lookspeed, then sets the rotation
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;

        //Clamps rotation in the bounds of look limit
        rotationX = Mathf.Clamp(rotationX, -lookLimitX, lookLimitX);

        //Edits the cameras transform to be that of the one we are updating
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        rotationY = Input.GetAxis("Mouse X") * lookSpeed;

        transform.localRotation *= Quaternion.Euler(0, rotationY, 0);
    }

    private void OnFire()
    {
        RaycastHit hit;
        bool interactCheck = Physics.Raycast(new Ray(transform.position, transform.forward), out hit, 2f);
        if (interactCheck && hit.transform.CompareTag("Interactable"))
        {
            //Code generated from ChatGPT on 5/4/25
            InteractAddon test = hit.collider.GetComponent<InteractAddon>();
            test.triggerInteraction();
            //End of generated code
        }
        if (interactCheck && hit.transform.CompareTag("Car"))
        {
            StartCoroutine(carInteract());
        }
    }

    private IEnumerator goUp()
    {
        yield return new WaitForSeconds(.01f);
        transform.position = new Vector3(transform.position.x,transform.position.y + .009f,transform.position.z);
    }
    private IEnumerator carInteract()
    {
        note.SetActive(false);
        carNote.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("EndScreen");
        
    }
}