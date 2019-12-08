using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform player;
    public Vector3 offset = new Vector3(0, 20, 0);
	
	void Update ()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, .5f * Time.deltaTime);

	}
}
