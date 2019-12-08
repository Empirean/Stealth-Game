using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public System.Action OnFinish;

    public float speed = 7;
    public float smoothDelay = 0.1f;
    public float turnRate = 8;

    bool disabled = false;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    float smoothAngle;
    Vector3 playerVelocity;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Guard.OnGuardSpottedPlayer += Disabled;
    }


    void Update ()
    {
        Vector3 input = Vector3.zero;
        if (!disabled)
        {
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }

        float magnitude = input.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, magnitude, ref smoothMoveVelocity, smoothDelay);

        float angle = Mathf.Atan2(input.x, input.z) *  Mathf.Rad2Deg;
        smoothAngle = Mathf.LerpAngle(smoothAngle, angle, turnRate * Time.deltaTime * magnitude);
        playerVelocity = transform.forward * smoothInputMagnitude * speed;
        //  transform.eulerAngles = Vector3.up * smoothAngle;
        //  transform.Translate(transform.forward * speed * Time.deltaTime * smoothInputMagnitude, Space.World);
        
	}

    private void Disabled()
    {
        disabled = true;
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * smoothAngle));
        rb.MovePosition(rb.position + playerVelocity * Time.deltaTime);
    }

    private void OnDestroy()
    {
        Guard.OnGuardSpottedPlayer -= Disabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            if (OnFinish != null)
            {
                Disabled();
                OnFinish();
            }
        }
    }
}
