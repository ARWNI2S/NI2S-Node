namespace NI2S.Node.Runtime
{
    internal interface IMessageCenter
    {
        void SendMessage(Message msg);

        void DispatchLocalMessage(Message message);
    }
}
