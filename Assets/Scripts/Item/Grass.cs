using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : BaseGameEntity
{
  public bool PlayerIn=false;
  public bool PlayerInFirstStart=true;
  private Animator animator;
   public override void Start()
    {
        base.Start();
        animator=GetComponent<Animator>();
        
    }

    
   public override  void Update()
    {
        base.Update();
        JudgeInfoInAnim();
        transform.position=EnterRegion.areaPos;
    }
    //根据自身的值来改变动画机的值
    private void JudgeInfoInAnim()
    {
        if(PlayerIn)
        {
            if(PlayerInFirstStart)
            {
        animator.SetTrigger("PlayerIn");
        PlayerInFirstStart=false;
            }
        
        }
        else
        {
          PlayerInFirstStart=true;
        }
    }
     public override bool HandleMessage(Telegram msg)
    {

        switch(msg.Msg)
        {
            case MessageType.Boom:
               
            return true;
            default:
            return true;

        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().InGrass=true;
            PlayerIn=true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
         if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().InGrass=false;
            PlayerIn=false;
        }
    }
}
