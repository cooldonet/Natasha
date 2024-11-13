﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace Natasha.CSharp.HotExecutor.Component
{
    internal class HETreeTriviaRewriter : CSharpSyntaxRewriter
    {
        private readonly HashSet<TriviaSyntaxPluginBase> _triviaPlugins;
        public HETreeTriviaRewriter()
        {
            _triviaPlugins = [];
        }

        public HETreeTriviaRewriter RegistePlugin(TriviaSyntaxPluginBase triviaSyntaxPluginBase)
        {
            _triviaPlugins.Add(triviaSyntaxPluginBase);
            return this;
        }

        public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia)
        {
            if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
            {
                var commentText = trivia.ToString();
                var commentLowerText = commentText.ToLower();
                string? newComment = null; 
                if (commentText.Length > 2)
                {
                    foreach (var plugin in _triviaPlugins)
                    {
                        if (plugin.IsMatch(commentText, commentLowerText))
                        {
                            var result= plugin.Handle(commentText, commentLowerText);
                            if (result != null)
                            {
                                newComment = result;
                            }
                        }
                    }
                }
                if (newComment!=null)
                {
                    return SyntaxFactory.Comment(newComment);
                }
            }
            return base.VisitTrivia(trivia);
        }
    }

}