using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceManager : MonoBehaviour
{
    
    private Rigidbody diceRb;
    private int diceValue;
    private bool ground;
    
    [SerializeField] private LayerMask lMask;
    [SerializeField] private float radius;
    [SerializeField] private GameObject[] players;
    [SerializeField] private ParticleSystem introParticle;
    
    void Start()
    {
        diceRb = GetComponent<Rigidbody>();
        
    }
    
    void Update()
    {
          if (diceRb.IsSleeping() &&  diceValue == 0 && ground)
          {
              if (Physics.Raycast(transform.position,Vector3.up,out var hit,Mathf.Infinity,lMask))
              {
                  diceValue = hit.collider.GetComponent<GetValue>().value;
                        
                  print(diceValue);
                   StartCoroutine(CreateAgent());
              }
          }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("ground") && !ground)
        {
            ground = true;

            if (ProjectileManager.ProjectileManagerInstance.diceId < ProjectileManager.ProjectileManagerInstance.DiceRb.Length - 1)
                ProjectileManager.ProjectileManagerInstance.diceId++;
            else
                ProjectileManager.ProjectileManagerInstance.diceId = 0;
        }
    }


    private IEnumerator CreateAgent()
    {
        var i = 0;
        
        while (i < diceValue)
        {
            float angle = i * Mathf.PI * 2f / diceValue;

            Vector3 newPos = new Vector3(transform.position.x + Mathf.Cos(angle) * radius, 0f, transform.position.z + Mathf.Sin(angle) * radius);

            Instantiate(players[Random.Range(0, 4)], newPos, Quaternion.identity);

            i++;

            yield return new WaitForSecondsRealtime(0.2f);
        }
                
        yield return new WaitForSecondsRealtime(1f);
        gameObject.SetActive(false);
        diceRb.isKinematic = true;
        diceValue = 0;
        gameObject.transform.position = ProjectileManager.ProjectileManagerInstance.DiceInitialPosition.position;
        gameObject.transform.rotation = Quaternion.identity;
        ground = false;
        
    }
}
