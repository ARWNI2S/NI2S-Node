using System.ComponentModel;

namespace ARWNI2S.Infrastructure.Metadata;

/// <summary>
/// Specifies that an assembly does not contain application code.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class FrameworkPartAttribute : Attribute
{
}
