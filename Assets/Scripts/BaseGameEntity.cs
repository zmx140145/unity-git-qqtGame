using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameEntity : MonoBehaviour
{
     public List<Region> InRegionsList;
    public Region EnterRegion;
    public Region LastRegion;
    public int EnterAreaNum=-1;
    public virtual void Start()
    {
        InRegionsList=new List<Region>();
    }
    public virtual void Update() {
        JudgeRegion();
    }
    //判断自己所在的区域 在自身按距离远近判断 能够避免误差
    private void JudgeRegion()
    {
         if(InRegionsList.Count==0)
        {
           
            EnterRegion=null;
        }
        if(EnterRegion)
        {
            EnterAreaNum=EnterRegion.Num;
        }
        else
        {
            EnterAreaNum=-1;
        }
       
        Vector3 v3;
        //记录最近的region
        Region Target=null;
        //设置一个记录的数
        float distance=float.MaxValue;
        //遍历列表时间复杂n
        foreach(Region rg in InRegionsList)
        {
            v3=transform.position;
            v3.z=rg.areaPos.z;
            //如果距离比记录的小 更新记录
            if((v3-rg.areaPos).sqrMagnitude<distance)
            {
               distance=(v3-rg.areaPos).sqrMagnitude;
               Target=rg;

            }

        }
        if(EnterRegion!=Target&&EnterRegion)
        {
            LastRegion=EnterRegion;
           
        }
        EnterRegion=Target;
        if(!EnterRegion.entitysList.Contains(gameObject))
            EnterRegion.entitysList.Add(gameObject);
            if(LastRegion)
            {
        if(LastRegion.entitysList.Contains(gameObject))
            LastRegion.entitysList.Remove(gameObject);
            }
    }
     public  abstract  bool HandleMessage(Telegram msg);
}
