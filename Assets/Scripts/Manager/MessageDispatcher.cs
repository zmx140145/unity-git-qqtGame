using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MessageType{
Boom, haveBoomed
}
public class Telegram
{
public BaseGameEntity Sender;
public BaseGameEntity Receiver;
public MessageType Msg;
public System.Object ExtraInfo;
}
public class MessageDispatcher :Singleton<MessageDispatcher>
{
   private void DisCharge(BaseGameEntity pReceiver,Telegram msg)
   {
pReceiver.HandleMessage(msg);
   }
   public void DispatchMessage(BaseGameEntity Sender,BaseGameEntity receiver,MessageType msg,System.Object ExtraInfo)
   {

//创建信息
Telegram telegram=CreateTelegram(Sender,receiver,msg,ExtraInfo);

    //发送telegram到接收器
    DisCharge(receiver,telegram);
    

   }
 private Telegram CreateTelegram(BaseGameEntity Sender,BaseGameEntity receiver,MessageType msg,System.Object ExtraInfo)
   {
       Telegram telegram=new Telegram();
      
       telegram.Sender=Sender;
       telegram.Receiver=receiver;
       telegram.Msg=msg;
       telegram.ExtraInfo=ExtraInfo;
       return telegram;
   }
}
