using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    public  List<Region> NearbyRegionsList; 
  public Vector3 areaPos;
  public BoxCollider2D boxCollider;
  public List<GameObject> entitysList;
  
 
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
    
 }
 void update()
 {
     
 }
 
 //更新附近的Region 到list 这个要在球场生成完所有region的时候 在CreateMap里执行
 public void InitNearbyRegionList()
 {
      int widthNum=Map.Instance.WidthNum;
     int[] areaNums;
     //这个分三种情况  如果是两边的格子 那就要排除多余的附近格子   
     //可以根据求余的值来判断
     if((Num+1)%widthNum==1)
     {
areaNums=new int[]{Num+1,Num-widthNum,Num-widthNum+1,Num+widthNum,Num+widthNum+1};
     }
     else
     {
      if((Num+1)%widthNum==0)
      {
 areaNums=new int[]{Num-1,Num-widthNum,Num-widthNum-1,Num+widthNum,Num+widthNum-1};
      }
      else
      {
areaNums=new int[]{Num+1,Num-1,Num-widthNum,Num-widthNum+1,Num-widthNum-1,Num+widthNum,Num+widthNum-1,Num+widthNum+1};
      }
     }
     
     Region tempRegion=null;
     foreach(var num in areaNums)
     {

         if(Map.Instance.m_RegionsList.TryGetValue(num,out tempRegion))
         {
              NearbyRegionsList.Add(tempRegion);
         }
     
     }

 }
private void OnTriggerEnter(Collider enterCollider)
{
entitysList.Add(enterCollider.gameObject);
}
  private void OnTriggerStay(Collider enterCollider) {
      if(enterCollider.CompareTag("Player"))
      {
       enterCollider.gameObject.GetComponent<PlayerController>().EnterAreaNum=Num;
      }
       if(enterCollider.CompareTag("Boom"))
      {
          enterCollider.gameObject.GetComponent<Boom>().EnterAreaNum=Num;
      }
      
  }
  private void OnTriggerExit(Collider enterCollider) {
      if(enterCollider.CompareTag("Player"))
      {
       enterCollider.gameObject.GetComponent<PlayerController>().EnterAreaNum=-1;
      
      }
      if(enterCollider.CompareTag("Boom"))
      {
          enterCollider.gameObject.GetComponent<Boom>().EnterAreaNum=-1;
      }
      if(entitysList.Contains(enterCollider.gameObject))
      {
      entitysList.Remove(enterCollider.gameObject);
      }
  }
    
}
