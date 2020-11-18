using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_CameraControls : MonoBehaviour
{
    //Members
    //This influences the speed of the camera's movement. The higher, the faster.
    private float fl_Speed = 10;

    // Update is called once per frame
    void Update()
    {
        //If the user presses A/D or the horizontal directional keys, move the camera accordingly
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            transform.position += new Vector3(0, 0, Input.GetAxisRaw("Horizontal") * fl_Speed * Time.deltaTime);
        }

        //If the user presses W/S or the vertical directional keys, move the camera accordingly
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            transform.position += new Vector3(0, Input.GetAxisRaw("Vertical") * fl_Speed * Time.deltaTime, 0);
        }

        //If the user presses Q, move the camera toward the L-System
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += new Vector3(-fl_Speed * Time.deltaTime * 2, 0, 0);
        }
        
        //If the user presses E, move the camera away from the L-System
        if (Input.GetKey(KeyCode.E))
        {
            transform.position += new Vector3(fl_Speed * Time.deltaTime * 2, 0, 0);
        }
    }
}