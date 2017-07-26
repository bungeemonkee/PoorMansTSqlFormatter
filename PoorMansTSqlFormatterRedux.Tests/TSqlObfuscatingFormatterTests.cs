/*
Poor Man's T-SQL Formatter - a small free Transact-SQL formatting 
library for .Net 2.0, written in C#. 
Copyright (C) 2011-2013 Tao Klerks

Additional Contributors:
 * Timothy Klenke, 2012

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoorMansTSqlFormatterRedux.Formatters;
using PoorMansTSqlFormatterRedux.Interfaces;
using PoorMansTSqlFormatterRedux.Parsers;
using PoorMansTSqlFormatterRedux.Tokenizers;

namespace PoorMansTSqlFormatterRedux.Tests
{
    [TestClass]
    [Ignore]
    public class TSqlObfuscatingFormatterTests
    {

        ISqlTokenizer _tokenizer;
        ISqlTokenParser _parser;
        TSqlStandardFormatter _standardFormatter;
        TSqlObfuscatingFormatter _obfuscatingFormatter;

        public TSqlObfuscatingFormatterTests()
        {
            _tokenizer = new TSqlStandardTokenizer();
            _parser = new TSqlStandardParser();
            _standardFormatter = new TSqlStandardFormatter(new TSqlStandardFormatterOptions
                {
                    TrailingCommas = true,
                    KeywordStandardization = true
                });
            _obfuscatingFormatter = new TSqlObfuscatingFormatter();
        }

        [TestMethod, DataSource("PoorMansTSqlFormatterTests.Utils.GetInputSqlFileNames")]
        public void ObfuscatingFormatReformatMatch(string FileName)
        {
            var inputSQL = Utils.GetTestMethodFileContent(FileName, Utils.INPUTSQLFOLDER);
            var tokenized = _tokenizer.TokenizeSQL(inputSQL);
            var parsedOriginal = _parser.ParseSQL(tokenized);

            var obfuscatedSql = _obfuscatingFormatter.FormatSQLTree(parsedOriginal);
            var tokenizedAgain = _tokenizer.TokenizeSQL(obfuscatedSql);
            var parsedAgain = _parser.ParseSQL(tokenizedAgain);
            var unObfuscatedSql = _standardFormatter.FormatSQLTree(parsedAgain);

            Utils.StripCommentsFromSqlTree(parsedOriginal);
            var standardFormattedSql = _standardFormatter.FormatSQLTree(parsedOriginal);

            if (!inputSQL.Contains(Utils.INVALID_SQL_WARNING))
            {
                Assert.AreEqual(standardFormattedSql, unObfuscatedSql, "standard-formatted vs obfuscatd and reformatted");
            }
        }
    }
}
