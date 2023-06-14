parser grammar NoWaParser;
options { tokenVocab = NoWaLexer; }

grammar_: rules=rule+ EOF ;

rule: name=nonterminal PRODUCES pattern+=symbol (OR pattern+=symbol)* TERMINATOR ;

symbol: terminal | nonterminal ;

nonterminal: ALPHA ;

terminal: QUOTE value=ALPHA QUOTE ;