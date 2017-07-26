
# Poor Man's T-SQL Formatter Redux

This is a port of [Poor Man's T-SQL Formatter](https://github.com/TaoK/PoorMansTSqlFormatter) to .Net Standard 1.4 so it can be used with .Net or .Net Core. Everything but the core library (console app, web app, plugins, etc.) has been removed.


## Features

* Simple Xml-based parse tree
* Extensible, with possibility of supporting other SQL dialects (but none implemented)
* Configurable according to SQL formatting preferences
* Handles "procedural" T-SQL; this is not just a SQL statement formatter, but it also formats entire batches, and multi-batch scripts.
* Fault-tolerant parsing and formatting - if some unknown SQL construct is encountered or a keyword is misinterpreted, parsing does not fail (but will simply not colorize or indent that portion correctly). If the parsing fails more catastrophically, a "best effort" will be made and warning displayed (or in the case of interactive use, eg in SSMS, the operation can be aborted).
* Reasonably fast: reformatting 1,500 or so files totalling 4MB takes 30 seconds on a cheap atom-processor (2009) netbook.


## General Limitations

* This is NOT a full SQL-parsing solution: only "coarse" parsing is performed, the minimum necessary for re-formatting.
* The standard formatter does not always maintain the order of comments in the code; a comment inside an "INNER JOIN" compound keyword, like "inner/\*test\*/join", would get moved out, to "INNER JOIN /\*test\*/". The original data is maintaned in the parse tree, but the standard formatter shuffles comments in cases like this for clarity.
* DDL parsing, in particular, is VERY coarse - the bare minimum to display ordered table column and procedure parameter declarations.
* No effort has been made to support compatibility level 70 (SQL Server 7)
* Where there is ambiguity between different compatibility levels (eg cross apply parens in compatibility level 90 vs table hints without "WITH" keyword in compatibility level 80), no approach has been decided. For now, table hints without WITH are considered to be arguments to a function.


## Known Issues / Todo

* Handling of DDL Triggers (eg "FOR LOGON")
* Formatting/indenting of ranking functions 
* FxCop checking
* And other stuff that is tracked in the GitHub issues list


## Version Numbering

Each version will have two numbers: a Windows file version and a SemVer version. The SemVer version will be derived from the Windows file version.

Versioning will follow this pattern:

* Major Version : Increased when there is a release that breaks a public api
* Minor Version : Increased for each non-api-breaking release
* Build Number : 0 for alpha versions, 1 for beta versions, 2 for release candidates, 3 for releases
* Revision : Always 0 for release versions, always 1+ for alpha, beta, rc versions to indicate the alpha/beta/rc  number (alpha/beeta/rc numbers should always be two-digits)

So the following versions are equalivalent

* 1.0.0.1 => 1.0.0-alpha01
* 1.0.1.1 => 1.0.1-beta01
* 1.0.2.1 => 1.0.2-rc01
* 1.0.3.0 => 1.0.3


### License & Credits

This application and library is released under the GNU Affero GPL v3: 
http://www.gnu.org/licenses/agpl.txt

Thanks to Tao Klerks who wrote the original (seemingly now defunct) Poor Man's T-SQL Formatter and anyone who contributed to that project.

