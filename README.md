# Lexeme ELAN Parser
Tools for processing ELAN annotation files.

At this moment, this project consists of the following tools:
- Parser for ELAN annotation files \*.eaf (XML). Parser creates a tree of objects in memory from annotations. Now, this parser is not universal (the parser knows the specific features of the tiers);
- An application that creates SQL-insert commands for MS SQL server database from a tree of objects (from annotations).

**The input to this parser are a \*.eaf (XML) files.**
Examples of files that the parser actually parsed: https://github.com/ubaleht/SiberianIngrianFinnish/tree/master/annotations

**The output of the parser:** SQL-commands to add rows to the database (the parser opens a connection to the database and executes the commands).
Examples of saved data in the database after the parser has executed the commands (saved in \*.sql (txt) files): https://github.com/ubaleht/SiberianIngrianFinnish/tree/master/SpeechDatabase/Data
Scheme of this database: https://github.com/ubaleht/SiberianIngrianFinnish/tree/master/SpeechDatabase/Scheme
