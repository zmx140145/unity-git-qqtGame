using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NextRegionDirection{left,right,top,bottom,all};
public class Region : MonoBehaviour
{
   public Region leftNearRegion;
   public Region rightNearRegion;
   public Region topNearRegion;
   public Region bottomNearRegion;
    public  List<Region> NearbyRegionsList; 
    public  List<Region> NearbyRegionWithoutDiagonalList;
    
  public Vector3 areaPos;
  public BoxCollider2D boxCollider;
  public List<GameObject> entitysList;
  private Animator animator;
 
  //X轴
  public float width;
  //Z轴
  public float length;
  public int Num;
  private float updateTime=0.5f;
 void Start()
 {

     if(GetComponent<BoxCollider2D>())
     {
         boxCollider=GetComponent<BoxCollider2D>();
     }
     else
     {
          boxCollider=gameObject.AddComponent<BoxCollider2D>();
     }
    
     boxCollider.size=new Vector2(width,length);
     boxCollider.isTrigger=true;
     areaPos=transform.position;
    animator=GetComponent<Animator>();
 }
 void update()
 {
     
 }
 
 //更新附近的Region 到list 这个要在球场生成完所有region的时候 在CreateMap里执行
 public void InitNearbyRegionList()
 {
      NearbyRegionsList=new List<Region>();
     NearbyRegionWithoutDiagonalList=new List<Region>();
      int widthNum=Map.Instance.WidthNum;
     int[] areaNums;
     int[] areaNums2;
     //这个分三种情况  如果是两边的格子 那就要排除多余的附近格子   
     //可以根据求余的值来判断
     if((Num+1)%widthNum==1)
     {
areaNums=new int[]{Num+1,Num-widthNum,Num-widthNum+1,Num+widthNum,Num+widthNum+1};
areaNums2=new int[]{Num+1,Num-widthNum,Num+widthNum};
     }
     else
     {
      if((Num+1)%widthNum==0)
      {
 areaNums=new int[]{Num-1,Num-widthNum,Num-widthNum-1,Num+widthNum,Num+widthNum-1};
  areaNums2=new int[]{Num-1,Num-widthNum,Num+widthNum};
      }
      else
      {
areaNums=new int[]{Num+1,Num-1,Num-widthNum,Num-widthNum+1,Num-widthNum-1,Num+widthNum,Num+widthNum-1,Num+widthNum+1};
areaNums2=new int[]{Num+1,Num-1,Num-widthNum,Num+widthNum};
      }
     }
     
     Region tempRegion=null;
     foreach(var num in areaNums)
     {
         tempRegion=null;

         if(Map.Instance.m_RegionsDic.TryGetValue(num,out tempRegion))
         {
            
             if(tempRegion)
              Debug.Log("yes");
              NearbyRegionsList.Add(tempRegion);
         }
     
     }
     foreach(var num in areaNums2)
     {

      tempRegion=null;
         if(Map.Instance.m_RegionsDic.TryGetValue(num,out tempRegion))
         {
             if(tempRegion)
             {
              NearbyRegionWithoutDiagonalList.Add(tempRegion);
              //四个方向的region的指定
              if(num==Num+1)
              {
                  rightNearRegion=tempRegion;
              }
                if(num==Num-1)
              {
                  leftNearRegion=tempRegion;
              }
                if(num==Num+widthNum)
              {
                 topNearRegion=tempRegion;
              }
                if(num==Num-widthNum)
              {
                  bottomNearRegion=tempRegion;
              }
             }
         }
     
     }

 }
 public PlayerController FindPlayer()
 {
      foreach(GameObject obj in entitysList)
     {
         if(obj)
         {
         if(obj.GetComponent<PlayerController>())
         {
             return obj.GetComponent<PlayerController>();
         }
         }
         else
         {
             entitysList.Remove(obj);
         }
     }
     return null;
 }
 public Boom FindBoom()
 {
     foreach(GameObject obj in entitysList)
     {
         if(obj)
         {
         if(obj.GetComponent<Boom>())
         {
             return obj.GetComponent<Boom>();
         }
         }
         else
         {
             entitysList.Remove(obj);
         }
     }
     return null;
 }
public void BoomThisArea(int BoomCount,NextRegionDirection direction)
{
    if(BoomCount<0)
    {
        return;
    }
     Boom b=FindBoom();
     PlayerController player=FindPlayer();
     if(player)
     {
         MessageDispatcher.Instance.DispatchMessage(null,player,MessageType.Boom,null);
     }
           if(b!=null&&b.haveBoomed==false)
           {
              b.StartBoom();
           }
           if(b==null)
           {
            animator.SetTrigger("Boom");
           }
           switch(direction)
           {
               case NextRegionDirection.left:
               {
                   if(leftNearRegion)
                   leftNearRegion.BoomThisArea(--BoomCount,NextRegionDirection.left);
               break;
               }
               case NextRegionDirection.right:
               {
                   if(rightNearRegion)
                   rightNearRegion.BoomThisArea(--BoomCount,NextRegionDirection.right);
                break;
               }
              case NextRegionDirection.top:
              {
                  if(topNearRegion)
                  topNearRegion.BoomThisArea(--BoomCount,NextRegionDirection.top);
                  break;
              }
              case NextRegionDirection.bottom:
              {
                  if(bottomNearRegion)
                  bottomNearRegion.BoomThisArea(--BoomCount,NextRegionDirection.bottom);
                  break;
              }
              case NextRegionDirection.all:
              {
                  BoomCount--;
                    if(leftNearRegion)
                   leftNearRegion.BoomThisArea(BoomCount,NextRegionDirection.left);
                    if(rightNearRegion)
                   rightNearRegion.BoomThisArea(BoomCount,NextRegionDirection.right);
                    if(topNearRegion)
                  topNearRegion.BoomThisArea(BoomCount,NextRegionDirection.top);
                    if(bottomNearRegion)
                  bottomNearRegion.BoomThisArea(BoomCount,NextRegionDirection.bottom);
                  break;
              }
           }
}
  private void OnTriggerEnter2D(Collider2D enterCollider) {
      if(enterCollider.CompareTag("Player"))
      {
         if(!enterCollider.gameObject.GetComponent<PlayerController>().InRegionsList.Contains(this))
       enterCollider.gameObject.GetComponent<PlayerController>().InRegionsList.Add(this);
      }
       if(enterCollider.CompareTag("StaticItem"))
      {
         if(!enterCollider.gameObject.GetComponent<BaseGameEntity>().InRegionsList.Contains(this))
       enterCollider.gameObject.GetComponent<BaseGameEntity>().InRegionsList.Add(this);
      }
       if(enterCollider.CompareTag("Boom"))
      {
           if(!enterCollider.gameObject.GetComponent<Boom>().InRegionsList.Contains(this))
          enterCollider.gameObject.GetComponent<Boom>().InRegionsList.Add(this);
      }
       if(enterCollider.CompareTag("MoveableItem"))
      {
         if(!enterCollider.gameObject.GetComponent<BaseGameEntity>().InRegionsList.Contains(this))
       enterCollider.gameObject.GetComponent<BaseGameEntity>().InRegionsList.Add(this);
      }
      
  }
  private void OnTriggerExit2D(Collider2D enterCollider) {
      if(enterCollider.CompareTag("Player"))
      {
           if(enterCollider.gameObject.GetComponent<PlayerController>().InRegionsList.Contains(this))
       enterCollider.gameObject.GetComponent<PlayerController>().InRegionsList.Remove(this);
      
      }
      if(enterCollider.CompareTag("Boom"))
      {
            if(enterCollider.gameObject.GetComponent<Boom>().InRegionsList.Contains(this))
          enterCollider.gameObject.GetComponent<Boom>().InRegionsList.Remove(this);
      }
       if(enterCollider.CompareTag("StaticItem"))
      {
         if(enterCollider.gameObject.GetComponent<BaseGameEntity>().InRegionsList.Contains(this))
       enterCollider.gameObject.GetComponent<BaseGameEntity>().InRegionsList.Remove(this);
      }
        if(enterCollider.CompareTag("MoveableItem"))
      {
         if(enterCollider.gameObject.GetComponent<BaseGameEntity>().InRegionsList.Contains(this))
       enterCollider.gameObject.GetComponent<BaseGameEntity>().InRegionsList.Remove(this);
      }
     
  }
    
}
