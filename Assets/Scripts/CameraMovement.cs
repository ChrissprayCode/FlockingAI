using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform playerPos;
    public float lerpSpeed;
    public float camOffset;

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = new Vector3(playerPos.position.x, playerPos.position.y, playerPos.position.z + camOffset);

        transform.position = Vector3.Lerp(transform.position, offset, lerpSpeed * Time.deltaTime);
    }
}
