﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Component.Exception;
using Natasha.CSharp.Syntax;
using System;
using System.Collections.Generic;

/// <summary>
/// 程序集编译构建器 - 语法树相关
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{

    public readonly List<SyntaxTree> SyntaxTrees;
    private CSharpParseOptions? _options;
    private UsingLoadBehavior _parsingBehavior;
    /// <summary>
    /// 配置语法树选项
    /// </summary>
    /// <param name="cSharpParseOptions"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder ConfigSyntaxOptions(CSharpParseOptions cSharpParseOptions)
    {
        _options = cSharpParseOptions;
        return this;
    }
    /// <summary>
    /// 配置语法树选项
    /// </summary>
    /// <param name="cSharpParseOptionsAction"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder ConfigSyntaxOptions(Func<CSharpParseOptions, CSharpParseOptions> cSharpParseOptionsAction)
    {
        _options = cSharpParseOptionsAction(new CSharpParseOptions());
        return this;
    }

    /// <summary>
    /// 设置 using 引用行为，默认为 None （同 WithoutCombineUsingCode）.
    /// </summary>
    /// <param name="usingLoadBehavior"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithCombineUsingCode(UsingLoadBehavior usingLoadBehavior)
    {
        _parsingBehavior = usingLoadBehavior;
        return this;
    }

    public AssemblyCSharpBuilder WithoutCombineUsingCode()
    {
        _parsingBehavior = UsingLoadBehavior.None;
        return this;
    }

    /// <summary>
    /// 注入代码并拼接using，拼接 using 的逻辑与 WithCombineUsingCode 方法设置有关.
    /// 在开启预热后，将自动拼接主域与当前域的 using.
    /// </summary>
    /// <param name="script">C#代码</param>
    /// <returns></returns>
    public AssemblyCSharpBuilder Add(string script)
    {
        return Add(script, _parsingBehavior);
    }

    /// <summary>
    /// 添加脚本
    /// </summary>
    /// <param name="script"></param>
    private AssemblyCSharpBuilder AddScript(string script)
    {
        var tree = NatashaCSharpSyntax.ParseTree(script, _options);
        var exception = NatashaExceptionAnalyzer.GetSyntaxException(tree);
        if (exception != null)
        {
            throw exception;
        }
        else
        {
            lock (SyntaxTrees)
            {
                SyntaxTrees.Add(tree);
            }
        }
        return this;
    }

    /// <summary>
    /// 快速添加语法树，无检查
    /// </summary>
    /// <param name="script"></param>
    /// <returns></returns>
    internal AssemblyCSharpBuilder FastAddScriptWithoutCheck(string script)
    {
        SyntaxTrees.Add(NatashaCSharpSyntax.ParseTree(script, _options));
        return this;
    }

    /// <summary>
    /// 添加语法树
    /// </summary>
    /// <param name="tree"></param>
    public AssemblyCSharpBuilder Add(SyntaxTree tree)
    {
        tree = NatashaCSharpSyntax.FormartTree(tree, _options);
        var exception = NatashaExceptionAnalyzer.GetSyntaxException(tree);
        if (exception != null)
        {
            throw exception;
        }
        else
        {
            lock (SyntaxTrees)
            {
                SyntaxTrees.Add(tree);
            }
        }
        return this;
    }

    public AssemblyCSharpBuilder ClearScript()
    {
        SyntaxTrees.Clear();
        return this;
    }

}



