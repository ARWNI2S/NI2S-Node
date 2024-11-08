namespace ARWNI2S.Infrastructure.Extensions
{
    public static class TaskExtensions
    {
        public static void DoNotAwait(this Task task)
        {

        }

        public static void DoNotAwait(this ValueTask task)
        {

        }
    }
}