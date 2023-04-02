using System;
using System.Runtime.Serialization;

namespace NI2S.Node
{
    public sealed class Resources
    {
        public static string ArgumentCannotBeNullOrEmpty => Internal.Resources.ArgumentCannotBeNullOrEmpty;
        public static string Argument_NullOrEmpty => Internal.Resources.Argument_NullOrEmpty;
        public static string Exception_PortMustBeGreaterThanZero => Internal.Resources.Exception_PortMustBeGreaterThanZero;
        public static string NodeHostBuilder_SingleInstance => Internal.Resources.NodeHostBuilder_SingleInstance;
        public static string TemplateRoute_MismatchedParameter => Internal.Resources.TemplateRoute_MismatchedParameter;
        public static string TemplateRoute_UnescapedBrace => Internal.Resources.TemplateRoute_UnescapedBrace;
        public static string TemplateRoute_CatchAllCannotBeOptional => Internal.Resources.TemplateRoute_CatchAllCannotBeOptional;
        public static string TemplateRoute_OptionalCannotHaveDefaultValue => Internal.Resources.TemplateRoute_OptionalCannotHaveDefaultValue;
        public static string TemplateRoute_CatchAllMustBeLast => Internal.Resources.TemplateRoute_CatchAllMustBeLast;
        public static string TemplateRoute_CannotHaveCatchAllInMultiSegment => Internal.Resources.TemplateRoute_CannotHaveCatchAllInMultiSegment;
        public static string TemplateRoute_CannotHaveConsecutiveParameters => Internal.Resources.TemplateRoute_CannotHaveConsecutiveParameters;
        public static string TemplateRoute_InvalidRouteTemplate => Internal.Resources.TemplateRoute_InvalidRouteTemplate;
        public static string TemplateRoute_CannotHaveConsecutiveSeparators => Internal.Resources.TemplateRoute_CannotHaveConsecutiveSeparators;


        public static string FormatApplicationPartFactory_InvalidFactoryType(Type type1, string v, Type type2) => FormatResource(Internal.Resources.FormatApplicationPartFactory_InvalidFactoryType, type1, v, type2);
        public static string FormatRelatedAssemblyAttribute_AssemblyCannotReferenceSelf(string v, string assemblyName) => FormatResource(Internal.Resources.FormatRelatedAssemblyAttribute_AssemblyCannotReferenceSelf, v, assemblyName);
        public static string FormatTemplateRoute_InvalidLiteral(string literal) => FormatResource(Internal.Resources.FormatTemplateRoute_InvalidLiteral, literal);
        public static string FormatTemplateRoute_InvalidParameterName(string empty) => FormatResource(Internal.Resources.FormatTemplateRoute_InvalidParameterName, empty);
        public static string FormatTemplateRoute_OptionalParameterCanbBePrecededByPeriod(string v1, string name, string v2) => FormatResource(Internal.Resources.FormatTemplateRoute_OptionalParameterCanbBePrecededByPeriod, v1, name, v2);
        public static string FormatTemplateRoute_OptionalParameterHasTobeTheLast(string v1, string name, string v2) => FormatResource(Internal.Resources.FormatTemplateRoute_OptionalParameterHasTobeTheLast, v1, name, v2);
        public static string FormatTemplateRoute_RepeatedParameter(string parameterName) => FormatResource(Internal.Resources.FormatTemplateRoute_RepeatedParameter, parameterName);
        public static string FormatRouteValueDictionary_DuplicateKey(string key, string v) => FormatResource(Internal.Resources.FormatRouteValueDictionary_DuplicateKey, key, v);
        public static string FormatRouteValueDictionary_DuplicatePropertyName(string fullName, string name1, string name2, string v) => FormatResource(Internal.Resources.FormatRouteValueDictionary_DuplicatePropertyName, fullName, name1, name2, v);
        public static string FormatRoutePattern_InvalidConstraintReference(object v, Type type) => FormatResource(Internal.Resources.FormatRoutePattern_InvalidConstraintReference, v, type);
        public static string FormatTemplateRoute_CannotHaveDefaultValueSpecifiedInlineAndExplicitly(string name) => FormatResource(Internal.Resources.FormatTemplateRoute_CannotHaveDefaultValueSpecifiedInlineAndExplicitly, name);
        public static string FormatMapGroup_RepeatedDictionaryEntry(string rawText, string dictionaryName, string key) => FormatResource(Internal.Resources.FormatMapGroup_RepeatedDictionaryEntry, rawText, dictionaryName, key);
        public static string FormatMapGroup_CustomEndpointUnsupported(Type type) => FormatResource(Internal.Resources.FormatMapGroup_CustomEndpointUnsupported, type);
        public static string FormatException_PathMustStartWithSlash(string v) => FormatResource(Internal.Resources.FormatException_PathMustStartWithSlash, v);

        private static string FormatResource(string formatString, params object[] arguments)
        {
            return string.Format(formatString, arguments);
        }

    }
}
