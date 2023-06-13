parser grammar NoWaParser;
options { tokenVocab = NoWaLexer; }

grammar_: rules=rule+ EOF ;
rule: SYM+ PROD SYM+ TERM ;
