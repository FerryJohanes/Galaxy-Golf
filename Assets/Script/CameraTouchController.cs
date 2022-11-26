using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTouchController : MonoBehaviour
{
    [SerializeField, Range(0, 20)] float filterFactor;
    [SerializeField, Range(0, 2)] float zoomFactor = 1;
    [SerializeField] float minCamPos = 70;
    [SerializeField] float maxCamPos = 170;
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minZ;
    [SerializeField] float maxZ;

    Vector3 touchBeganWorldPose;
    Vector3 cameraBeganWorldPos;
    float distance;

    void Start()
    {
        distance = this.transform.position.y;
    }

    void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        var touch0 = Input.GetTouch(0);

        // simpan posisi awal tapi posisi real world
        if (touch0.phase == TouchPhase.Began)
        {
            touchBeganWorldPose = Camera.main.ScreenToWorldPoint(
                new Vector3(touch0.position.x, touch0.position.y, distance)
            );
            cameraBeganWorldPos = this.transform.position;

        }

        // Drag
        if (Input.touchCount == 1 && touch0.phase == TouchPhase.Moved)
        {
            var touchMovedWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(touch0.position.x, touch0.position.y, distance)
            );
            var delta = touchMovedWorldPos - touchBeganWorldPose;
            var targetPos = cameraBeganWorldPos - delta;

            // ToDO Clamp targetPos
            targetPos = new Vector3(
                Mathf.Clamp(targetPos.x, minX, maxX),
                targetPos.y,
                Mathf.Clamp(targetPos.z, minZ, maxZ)
            );


            this.transform.position = Vector3.Lerp(
                this.transform.position,
                targetPos,
                Time.deltaTime * filterFactor
            );
        }

        if (Input.touchCount < 2)
        {
            return;
        }
        var touch1 = Input.GetTouch(1);

        // Zoom
        if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
        {
            var touch0PrevPos = touch0.position - touch0.deltaPosition;
            var touch1PrevPos = touch1.position - touch1.deltaPosition;
            var prevDistance = Vector3.Distance(touch0PrevPos, touch1PrevPos);
            var currDistance = Vector3.Distance(touch0.position, touch1.position);
            var delta = currDistance - prevDistance;

            this.transform.position -= new Vector3(0, delta * zoomFactor, 0);
            this.transform.position = new Vector3(
                this.transform.position.x,
                Mathf.Clamp(this.transform.position.y, minCamPos, maxCamPos),
                this.transform.position.z
            );
            distance = this.transform.position.y;
        }
    }
}
