namespace ARWNI2S.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns the simple name of the class, for use in exception messages. 
        /// </summary>
        /// <returns>The simple name of this class.</returns>
        public static string SimpleClassName(this Type type)
        {
            string name = type.Name;

            // Just use the simple name.
            int index = name.IndexOfAny(['<', '{', '`']);
            if (index >= 0)
                name = name[..index];

            return name;
        }
    }
}
