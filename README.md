# ELAN
Tools for processing ELAN annotation files.

At this moment, this project consists of the following tools:
- Parser for ELAN annotation files \*.eaf (XML). Parser creates a tree of objects in memory from annotations. Now, this parser is not universal (the parser knows the specific features of the tiers);
- An application that creates SQL-insert commands for MS SQL server database from a tree of objects (from annotations).
