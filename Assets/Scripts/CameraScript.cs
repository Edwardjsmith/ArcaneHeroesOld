using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

 
    public GameObject Player;
    void Start()
    {
        //Search for player
        
        //Resize the camera accordingly to the resolution
        /*Camera.main.projectionMatrix = Matrix4x4.Ortho(
                -orthographicSize * aspect, orthographicSize * aspect,
                -orthographicSize, orthographicSize,
                GetComponent<Camera>().nearClipPlane, GetComponent<Camera>().farClipPlane);*/


    }

    void Update()
    {
        //Camera follows player only in the X axis
       if (Player != null)
        {
            GetComponent<Transform>().position = new Vector3(Player.GetComponent<Transform>().position.x, Player.GetComponent<Transform>().position.y, -10);
        }
    }
}
