using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Segmentation.Domain.Abstractions;
using Segmentation.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace Segmentation.Domain.Implementation
{
    public class ExpressionCompilationService : IExpressionCompilationService
    {

        private static readonly Lazy<IReadOnlyCollection<MetadataReference>> _defaultReferences = new(() =>
        {
            var trustedAssemblies = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string;
            if (string.IsNullOrWhiteSpace(trustedAssemblies))
            {
                throw new InvalidOperationException("Unable to resolve trusted assemblies for Roslyn compilation.");
            }

            var frameworkReferences = trustedAssemblies
                .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Where(File.Exists)
                .Select(path => MetadataReference.CreateFromFile(path))
                .ToList();

            // Ensure Microsoft.CSharp (runtime binder) is always available even if not part of the trusted list.
            frameworkReferences.Add(MetadataReference.CreateFromFile(typeof(Binder).Assembly.Location));

            return frameworkReferences;
        });

        public Func<object, bool?> Parse(Segment segment)
        {
            if (segment == null) return (x)=> null;

            string code = @$"
            using System;
            public static class DynamicClass
            {{
                public static bool Execute(dynamic x)
                {{ 
                    return {segment.Expression};
                }}
            }}";
            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var compilation = CSharpCompilation.Create(
                "DynamicAssembly",
                new[] { syntaxTree },
                _defaultReferences.Value,
                        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (result.Success)
            {
                ms.Seek(0, SeekOrigin.Begin);
                var assembly = Assembly.Load(ms.ToArray());
                var type = assembly.GetType("DynamicClass");
                var method = type.GetMethod("Execute");
                return (x) => (bool)method.Invoke(null, new object[] { x });
            }
            else
            {
                foreach (var diag in result.Diagnostics)
                    Console.WriteLine(diag);
                throw new InvalidOperationException("Compilation failed");

            }

        }
    }
}
