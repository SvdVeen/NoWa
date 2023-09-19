lexer grammar NoWaLexer;

fragment UPPERCASE: [A-Z] ;
fragment LOWERCASE: [a-z] ;
fragment DIGIT:		[0-9] ;

// Single-character terminals
WS:			' '  ;
QUOTE:		'\'' ;
PRODUCES:	'='  ;
TERMINATOR: ';'  ;
OR:			'|'  ;
DASH:		'-'  ;

ALPHA: (UPPERCASE | LOWERCASE | DIGIT)+ ; // Any alphanumeric character
NEWLINE: [\r\n]+ -> skip ; // skip line breaks
