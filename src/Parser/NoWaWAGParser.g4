parser grammar NoWaWAGParser;
options { tokenVocab = NoWaWAGLexer; }

wag: productions=production+ EOF ;

production: head=productionhead WS DASH (weight=wt)? DASH ARROW WS body=productionbody (WS exprs=productionexprs)? WS SEMICOLON ;

productionhead	: nonterminal=nt (attrs=headattributes)? ;
headattributes	: BRACEOPEN inheritedattrs=attributes SEMICOLON synthesizedattrs=attributes SEMICOLON staticattrs=attributes BRACECLOSE	// Inherited, synthesized, and static attributes, with a semicolon in between.
				| BRACEOPEN inheritedattrs=attributes SEMICOLON synthesizedattrs=attributes SEMICOLON? BRACECLOSE						// Inherited and synthesized attributes, with a semicolon in between, and which may be succeeded by a semicolon.
				| BRACEOPEN inheritedattrs=attributes SEMICOLON SEMICOLON staticattrs=attributes BRACECLOSE								// Inherited and static attributes, with two semicolons in between.
				| BRACEOPEN SEMICOLON synthesizedattrs=attributes SEMICOLON staticattrs=attributes BRACECLOSE							// Synthesized and static attributes, preceded by a semicolon, and with a semicolon in between.
				| BRACEOPEN inheritedattrs=attributes SEMICOLON? SEMICOLON? BRACECLOSE													// Only inherited attributes, which may be succeeded by one or two semicolons.
				| BRACEOPEN SEMICOLON synthesizedattrs=attributes SEMICOLON? BRACECLOSE													// Only synthesized attributes, which are always preceded by a semicolon and may be succeeded by a semicolon.
				| BRACEOPEN SEMICOLON SEMICOLON staticattrs=attributes BRACECLOSE ;														// Only static attributes, which are always preceded by two semicolons.

productionexprs : PARENOPEN exprs+=expression (WS? COMMA exprs+=expression)* PARENCLOSE	;
expression		: attr=attrref WS op=operator WS val=DECIMAL								;

productionbody	: (QUOTE QUOTE | symbols+=symbol (WS symbols+=symbol)*)				;
symbol			: QUOTE value=t QUOTE								#Terminal
				| value=nt (BRACEOPEN attrs=attributes BRACECLOSE)?	#Nonterminal	;


alpha		: ALPHA | DECIMAL | CHAR							; // An alphanumeric can be either an ALPHA, a DECIMAL, or a CHAR because of ambiguity in the lexer grammar.
attributes	: attrs+=CHAR (COMMA attrs+=CHAR)*					; // Attributes are simply characters.
attrref		: (DOLLAR | AMPERSAND | EXCLAMATION) CHAR			; // An attribute reference.
nt			: alpha (DASH alpha)*								; // A nonterminal expressed as an alphanumeric string with dashes.
t			: (alpha | WS)+										; // A terminal expressed as an alphanumeric string with spaces.
wt			: DECIMAL | attrref									; // A weight can either be a decimal number, or an attribute reference.
operator	: PLUS | DASH | EQUALS								; // Valid operators are plus, minus (dash), and equals.
