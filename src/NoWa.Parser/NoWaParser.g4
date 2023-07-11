parser grammar NoWaParser;
options { tokenVocab = NoWaLexer; }

grammar_: rules=rule+ EOF ;

rule: nonterminal=ALPHA WS PRODUCES WS exprs+=expression (WS OR WS exprs+=expression)* WS TERMINATOR ;

expression: symbols+=symbol (WS symbols+=symbol)* ;

symbol: QUOTE QUOTE						#EmptyString
	  | value=ALPHA						#Nonterminal
	  | QUOTE value=(ALPHA | WS)+ QUOTE	#Terminal ;
// (t=terminal | nt=nonterminal) ;

// nonterminal: value=ALPHA ;

// terminal: QUOTE value=(ALPHA | WS)+ QUOTE ;