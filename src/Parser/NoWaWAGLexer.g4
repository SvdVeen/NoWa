lexer grammar NoWaWAGLexer;

// Fragments for reuse within the lexer grammar.
fragment Uppercase		: [A-Z]		; // Any uppercase character.
fragment Lowercase		: [a-z]		; // Any lowercase character.
fragment Digit			: [0-9]		; // Any digit.
fragment Separator		: ',' | '.'	; // A decimal separator.


// Literals to be used by the parser.
AMPERSAND		: '&'		; // An ampersand.
ARROW			: '>'		; // A right-facing arrowhead.
BRACEOPEN		: '{'		; // An opening brace.
BRACECLOSE		: '}'		; // A closing brace.
CHAR			: Lowercase	; // A single lowercase character.
COMMA			: ','		; // A comma.
DASH			: '-'		; // A dash.
DOLLAR			: '$'		; // A dollar sign.
QUOTE			: '\''		; // A single quote.
SEMICOLON		: ';'		; // A semicolon.
WS				: ' '		; // A single space.

DECIMAL		: Digit+ (Separator Digit+)?		; // A decimal number. Takes precedence in the lexer grammar so numbers without decimals are always parsed as decimals.
ALPHA		: (Uppercase | Lowercase | Digit)+	; // Any alphanumeric string.
NEWLINE		: [\r\n]+ -> skip					; // Line breaks, skipped.