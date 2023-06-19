parser grammar NoWaParser;
options { tokenVocab = NoWaLexer; }

grammar_: rules=rule+ EOF ;

rule: name=nonterminal WS PRODUCES WS exprs+=expression (WS OR WS exprs+=expression)* WS TERMINATOR ;

expression: symbols+=symbol (WS symbols+=symbol)* ;

symbol: (t=terminal | nt=nonterminal) ;

nonterminal: value=ALPHA ;

terminal: QUOTE value=(ALPHA | WS)+ QUOTE ;