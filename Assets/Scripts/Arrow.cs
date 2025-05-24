using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float arrowSpeed;


    private void Update()
    {
        Vector3 relativePosition = target.position - transform.position;    

        Quaternion rotation =  Quaternion.LookRotation(relativePosition  ,Vector3.up) ;

        Quaternion additionalRotation = Quaternion.Euler(0,- 90, 0);

        transform.rotation = rotation * additionalRotation;
    }
}
