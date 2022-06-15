using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionManager:MonoBehaviour
{
    [SerializeField]private int CameraX=7;
    [SerializeField]private int CameraY=4;

    public bool IsInPlayerVision(Vector3 currentPosition)
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (currentPosition.x <= playerPosition.x + CameraX
            && currentPosition.x >= playerPosition.x - CameraX
            && currentPosition.y <= playerPosition.y + CameraY
            && currentPosition.y >= playerPosition.y - CameraY
            ) return true;

        return false;
    }

    public bool IsInPlayerVision(Vector3 currentPosition,Vector3 nextPosition) {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        if ((currentPosition.x <= playerPosition.x + CameraX
            && currentPosition.x >= playerPosition.x - CameraX
            && currentPosition.y <= playerPosition.y + CameraY
            && currentPosition.y >= playerPosition.y - CameraY)||
            (nextPosition.x <= playerPosition.x + CameraX
            && nextPosition.x >= playerPosition.x - CameraX
            && nextPosition.y <= playerPosition.y + CameraY
            && nextPosition.y >= playerPosition.y - CameraY)
            ) return true;

        return false;
    }
}
