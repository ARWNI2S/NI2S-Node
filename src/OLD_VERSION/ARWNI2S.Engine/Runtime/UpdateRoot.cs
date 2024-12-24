namespace ARWNI2S.Engine.Runtime
{
    public class UpdateRoot : UpdateFunction
    {
        public UpdateRoot() : base() { }

        public FrameId FrameId { get; internal set; }
        public CycleId CycleId { get; internal set; }

        //protected override string DiagnosticMessage()
        //{
        //    throw new NotImplementedException();
        //}

        //protected override void ExecuteUpdate(float DeltaTime, LevelUpdate UpdateType, ThreadType CurrentThread, GraphEventRef MyCompletionGraphEvent)
        //{
        //    throw new NotImplementedException();
        //}
    }
}