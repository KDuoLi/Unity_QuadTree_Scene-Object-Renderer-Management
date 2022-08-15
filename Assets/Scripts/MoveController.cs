using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private bool movecontro = true;
    public float Speed = 5f;
    void Update()
    {
        if(movecontro)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);
            if(transform.position.z>=90)
            {
                movecontro = false;
            }
        }
        else
        {
            transform.Translate(Vector3.back * Time.deltaTime * Speed);
            if(transform.position.z<=5)
            {
                movecontro = true;
            }
        }
    }
}
