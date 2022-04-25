using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : Singleton<Map>
{
   public Dictionary<int,Region> m_RegionsList;
   //预设体
   public GameObject wall;
    public GameObject RegionPrefab;
   //定义场地的长和宽
   public float length;
   
   public float width;
  
   //瓦片方块的大小
  public  int LNum;
  //实际可以用的长度数
  public int LengthNum;

  public  int WNum;
  //实际可以用的宽度数
  public int WidthNum;
   public int TotalNum;
   public float size;
   protected override void Awake()
   {
      base.Awake();
      m_RegionsList=new Dictionary<int, Region>();
   }
   private void Start() {
      
      length=(float)(LNum*size);
     width=(float)(WNum*size);
     WidthNum=WNum-2;
     LengthNum=LNum-2;
       TotalNum=WidthNum*LengthNum;
       //生成边界
       for(int i=0;i<WNum;i++)
       {
          float x=0.25f+i*0.5f;
         GameObject obj=Instantiate(wall,new Vector3(x,0f,0f),transform.rotation);
         obj.transform.parent=this.transform;
         if(LNum>1)
         {
          obj=Instantiate(wall,new Vector3(x,(LNum-1)*0.5f,0f),transform.rotation);
          obj.transform.parent=this.transform;
         }
       }
       for(int i=1;i<LNum-1;i++)
       {
          float y=i*0.5f;
           GameObject obj=Instantiate(wall,new Vector3(0.25f,y,0f),transform.rotation);
         obj.transform.parent=this.transform;
         if(WNum>1)
         {
              obj=Instantiate(wall,new Vector3(((WNum-1)*0.5f+0.25f),y,0f),transform.rotation);
          obj.transform.parent=this.transform;
         }
       }

       //关于区域检测器
       for(int i=0;i<TotalNum;i++)
 {
   GameObject obj= Instantiate(RegionPrefab,CalculatePos(i),Quaternion.identity);
  obj.GetComponent<Region>().Num=i;
  m_RegionsList.Add(i,obj.GetComponent<Region>());
 }
 //加完之后 要给每个region 的list指定附近的region
for(int i=0;i<TotalNum;i++)
{
 Region region=null;
 m_RegionsList.TryGetValue(i,out region);
 region.InitNearbyRegionList();
}
   }
void CalculatePos(out float Length,out float Width,int Num)
  {
    float LPosNum,WPosNum;
    if(Num<TotalNum&&Num>=0)
    {
     WPosNum=(float)Num%WidthNum+0.5f;
     LPosNum=(float)((int)(Num/WidthNum));
     Length=LPosNum*size+size;
     Width=WPosNum*size+size;
    }
    else
    {
      Length=-1;
      Width=-1;
    }
  }
  Vector2 CalculatePos(int Num)
  {
    float Length,Width;
   
    CalculatePos(out Length,out Width,Num);
    if(Length==-1&&Width==-1)
    {
       return Vector2.zero;
    }
    return new Vector2 (Width,Length);

  }
}
