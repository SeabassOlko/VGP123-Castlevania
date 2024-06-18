using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{ 
    [SerializeField] private float minXClamp = -2.66f;
    [SerializeField] private float maxXClamp = 74.66f;

    private void LateUpdate()
    {
        Vector3 cameraPos = transform.position;
        cameraPos.x = Mathf.Clamp(GameManager.Instance.PlayerInstance.transform.position.x, minXClamp, maxXClamp);
        transform.position = cameraPos;
    }
}
