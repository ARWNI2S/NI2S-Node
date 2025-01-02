namespace ARWNI2S.Engine.Extensibility
{
    internal static class ModulenameStringExtensions
    {
        public static string ToModuleName(this string moduleName)
        {
            var result = moduleName;
            var hasNiisMark = false;

            result = result.Replace("Module", "");
            if (result.Contains("NI2S"))
            {
                hasNiisMark = true;
                result = result.Replace("NI2S", "");
            }

            for(int i = 1; i < result.Length; i++)
            {
                if (char.IsUpper(result[i]))
                {
                    result = result.Insert(i, ".");
                    i++;
                }
            }

            if (hasNiisMark)
                result = "NI2S." + result;

            return result;
        }

        public static string ToFriendlyModuleName(this string moduleName)
        {
            var result = moduleName;
            result = result.Replace("Module", "");
            if (result.Contains("NI2S"))
            {
                result = result.Replace("NI2S", "");
            }
            for (int i = 1; i < result.Length; i++)
            {
                if (char.IsUpper(result[i]))
                {
                    result = result.Insert(i, " ");
                    i++;
                }
            }
            return result;
        }
    }
}
