using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

    public Transform patrolPoints;
    public float speed = 5;
    public float delay = .3f;
    public float turnRate = 90;

    private void Start()
    {
        Vector3[] patrolPointsArray = new Vector3[patrolPoints.childCount];

        for (int i = 0; i < patrolPointsArray.Length; i++)
        {
            patrolPointsArray[i] = patrolPoints.GetChild(i).position;
            patrolPointsArray[i].y = transform.position.y;
        }

        StartCoroutine(Move(patrolPointsArray));
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
}
