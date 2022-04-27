using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{

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
              NearbyRegionWithoutDiagonalList.Add(tempRegion);
         }
     
     }

 }
 public Boom FindBoom()
 {
     foreach(GameObject obj in entitysList)
     {
         if(obj.GetComponent<Boom>())
         {
             return obj.GetComponent<Boom>();
         }
     }
     return null;
 }
public void BoomThisArea()
{
     Boom b=FindBoom();
           if(b!=null&&b.haveBoomed==false)
           {
               b.StartBoom();
           }
           if(b==null)
           {
            animator.SetTrigger("Boom");
           }
}
  private void OnTriggerEnter2D(Collider2D enterCollider) {
      if(enterCollider.CompareTag("Player"))
      {
         if(!enterCollider.gameObject.GetComponent<PlayerController>().InRegionsList.Contains(this))
       enterCollider.gameObject.GetComponent<PlayerController>().InRegionsList.Add(this);
      }
       if(enterCollider.CompareTag("Boom"))
      {
           if(!enterCollider.gameObject.GetComponent<Boom>().InRegionsList.Contains(this))
          enterCollider.gameObject.GetComponent<Boom>().InRegionsList.Add(this);
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
     
  }
    
}
