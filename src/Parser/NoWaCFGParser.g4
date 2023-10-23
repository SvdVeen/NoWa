parser grammar NoWaCFGParser;
options { tokenVocab = NoWaCFGLexer; }

grammar_: rules=rule+ EOF ;

rule: nonterminal=(ALPHA | DASH)+ WS PRODUCES WS exprs+=expression (WS OR WS exprs+=expression)* WS TERMINATOR ;

expression: symbols+=symbol (WS symbols+=symbol)* ;

symbol: QUOTE QUOTE						#EmptyString
	  | value=(ALPHA | DASH)+			#Nonterminal
	  | QUOTE value=(ALPHA | WS)+ QUOTE	#Terminal ;