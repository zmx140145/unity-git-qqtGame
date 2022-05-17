using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PickableItem{powerDrink,speedShoe,healthDrink,boomCountDrug};
public class PlayerController : BaseGameEntity
{
    private Vector3 originalPos;
    public MainCamera mainCamera;
    public GameObject BoomParent;
    public bool InGrass=false;
    public float KeyAValue;
    public float KeySValue;
    public float KeyDValue;
    public float KeyWValue;
    public bool BoomKey;
    public GameObject BoomPrefab;
   public int dirtion;
    private int boomSetCount=0;
     [Header("角色属性")]
     public int boomPower;
     public int boomCount;
     public int speedPower;
     public int health;
     public float origionalSpeed;  
     public float speed;
     public bool isDie=false;
     public bool isDying=false;
     public bool canControll=true;
   private Animator animator;
    
    public override void Start()
        {
            base.Start();
            originalPos=transform.position;
            animator = GetComponent<Animator>();
            boomPower=1;
            speedPower=1;
            health=1;
            boomCount=1;
        }


        public override void Update()
        {
            base.Update();
            JudgeKey();
            CreateBoom();
            Move();
           JudgeAnimInfo();
        }
        //根据自身的条件来改变动画机里面的值
        private void JudgeAnimInfo()
        {
         if(InGrass)
         {
         animator.SetBool("InGrass",true);
         }
         else
         {
         animator.SetBool("InGrass",false);
         }
         if(isDie)
         {
              animator.SetBool("IsDie",true);
         }
         else
         {
               animator.SetBool("IsDie",false);
         }
         if(isDying)
         {
             animator.SetBool("IsDying",true);
         }
         else
         {
             animator.SetBool("IsDying",false);
         }
        }
        //判断数值
        private void JudgePowerValue()
        {
            speed=0.3f*(speedPower-1)+origionalSpeed;
        }
        //判断键入
        private void JudgeKey()
        {
            if(Input.GetKeyDown(KeyCode.B))
            {
                isDying=false;
                canControll=true;
            }
            //关于移动和操作的所有按键都在canControll这个条件里
            if(canControll)
            {
           if(Input.GetKeyDown(KeyCode.Space))
           {
            BoomKey=true;
           }
           else
           {
            BoomKey=false;
           }
             //键盘松开
           
            //键盘按下
            if(Input.GetKey(KeyCode.A))
            {
                KeyAValue+=Time.deltaTime;
               
            }
            else
            {
                KeyAValue=0f;
            }
            
            if(Input.GetKey(KeyCode.S))
            {
                KeySValue+=Time.deltaTime;
               
            }
            else
            {
                KeySValue=0f;
            }
             if(Input.GetKey(KeyCode.W))
            {
                 KeyWValue+=Time.deltaTime;
              
            }
            else
            {
                KeyWValue=0f;
            }
             if(Input.GetKey(KeyCode.D))
            {
                 KeyDValue+=Time.deltaTime;
              
               
            }
            else
            {
                KeyDValue=0f;
            }
            }
           else
           {
               BoomKey=false;
               KeyAValue=0f;
               KeyDValue=0f;
               KeyWValue=0f;
               KeySValue=0f;
               dirtion=4;
           }
        }
        //判断是否被炸到 被炸到就开始协程
        public void isBoomed()
        {
           
           
            StartCoroutine("CalculateDyingTime");
                      
        }
        //濒临死亡计算时长
       private IEnumerator CalculateDyingTime()
        {
           canControll=false;
           int time=5;
           while(time>=0f)
           {
              
               time-=1;
               yield return new WaitForSeconds(1f);
                if(!isDying)
               {
                   yield break;
               }
            }
              if(!isDying)
               {
                   canControll=true;
                   yield break;
               }
             isDie=true;
            
             StartCoroutine("Die");
            yield break;
        }
        //死亡
        IEnumerator Die()
        {
         isDying=false;
        mainCamera.StartCoroutine("PlayerDie");
        yield return new WaitForSeconds(5f);
        transform.position=originalPos;
        isDie=false;
        canControll=true;
        }
        //生成炸弹
        private void CreateBoom()
        {
            if(BoomKey&&!FindBoomInArea(EnterAreaNum)&&boomSetCount<boomCount)
            {
                boomSetCount++;
                GameObject obj=Instantiate(BoomPrefab,EnterRegion.areaPos,transform.rotation);
                obj.GetComponent<Boom>().BoomPower=boomPower;
                  obj.GetComponent<Boom>().Owner=this;
                //把区域赋给炸弹
                obj.GetComponent<Boom>().InRegionsList.Add(EnterRegion);
                obj.transform.parent=BoomParent.transform;
                
            }
        }
        private bool FindBoomInArea(int areaNum)
        {
            //越界
            if(areaNum<=-1||areaNum>Map.Instance.TotalNum)
            {
                //那么就不能放炸弹了
                return true;
            }
            //查找region上的物体 看看有没有炸弹Dic
            Region region;
            Map.Instance.m_RegionsDic.TryGetValue(EnterAreaNum,out region);
            
       foreach(GameObject obj in region.entitysList)
       {
        if(obj.GetComponent<Boom>())
        {
            return true;
        }
       }
       return false;
        }
        private void Move()
        {
            //如果四个值有其中两个的相邻的键盘按下
          if((KeyAValue>0||KeyDValue>0)&&(KeyWValue>0||KeySValue>0))
          {
            float value=Mathf.Max(KeySValue,Mathf.Max(Mathf.Max(KeyAValue,KeyDValue),KeyWValue));
            if(KeyAValue>0&&KeyAValue<value)
            {
                dirtion=3;
            }
            if(KeyDValue>0&&KeyDValue<value)
            {
                dirtion=2;
            }
            if(KeyWValue>0&&KeyWValue<value)
            {
                dirtion=1;
            }
            if(KeySValue>0&&KeySValue<value)
            {
                dirtion=0;
            }
          }
          else
          {
              if(KeyAValue>0||KeyDValue>0||KeyWValue>0||KeySValue>0)
              {
              //一个按键按下
              if(KeyAValue>0)
              {
                  dirtion=3;
              }
              if(KeyDValue>0)
              {
                  dirtion=2;
              }
              if(KeyWValue>0)
              {
                  dirtion=1;
              }
              if(KeySValue>0)
              {
                  dirtion=0;
              }
              }
              else
              {
                  dirtion=4;
              }
          }
Vector2 dir = Vector2.zero;
if(dirtion<4)
{
    switch(dirtion)
    {
        case 0:
         dir= Vector2.down;
                animator.SetInteger("Direction", 0);
        break;
        case 1:
        
                dir = Vector2.up;
                animator.SetInteger("Direction", 1);
        break;
        case 2:
        dir = Vector2.right;
                animator.SetInteger("Direction", 2);
        break;
        case 3:
        dir= Vector2.left;
                animator.SetInteger("Direction", 3);
        break;
        default:
        break;
    }
    
}
     dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().velocity = speed * dir;     
 }
 public void PickItem(PickableItem item)
 {
     switch(item)
     {
         case PickableItem.powerDrink:
         {
         boomPower++;

             break;
         }
         case PickableItem.speedShoe:
         {
            speedPower++;
             break;
         }
         case PickableItem.boomCountDrug:
         {
           boomCount++;  
             break;
         }
     }
     //根据拾取数量更改角色数值
     JudgePowerValue();
 }
    public override bool HandleMessage(Telegram msg)
    {
      switch(msg.Msg)
      {
          case MessageType.haveBoomed:
          {
              if(boomSetCount>0)
              boomSetCount--;
              return true;
          }
          case MessageType.Boom:
          {
              
              
                  isDying=true;
                  isBoomed();
                  return true;
              
          }
       
      }
         return false;
    }
}

