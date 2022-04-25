using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : BaseGameEntity
{
   private float BoomSetTime;
   private float BoomRemainTime=3f;
   private float BoomTime;
   private Animator animator;
   private CircleCollider2D circleCollider;
    public bool haveBoomed=false;
   void Start()
   {
       animator=GetComponent<Animator>();
       BoomSetTime=Time.time;
       BoomTime=BoomSetTime+BoomRemainTime;
       circleCollider=GetComponent<CircleCollider2D>();
   }
   private void Update() {
      switch((int)(BoomTime-Time.time)%3)
      {
          case 2:
          animator.SetInteger("Time",3);
          break;
           case 1:
          animator.SetInteger("Time",2);
          break;
           case 0:
          animator.SetInteger("Time",1);
          break;
          default:
          break;
      }
       if(Time.time-BoomTime>0f&&haveBoomed==false)
       {
       StartBoom();
       }
   }
   public void StartBoom()
   {
   animator.SetTrigger("Boom");
   haveBoomed=true;
   }
   public void DestroyMe()
   {
      
       Destroy(this.gameObject);
   }
   private void OnTriggerStay2D(Collider2D other) {
      if( other.CompareTag("Player"))
      {
         
          circleCollider.enabled=false;
      }
   }
   private void OnTriggerExit2D(Collider2D other) {
       if(other.CompareTag("Player"))
       {
            circleCollider.enabled=true;
       }
   }
    public override bool HandleMessage(Telegram msg)
    {

        switch(msg.Msg)
        {
            case MessageType.Boom:
                 StartBoom();
            return true;
            default:
            return true;

        }
    }
}
