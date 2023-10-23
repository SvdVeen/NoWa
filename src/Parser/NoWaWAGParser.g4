parser grammar NoWaWAGParser;
options { tokenVocab = NoWaWAGLexer; }

wag: productions=production+ EOF ;

production: head=productionhead WS DASH (weight=DECIMAL)? DASH ARROWRIGHT WS body=productionbody WS SEMICOLON ;

productionhead	: nonterminal=nt (attrs=headattributes)? ;
headattributes	: BRACEOPEN inheritedattrs=attributes SEMICOLON synthesizedattrs=attributes BRACECLOSE	// Both inherited and synthesized attributes, with a semicolon in between.
				| BRACEOPEN inheritedattrs=attributes SEMICOLON? BRACECLOSE								// Only inherited attributes, which may be succeeded by a semicolon.
				| BRACEOPEN SEMICOLON synthesizedattrs=attributes BRACECLOSE ;							// Only synthesized attributes, which are always preceded by a semicolon.
attributes		: attrs+=CHAR (COMMA attrs+=CHAR)* ;

productionbody	: (QUOTE QUOTE | symbols+=symbol (WS symbols+=symbol)*) ;
symbol			: QUOTE value=t QUOTE								#Terminal
				| value=nt (BRACEOPEN attrs=attributes BRACECLOSE)?	#Nonterminal ;

alpha	: ALPHA | DECIMAL | CHAR	; // An alphanumeric can be either an ALPHA, a DECIMAL, or a CHAR because of ambiguity in the lexer grammar. 
nt		: alpha (DASH alpha)*		; // A nonterminal expressed as an alphanumeric string with dashes.
t		: (alpha | WS)+				; // A terminal expressed as an alphanumeric string with spaces.