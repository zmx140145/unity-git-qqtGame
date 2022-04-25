using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseGameEntity
{
    public float KeyAValue;
    public float KeySValue;
    public float KeyDValue;
    public float KeyWValue;
    public bool BoomKey;
    public GameObject BoomPrefab;
   public int dirtion;
     public float speed;
    
    
   private Animator animator;
   
    private void Start()
        {
            animator = GetComponent<Animator>();
        }


        private void Update()
        {
            
            JudgeKey();
            CreateBoom();
            Move();
        }
        private void JudgeKey()
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
            if(Input.GetKeyUp(KeyCode.A))
            {
                KeyAValue=0f;
            }
            if(Input.GetKeyUp(KeyCode.D))
            {
              KeyDValue=0f;
            }
            if(Input.GetKeyUp(KeyCode.S))
            {
                KeySValue=0f;
            }
            if(Input.GetKeyUp(KeyCode.W))
            {
                KeyWValue=0f;
            }
            //键盘按下
            if(Input.GetKey(KeyCode.A))
            {
                KeyAValue+=Time.deltaTime;
               
            }
            
            if(Input.GetKey(KeyCode.S))
            {
                KeySValue+=Time.deltaTime;
               
            }
             if(Input.GetKey(KeyCode.W))
            {
                 KeyWValue+=Time.deltaTime;
              
            }
             if(Input.GetKey(KeyCode.D))
            {
                 KeyDValue+=Time.deltaTime;
              
               
            }
           
        }
        private void CreateBoom()
        {
            if(BoomKey)
            {
                Instantiate(BoomPrefab,transform.position,transform.rotation);
                
            }
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
    public override bool HandleMessage(Telegram msg)
    {
       return true;
    }
}

