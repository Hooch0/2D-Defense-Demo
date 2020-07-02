using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Speed = 10;

    private void Update()
    {
        //Simple camera movement

        Vector3 mov = transform.position;

        mov += new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0) * Speed * Time.deltaTime;

        mov.x = Mathf.Clamp(mov.x,-20f,20f);
        mov.y = Mathf.Clamp(mov.y,2.5f,4);

        transform.position = mov;
    }
}
