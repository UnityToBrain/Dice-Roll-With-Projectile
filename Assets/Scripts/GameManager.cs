using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance;
    
    private Vector2 lastMousePosition;
    private GameObject currentLevel;
    private Vector3 _mouseStartPos, playerStartPos;
    private Camera cam;
    
    [SerializeField] [Range(0f,100f)] public float pointerSpeed;
    [HideInInspector] public Vector3  controller;
    [HideInInspector] public bool moveByTouch,readyToToss;
    public  Image ProgressBar_Img;
    [SerializeField] private float loadTime;
    void Start()
    {
        GameManagerInstance = this;
        readyToToss = true;
        cam = Camera.main;
    }

    void Update()
    {
         if (Input.GetMouseButtonDown(0) && readyToToss)
         {
             moveByTouch = true;
                    
             Plane plane = new Plane(Vector3.up, 5f);
        
             Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                
             if (plane.Raycast(ray,out var distance))
             {
                 _mouseStartPos = ray.GetPoint(distance);
                 playerStartPos = transform.position;
             }
                        
         }
        
         // if (Input.GetMouseButtonUp(0))
         // {
         //     moveByTouch = false;
         // }
         
         if (moveByTouch)
         {
             Plane plane = new Plane(Vector3.up, 5f);
             Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            
             if (plane.Raycast(ray,out var distance))
             {
                 Vector3 mousePos = ray.GetPoint(distance);
                 Vector3 move = mousePos - _mouseStartPos;
                 controller = playerStartPos + move;
                 controller.x = Mathf.Clamp(controller.x,-7f,7f);
                 controller.z = Mathf.Clamp(controller.z,-18.75f,18.75f);
                 transform.position  = Vector3.MoveTowards(transform.position,controller,Time.deltaTime * pointerSpeed);
             }
         }

         if (!readyToToss)
         {
             Invoke("FillProgressBar",1f);
         }
    }

    private void FillProgressBar()
    {
        if (ProgressBar_Img.fillAmount != 1f)
        {
            ProgressBar_Img.fillAmount += loadTime * Time.deltaTime;
        }
        else
        {
            readyToToss = true;
        }
    }
}
