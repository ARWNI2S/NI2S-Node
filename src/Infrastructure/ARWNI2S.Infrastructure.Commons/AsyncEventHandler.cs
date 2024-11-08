namespace ARWNI2S.Infrastructure
{
    public delegate ValueTask AsyncEventHandler(object sender, EventArgs e);

    public delegate ValueTask AsyncEventHandler<TEventArgs>(object sender, TEventArgs e)
        where TEventArgs : EventArgs;

    public delegate ValueTask AsyncEventHandler<TSender, TEventArgs>(TSender sender, TEventArgs e)
        where TSender : class
        where TEventArgs : EventArgs;
}