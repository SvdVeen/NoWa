parser grammar NoWaCFGParser;
options { tokenVocab = NoWaCFGLexer; }

cfg: rules=rule+ EOF ;

rule: head=nt WS EQUALS WS productions+=production (WS PIPE WS productions+=production)* WS SEMICOLON ;

production: QUOTE QUOTE | symbols+=symbol (WS symbols+=symbol)* ;

symbol	: QUOTE value=t QUOTE	#Terminal
		| value=nt				#Nonterminal ;

nt	: ALPHA (DASH ALPHA)*	; // Nonterminals can be several alphanumeric strings separated with dashes.
t	: (ALPHA | WS)+			; // Terminals can be any combination of alphanumeric strings and whitespaces.