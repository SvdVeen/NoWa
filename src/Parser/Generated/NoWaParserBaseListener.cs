//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from NoWaParser.g4 by ANTLR 4.13.0

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace NoWa.Parser.Generated {

using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="INoWaParserListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.0")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class NoWaParserBaseListener : INoWaParserListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="NoWaParser.grammar_"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterGrammar_([NotNull] NoWaParser.Grammar_Context context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="NoWaParser.grammar_"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitGrammar_([NotNull] NoWaParser.Grammar_Context context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="NoWaParser.rule"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRule([NotNull] NoWaParser.RuleContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="NoWaParser.rule"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRule([NotNull] NoWaParser.RuleContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="NoWaParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpression([NotNull] NoWaParser.ExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="NoWaParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpression([NotNull] NoWaParser.ExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>EmptyString</c>
	/// labeled alternative in <see cref="NoWaParser.symbol"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEmptyString([NotNull] NoWaParser.EmptyStringContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>EmptyString</c>
	/// labeled alternative in <see cref="NoWaParser.symbol"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEmptyString([NotNull] NoWaParser.EmptyStringContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>Nonterminal</c>
	/// labeled alternative in <see cref="NoWaParser.symbol"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNonterminal([NotNull] NoWaParser.NonterminalContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>Nonterminal</c>
	/// labeled alternative in <see cref="NoWaParser.symbol"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNonterminal([NotNull] NoWaParser.NonterminalContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>Terminal</c>
	/// labeled alternative in <see cref="NoWaParser.symbol"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTerminal([NotNull] NoWaParser.TerminalContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>Terminal</c>
	/// labeled alternative in <see cref="NoWaParser.symbol"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTerminal([NotNull] NoWaParser.TerminalContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
} // namespace NoWa.Parser.Generated
