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
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="NoWaParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.0")]
[System.CLSCompliant(false)]
public interface INoWaParserListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="NoWaParser.grammar_"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterGrammar_([NotNull] NoWaParser.Grammar_Context context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="NoWaParser.grammar_"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitGrammar_([NotNull] NoWaParser.Grammar_Context context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="NoWaParser.rule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRule([NotNull] NoWaParser.RuleContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="NoWaParser.rule"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRule([NotNull] NoWaParser.RuleContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="NoWaParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpression([NotNull] NoWaParser.ExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="NoWaParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpression([NotNull] NoWaParser.ExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>EmptyString</c>
	/// labeled alternative in <see cref="NoWaParser.symbol"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEmptyString([NotNull] NoWaParser.EmptyStringContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>EmptyString</c>
	/// labeled alternative in <see cref="NoWaParser.symbol"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEmptyString([NotNull] NoWaParser.EmptyStringContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>Nonterminal</c>
	/// labeled alternative in <see cref="NoWaParser.symbol"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNonterminal([NotNull] NoWaParser.NonterminalContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>Nonterminal</c>
	/// labeled alternative in <see cref="NoWaParser.symbol"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNonterminal([NotNull] NoWaParser.NonterminalContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>Terminal</c>
	/// labeled alternative in <see cref="NoWaParser.symbol"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTerminal([NotNull] NoWaParser.TerminalContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>Terminal</c>
	/// labeled alternative in <see cref="NoWaParser.symbol"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTerminal([NotNull] NoWaParser.TerminalContext context);
}
} // namespace NoWa.Parser.Generated
