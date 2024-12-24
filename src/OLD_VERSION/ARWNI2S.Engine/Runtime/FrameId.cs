namespace ARWNI2S.Engine.Runtime
{
    public class FrameId
    {
        private static FrameId _instance;

        public static FrameId Current => _instance;
    }
}