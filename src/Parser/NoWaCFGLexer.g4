lexer grammar NoWaCFGLexer;

// Fragments for reuse within the lexer grammar.
fragment Uppercase		: [A-Z]	; // Any uppercase character.
fragment Lowercase		: [a-z]	; // Any lowercase character.
fragment Digit			: [0-9]	; // Any digit.


// Literals to be used by the parser.
DASH		: '-'	; // A dash.
EQUALS		: '='	; // An equals sign.
PIPE		: '|'	; // A pipe, used to indicate the 'or'.
QUOTE		: '\''	; // A single quote.
SEMICOLON	: ';'	; // A semicolon, terminates rules.
WS			: ' '	; // A single space.

ALPHA	: (Uppercase | Lowercase | Digit)+	; // Any alphanumeric string.
NEWLINE	: [\r\n]+ -> skip					; // skip line breaks
