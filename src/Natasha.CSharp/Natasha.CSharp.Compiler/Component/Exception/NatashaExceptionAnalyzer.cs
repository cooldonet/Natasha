﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Linq;

namespace Natasha.CSharp.Compiler.Component.Exception
{
    internal sealed class NatashaExceptionAnalyzer
    {

        internal static NatashaException? GetSyntaxException(SyntaxTree tree)
        {

            var diagnostics = tree.GetDiagnostics();
            if (diagnostics.Any())
            {
                var errors = diagnostics.Where(item => !item.IsSuppressed).ToArray();
                if (errors != null && errors.Length > 0)
                {
                    var first = errors.FirstOrDefault(item => item.DefaultSeverity == DiagnosticSeverity.Error);
                    if (first == null)
                    {
                        first = errors[0];
                    }
                    var exception = new NatashaException(first.GetMessage());
                    exception.Diagnostics.AddRange(errors);
                    exception.Formatter = tree.ToString();
                    exception.ErrorKind = NatashaExceptionKind.Syntax;
                    return exception;
                }
            }
            return null;
        }

        internal static NatashaException GetCompileException(CSharpCompilation compilation, ImmutableArray<Diagnostic> errors)
        {
            var first = errors.FirstOrDefault(item=>item.DefaultSeverity == DiagnosticSeverity.Error);
            if (first == null)
            {
                first = errors[0];
            }   
            var exception = new NatashaException(first.GetMessage());
            exception.Diagnostics.AddRange(errors);
            if (first.Location.SourceTree != null)
            {
                exception.Formatter = first.Location.SourceTree.ToString();
            }
            else if (compilation.SyntaxTrees.Length > 0)
            {
                exception.Formatter = compilation.SyntaxTrees[0].ToString();
            }
            exception.CompileMessage = $"编译程序集为:{compilation.AssemblyName};CSharp版本:{compilation.LanguageVersion};";
            exception.ErrorKind = NatashaExceptionKind.Compile;
            return exception;
        }
    }
}
