using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 moveVector;// vector di chuyen
    private Vector3 startOffset;// vecto offset(giu nguyen khoang cach)
    private Vector3 animtionOffset; // vecto pham vi hoat dong
    private Transform lookAt;// doi tuong follow
    private float transition = 0f; // khoang cac bien doi(chay nhanh, chay cham)
    private float animtionDuration = 3f;// thoi gian animation 
    private bool isStart = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.isGameStarted)
        {
            if (isStart == false)
            {
                animtionOffset = new Vector3(0, 5, 5);
                lookAt = GameObject.FindGameObjectWithTag("Player").transform;
                startOffset = transform.position - lookAt.position;
                isStart = true;
            }
            //x
            moveVector.x = 0;
            //y
            moveVector.y = Mathf.Clamp(moveVector.y, 3, 5);// gioi han pham vi hoat dong
            //z
            moveVector = lookAt.position + startOffset;

            if (transition > 1f)
            {
                transform.position = moveVector;
            }
            else
            {
                transform.position = Vector3.Lerp(moveVector + animtionOffset, moveVector, transition);
                transition += Time.deltaTime * 1 / animtionDuration;
                transform.LookAt(lookAt.position + Vector3.up);

            }
        }




    }
}
