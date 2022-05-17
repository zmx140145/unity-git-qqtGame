using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableItems : BaseGameEntity
{
    public bool startOnce=false;
   public override void Start()
    {
        base.Start();
        
    }

  
   public override void Update()
    {
        base.Update();
        if(!startOnce)
        {
            SetPositionInAreaCenter();
            startOnce=true;
        }
    }
     private void SetPositionInAreaCenter()
    {
         transform.position=EnterRegion.areaPos;
    }
    private void MoveToNextRegion(NextRegionDirection direction)
    {
        switch(direction)
        {
           case NextRegionDirection.left:
               {
                   if(EnterRegion.leftNearRegion)
                   transform.position=EnterRegion.leftNearRegion.areaPos;
               break;
               }
               case NextRegionDirection.right:
               {
                   if(EnterRegion.rightNearRegion)
                   transform.position=EnterRegion.rightNearRegion.areaPos;
                break;
               }
              case NextRegionDirection.top:
              {
                  if(EnterRegion.topNearRegion)
                   transform.position=EnterRegion.topNearRegion.areaPos;
                  break;
              }
              case NextRegionDirection.bottom:
              {
                  if(EnterRegion.bottomNearRegion)
                   transform.position=EnterRegion.bottomNearRegion.areaPos;
                  break;
              }
              case NextRegionDirection.all:
              {
                  break;
              }
        }
    }
    private NextRegionDirection JudgeMoveDirection(int direction)
    {
   if(direction==0)
   {
       return NextRegionDirection.bottom;
   }
   if(direction==1)
   {
       return NextRegionDirection.top;
   }
   if(direction==2)
   {
       return NextRegionDirection.right;
   }
   if(direction==3)
   {
       return NextRegionDirection.left;
   }
   return NextRegionDirection.all;
    }
    public override bool HandleMessage(Telegram msg)
    {
        return false;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
       MoveToNextRegion(JudgeMoveDirection( other.GetComponent<PlayerController>().dirtion)) ;
       
        }
}
}
