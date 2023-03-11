using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace NI2S.Node.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AlwaysInterleaveDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private const string AlwaysInterleaveAttributeName = "NI2S.Node.Concurrency.AlwaysInterleaveAttribute";

        public const string DiagnosticId = "NI2S0001";
        public const string Title = "[AlwaysInterleave] must only be used on the grain interface method and not the grain class method";
        public const string MessageFormat = Title;
        public const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(
                AnalyzeSyntax,
                SyntaxKind.MethodDeclaration);
        }

        private static void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            var alwaysInterleaveAttribute = context.Compilation.GetTypeByMetadataName(AlwaysInterleaveAttributeName);

            var syntax = (MethodDeclarationSyntax)context.Node;
            var symbol = context.SemanticModel.GetDeclaredSymbol(syntax, context.CancellationToken);

            if (symbol.ContainingType.TypeKind == TypeKind.Interface)
            {
                // TODO: Check that interface inherits from IEntity
                return;
            }

            foreach (var attribute in symbol.GetAttributes())
            {
                if (!SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, alwaysInterleaveAttribute))
                {
                    return;
                }

                var syntaxReference = attribute.ApplicationSyntaxReference;

                context.ReportDiagnostic(
                    Diagnostic.Create(Rule, Location.Create(syntaxReference.SyntaxTree, syntaxReference.Span)));
            }
        }
    }
}
