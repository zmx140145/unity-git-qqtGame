using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    //这是一个用顺序堆实现的树 再通过树的操作来实现优先级别
    private List<QueueVal<T>> list=new List<QueueVal<T>>();
    public PriorityQueue()
    {
        list.Add(null);
    }
    //获得优先队列里面元素的个数
    public int GetCount()
    {
     return list.Count-1;
    }
    //添加元素
    public void Push(QueueVal<T> val)
    {
     list.Add(val);
     //把刚加进去的元素进行上浮操作
     NodeUp(GetCount());
    }
    public QueueVal<T> Out()
    {
        if(GetCount()<1)
        {
            //没有储存到数据
            return null;
        }
        QueueVal<T> value;
        value=list[1];
        list[1]=list[GetCount()];
        list.RemoveAt(GetCount());
        NodeDown(1);
        return value;

    }
    //节点的上浮
    public void NodeUp(int index)
    {
        //如果记录的时间要小那么就上浮
     if(index>1&&list[index].key<list[index/2].key)
     {
         QueueVal<T> val;
      val=list[index];
      list[index]=list[index/2];
      list[index/2]=val;
      //继续上浮判断
      NodeUp(index/2);
     }
    }
    //节点的下沉
    public void NodeDown(int index)
    {
        int childIndex=0;
        //有左孩子
    if(2*index<GetCount())
    {
childIndex=2*index;
    }
    else
    {
        //没有左孩子就意味着没有右孩子
        return;
    }
    if(childIndex+1<GetCount()&&list[childIndex].key>list[childIndex+1].key)
    {
        //找到个更小的值
        childIndex++;
    }
    //保证再上面一层要小于下面一层的
    if(list[index].key<list[childIndex].key)
    {
     return;
    }
   
//进行交换 然后继续判断下沉
     QueueVal<T> value;
     value=list[index];
     list[index]=list[childIndex];
     list[childIndex]=value;
     //递归
     NodeDown(childIndex);
    }

}
