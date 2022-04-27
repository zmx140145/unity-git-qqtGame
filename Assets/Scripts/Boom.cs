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
   private BoxCollider2D boxCollider;
    public bool haveBoomed=false;
   public override void Start()
   {
       base.Start();
       animator=GetComponent<Animator>();
       BoomSetTime=Time.time;
       BoomTime=BoomSetTime+BoomRemainTime;
       circleCollider=GetComponent<CircleCollider2D>();
       boxCollider=GetComponent<BoxCollider2D>();
       if(EnterAreaNum!=-1)
       {
           transform.position=Map.Instance.m_RegionsDic[EnterAreaNum].areaPos;
       }
   }
   public override void Update() {
       base.Update();
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
   SendBoomToNearby();
   }
   public void SendBoomToNearby()
   {
       //n平方的复杂度
       //去当前的region找临近的region 
       //看看上面有没有炸弹
       //有的话向他们发送保炸的信息
       foreach(Region rg in EnterRegion.NearbyRegionWithoutDiagonalList)
       {
           Boom b=rg.FindBoom();
           if(b!=null&&b.haveBoomed==false)
           {
               MessageDispatcher.Instance.DispatchMessage(this,b,MessageType.Boom,null);
           }
       }
       
   }
   public void DestroyMe()
   {
       if(EnterRegion.entitysList.Contains(gameObject))
       {
       EnterRegion.entitysList.Remove(gameObject);
       }
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
