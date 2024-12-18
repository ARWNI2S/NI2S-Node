using System.Globalization;

namespace ARWNI2S.Engine
{
    public interface IWorkContext
    {
        Task<CultureInfo> GetWorkingCultureAsync();
    }
}
