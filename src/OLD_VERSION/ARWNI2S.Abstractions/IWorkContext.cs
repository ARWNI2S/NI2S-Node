using System.Globalization;

namespace ARWNI2S
{
    public interface IWorkContext
    {
        Task<CultureInfo> GetWorkingCultureAsync();
    }
}
