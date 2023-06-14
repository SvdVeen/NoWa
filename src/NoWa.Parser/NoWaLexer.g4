lexer grammar NoWaLexer;

fragment UPPERCASE: [A-Z] ;
fragment LOWERCASE: [a-z] ;
fragment DIGIT:		[0-9] ;

// Single-character terminals
SPACE:		' '  ;
QUOTE:		'\'' ;
PRODUCES:	'='  ;
TERMINATOR: ';'  ;
OR:			'|'  ;


ALPHA: (UPPERCASE | LOWERCASE | DIGIT | ' ')+ ;
WS: [ \t\r\n]+ -> skip ; // skip whitespace
