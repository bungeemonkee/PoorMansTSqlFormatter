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

using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoorMansTSqlFormatterRedux.Formatters;
using PoorMansTSqlFormatterRedux.Parsers;
using PoorMansTSqlFormatterRedux.Tokenizers;

namespace PoorMansTSqlFormatterRedux.Tests
{
    [TestClass]
    public class TSqlObfuscatingFormatterTests
    {
        private readonly TSqlStandardFormatterOptions _options = new TSqlStandardFormatterOptions
        {
            TrailingCommas = true,
            KeywordStandardization = true
        };

        public static IEnumerable<object[]> TestFiles => Utils.GetTestFiles();

        [TestMethod]
        [DynamicData("TestFiles")]
        public void ObfuscatingFormatReformatMatch(FileInfo inputFile, FileInfo parsedFile, FileInfo formattedFile)
        {
            var inputSql = inputFile.GetAllText();

            if (inputSql.Contains(Utils.InvalidSqlWarning))
                Assert.Inconclusive(Utils.InvalidSqlWarning);

            var tokenized = new TSqlStandardTokenizer().TokenizeSQL(inputSql);
            var parsedOriginal = new TSqlStandardParser().ParseSQL(tokenized);

            var obfuscatedSql = new TSqlObfuscatingFormatter().FormatSQLTree(parsedOriginal);
            var tokenizedAgain = new TSqlStandardTokenizer().TokenizeSQL(obfuscatedSql);
            var parsedAgain = new TSqlStandardParser().ParseSQL(tokenizedAgain);
            var unObfuscatedSql = new TSqlStandardFormatter(_options).FormatSQLTree(parsedAgain);

            Utils.StripCommentsFromSqlTree(parsedOriginal);
            var standardFormattedSql = new TSqlStandardFormatter(_options).FormatSQLTree(parsedOriginal);
            
            Assert.AreEqual(standardFormattedSql, unObfuscatedSql, "standard-formatted vs obfuscatd and reformatted");
        }
    }
}
