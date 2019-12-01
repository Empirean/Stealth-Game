using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

    public Transform patrolPoints;
    public LayerMask viewMask;
    public float speed = 5;
    public float delay = .3f;
    public float turnRate = 90;
    public float viewRange = 8;

    public Light spotlight;
    float spotAngle;
    Color originalColor;

    Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        GameObject.Destroy(player);
        spotAngle = spotlight.spotAngle;
        Vector3[] patrolPointsArray = new Vector3[patrolPoints.childCount];

        for (int i = 0; i < patrolPointsArray.Length; i++)
        {
            patrolPointsArray[i] = patrolPoints.GetChild(i).position;
            patrolPointsArray[i].y = transform.position.y;
        }

        StartCoroutine(Move(patrolPointsArray));
        originalColor = spotlight.color;
    }

    private void Update()
    {

        if (CanSeePlayer())
        {
            spotlight.color = Color.red;
        }
        else
        {
            spotlight.color = originalColor;
        }
    }

    private void OnDrawGizmos()
    {
        
        Vector3 startPosition = patrolPoints.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        
        
        foreach (Transform patrolPoint in patrolPoints)
        {
            Gizmos.DrawSphere(patrolPoint.position, .3f);
            Gizmos.DrawLine(previousPosition, patrolPoint.position);
            previousPosition = patrolPoint.position;
        }
        
        Gizmos.DrawLine(previousPosition, startPosition);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * viewRange);
    }

    private IEnumerator Move(Vector3[] patrolPoints)
    {
        transform.position = patrolPoints[0];
        int targetIndex = 1;
        Vector3 targetPoint = patrolPoints[targetIndex];
        transform.LookAt(targetPoint);

        while (true)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetIndex], speed * Time.deltaTime);
            if (transform.position == targetPoint)
            {
                targetIndex = (targetIndex + 1) % patrolPoints.Length;
                targetPoint = patrolPoints[targetIndex];

                yield return new WaitForSeconds(delay);
                yield return StartCoroutine(FaceAngle(targetPoint));
            }
            
            yield return null;
        }
    }

    private IEnumerator FaceAngle(Vector3 targetPoint)
    {
        Vector3 direction = (targetPoint - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;


        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnRate * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
        
    }

    private bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewRange) {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleBetweenGuardAndPlayer < spotAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }

        }
        return false;
    }
}
