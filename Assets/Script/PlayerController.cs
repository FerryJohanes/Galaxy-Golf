using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] GameObject arrow;
    [SerializeField] Image aim;
    [SerializeField] LineRenderer line;
    [SerializeField] TMP_Text shootCountText;
    [SerializeField] LayerMask ballLayer;
    [SerializeField] LayerMask rayLayer;
    [SerializeField] Transform camPivot;
    [SerializeField] Camera cam;
    [SerializeField] Vector2 camSensitivity;
    [SerializeField] float shootForce;
    [SerializeField] AudioSource sfx;
    [SerializeField] Animator animator;

    Vector3 lastMousePosition;
    float ballDistance;
    bool isShooting;

    Vector3 forceDir;
    float forceFactor;

    Renderer[] arrowRends;
    Color[] arrowOriginalColors;
    int shootCount = 0;

    public int ShootCount { get => shootCount; }

    private void Start()
    {
        ballDistance = Vector3.Distance(
                cam.transform.position, ball.Position) + 1;
        arrowRends = arrow.GetComponentsInChildren<Renderer>();
        arrowOriginalColors = new Color[arrowRends.Length];
        for (int i = 0; i < arrowRends.Length; i++)
        {
            arrowOriginalColors[i] = arrowRends[i].material.color;
        }
        aim.gameObject.SetActive(false);
        animator.SetBool("isShot", false);
        arrow.SetActive(false);
        shootCountText.text = "Shoot Count: " + shootCount;
    }

    private void Update()
    {
        if (ball.IsMoving || ball.IsTeleporting)
        {
            animator.SetBool("isShot", true);
            return;
        }

        if (ball.IsMoving == false || ball.IsTeleporting == false)
        {
            animator.SetBool("isShot", false);
            aim.gameObject.SetActive(false);
        }

        if (this.transform.position != ball.Position)
        {
            this.transform.position = ball.Position;
            aim.gameObject.SetActive(true);
            var rect = aim.GetComponent<RectTransform>();
            rect.anchoredPosition = cam.WorldToScreenPoint(ball.Position);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, ballDistance, ballLayer))
            {
                animator.SetBool("isShot", true);
                isShooting = true;
                arrow.SetActive(true);
                line.enabled = true;
                aim.gameObject.SetActive(true);
            }
        }

        //shooting mode
        if (Input.GetMouseButton(0) && isShooting == true)
        {
            animator.SetBool("isShot", true);
            aim.gameObject.SetActive(true);
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, ballDistance * 2, rayLayer))
            {
                var forceVector = ball.Position - hit.point;
                forceVector = new Vector3(forceVector.x, 0, forceVector.z);
                forceDir = forceVector.normalized;
                var forceMagnitude = forceVector.magnitude;
                forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 5);
                forceFactor = forceMagnitude / 5;
            }
            //arrow
            arrow.transform.LookAt(this.transform.position + forceDir);
            arrow.transform.localScale = new Vector3(1 + 0.5f * forceFactor, 1 + 0.5f * forceFactor, 1 + 2f * forceFactor);
            for (int i = 0; i < arrowRends.Length; i++)
            {
                arrowRends[i].material.color = Color.Lerp(Color.white, arrowOriginalColors[i], forceFactor);
            }

            //aim
            var rect = aim.GetComponent<RectTransform>();
            rect.anchoredPosition = Input.mousePosition;
            
            //line
            var ballScrPos = cam.WorldToScreenPoint(ball.Position);
            line.SetPositions(new Vector3[] { ballScrPos, Input.mousePosition });
        }
        
        if (Input.GetMouseButton(0) && isShooting == false)
        {
            aim.gameObject.SetActive(false);
            //animator.SetBool("isShoot", false);
            var current = cam.ScreenToViewportPoint(Input.mousePosition);
            var last = cam.ScreenToViewportPoint(lastMousePosition);

            var delta = current - last;

            //rotate horizontal
            camPivot.transform.RotateAround(
                ball.Position, 
                Vector3.up, 
                delta.x * camSensitivity.x);

            //rotate vertical
            camPivot.transform.RotateAround(
                ball.Position, 
                cam.transform.right, 
                -delta.y*camSensitivity.y);

            //angle
            var angle = Vector3.SignedAngle(
                Vector3.up, cam.transform.up, cam.transform.right);

            //kalau melewati batas, putar balik
            if (angle < 5)
                camPivot.transform.RotateAround(
                ball.Position,
                cam.transform.right,
                3 - angle);

            else if (angle > 65)
                camPivot.transform.RotateAround(
                ball.Position,
                cam.transform.right,
                65 - angle);
        }
        
        if (Input.GetMouseButtonUp(0) && isShooting)
        {
            sfx.Play();
            animator.SetBool("isShot", true);
            ball.Addforce(forceFactor * shootForce * forceDir);
            shootCount += 1;
            shootCountText.text = "Shoot Count: " + shootCount;
            forceFactor = 0;
            forceDir = Vector3.zero;
            
            isShooting = false;
            arrow.SetActive(false);
            aim.gameObject.SetActive(false);
            line.enabled = false;
        }

        lastMousePosition = Input.mousePosition;
    }

}
