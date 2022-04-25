using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueVal<T>
{
    public T value;
    public float key;
   public QueueVal(float key,T value)
   {
   this.key=key;
   this.value=value;
   }
}
