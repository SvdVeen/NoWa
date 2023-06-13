lexer grammar NoWaLexer;

fragment UPPERCASE: [A-Z] ;
fragment LOWERCASE: [a-z] ;
fragment DIGIT: [0-9] ;


PROD: '=' ;
TERM: ';' ;
SYM: UPPERCASE+ ;
WS: [ \t\r\n]+ -> skip ; // skip whitespaces