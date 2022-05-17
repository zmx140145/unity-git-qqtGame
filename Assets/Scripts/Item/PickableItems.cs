using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItems : BaseGameEntity
{
    public PickableItem item;
    public Animator animator;
    
    public override void Start() {
        base.Start();
        animator=GetComponent<Animator>();
    }
    public override void Update() {
        base.Update();
        SetPositionInAreaCenter();
    }
    private void SetPositionInAreaCenter()
    {
         transform.position=EnterRegion.areaPos;
    }
    public override bool HandleMessage(Telegram msg)
    {
        return false;
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().PickItem(item);
             if(EnterRegion.entitysList.Contains(gameObject))
       {
       EnterRegion.entitysList.Remove(gameObject);
       }
            Destroy(gameObject);
        }
    }
}
