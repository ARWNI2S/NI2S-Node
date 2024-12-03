namespace ARWNI2S
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static List<Exception> FlattenAggregate(this Exception exc)
        {
            var result = new List<Exception>();
            if (exc is AggregateException)
                result.AddRange(exc.InnerException.FlattenAggregate());
            else
                result.Add(exc);
            return result;
        }
    }
}
