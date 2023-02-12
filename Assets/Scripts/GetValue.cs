using System;
using UnityEngine;

public class GetValue : MonoBehaviour
{
   public int value;

   private void Start()
   {
      value = Int32.Parse(gameObject.name); 
   }
}
