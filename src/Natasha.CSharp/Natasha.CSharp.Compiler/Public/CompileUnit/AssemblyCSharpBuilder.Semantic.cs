﻿using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Compiler.SemanticAnalaysis;
using System;
using System.Collections.Generic;

/// <summary>
/// 程序集编译构建器 - 语义
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{

    private readonly List<Func<AssemblyCSharpBuilder, CSharpCompilation, bool,  CSharpCompilation>> _semanticAnalysistor;
    private bool _semanticCheckIgnoreAccessibility;

    /// <summary>
    /// 语义检查时，开启访问性检查
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithAnalysisAccessibility()
    {
        _semanticCheckIgnoreAccessibility = false;
        return this;
    }

    /// <summary>
    /// 语义检查时，关闭访问性检查
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithoutAnalysisAccessibility()
    {
        _semanticCheckIgnoreAccessibility = true;
        return this;
    }
    /// <summary>
    /// 添加语义处理器
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder AddSemanticAnalysistor(Func<AssemblyCSharpBuilder, CSharpCompilation, bool, CSharpCompilation> func)
    {
        _semanticAnalysistor.Add(func);
        return this;
    }
    /// <summary>
    /// 移除语义处理器
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder RemoveSemanticAnalysistor(Func<AssemblyCSharpBuilder, CSharpCompilation, bool, CSharpCompilation> func)
    {
        _semanticAnalysistor.Remove(func);
        return this;
    }

    public bool EnableSemanticHandler;
    /// <summary>
    /// 开启语义检测, 若预热，则自动开启。
    /// </summary>
    /// <returns></returns>

    public AssemblyCSharpBuilder WithoutSemanticCheck()
    {
        EnableSemanticHandler = false;
        return this;
    }
    /// <summary>
    /// 关闭语义检测，默认：若预热则为开启，否则是关闭。
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithSemanticCheck()
    {
        EnableSemanticHandler = true;
        return this;
    }

    /// <summary>
    /// 清除当前编译单元所有的语义处理器
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder ClearInnerSemanticAnalysistor()
    {
        _semanticAnalysistor.Remove(UsingAnalysistor._usingSemanticDelegate);
        return this;
    }


}



