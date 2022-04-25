using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameEntity : MonoBehaviour
{
    public int EnterAreaNum=-1;
     public  abstract  bool HandleMessage(Telegram msg);
}
