using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
   
    public static ProjectileManager ProjectileManagerInstance;
    [SerializeField] private GameObject Pointer;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int lineSegment = 50;
    [SerializeField] private float PointerHight;
    public Transform DiceInitialPosition;

    public Rigidbody[] DiceRb;
    public int diceId;
    void Start()
    {
        ProjectileManagerInstance = this;
        Pointer = GameObject.FindWithTag("Player");
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = lineSegment + 1;
        DiceRb[diceId].transform.position = DiceInitialPosition.position;
    }

  
    void Update()
    {
        if (GameManager.GameManagerInstance.moveByTouch && GameManager.GameManagerInstance.readyToToss)
        {
            UpdateProjectile();
        }
    }

    private void UpdateProjectile()
    {
        Vector3 velocity = CalculateVelocity(Pointer.transform.position, DiceInitialPosition.position, PointerHight);
           
        Visualize(velocity, Pointer.transform.position);
           
        transform.rotation = Quaternion.LookRotation(velocity);
           
        lineRenderer.enabled = true;

        if (!DiceRb[diceId].gameObject.activeSelf)
        {
            DiceRb[diceId].gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.enabled = false;
            DiceRb[diceId].isKinematic = false;
            DiceRb[diceId].velocity = velocity;
            DiceRb[diceId].AddForceAtPosition(Vector3.forward, transform.position * 10f);
            DiceRb[diceId].AddTorque(Vector3.one * 10f);
            GameManager.GameManagerInstance.moveByTouch = GameManager.GameManagerInstance.readyToToss = false;

            GameManager.GameManagerInstance.ProgressBar_Img.fillAmount = 0f;
        }
    }
    
    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float height)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXz = distance;
        distanceXz.y = 0f;
    
        float sY = distance.y;
        float sXz = distanceXz.magnitude;
    
        float vxz = sXz / height;
        float vy = sY / height + 0.5f * Mathf.Abs(Physics.gravity.y) * height;
    
        Vector3 result = distanceXz.normalized;
        result *= vxz;
        result.y = vy;
    
        return result;
    }
    
    void Visualize(Vector3 vo, Vector3 finalPos)
    {
        for (int i = 0; i < lineSegment; i++)
        {
            Vector3 pos = CalculatePosInTime(vo, i / (float)lineSegment * PointerHight);
            lineRenderer.SetPosition(i, pos);
        }
    
        lineRenderer.SetPosition(lineSegment, finalPos);
    }
    
    Vector3 CalculatePosInTime(Vector3 vo, float height)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;
    
        Vector3 result = DiceInitialPosition.position + vo * height;
        float sY = -0.5f * Mathf.Abs(Physics.gravity.y) * (height * height) + vo.y * height + DiceInitialPosition.position.y;
    
        result.y = sY;
    
        return result;
    }
}
