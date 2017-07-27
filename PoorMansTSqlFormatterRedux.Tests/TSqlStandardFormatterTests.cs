/*
Poor Man's T-SQL Formatter - a small free Transact-SQL formatting 
library for .Net 2.0, written in C#. 
Copyright (C) 2011 Tao Klerks

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
    public class TSqlStandardFormatterTests
    {
        public static IEnumerable<object[]> TestFiles => Utils.GetTestFiles();

        [TestMethod]
        [DynamicData("TestFiles")]
        public void StandardFormatReparsingReformatting(FileInfo inputFile, FileInfo parsedFile, FileInfo formattedFile)
        {
            if (formattedFile == null)
                Assert.Inconclusive("No formatted sql file for this input file.");

            var inputSql = inputFile.GetAllText();

            if (inputSql.Contains(Utils.InvalidSqlWarning))
                Assert.Inconclusive(Utils.InvalidSqlWarning);

            if (inputSql.Contains(Utils.ReformattingInconsistencyWarning))
                Assert.Inconclusive(Utils.ReformattingInconsistencyWarning);

            var options = Utils.GetConfig(inputFile.Name);
            var tokenized = new TSqlStandardTokenizer().TokenizeSQL(inputSql);
            var parsed = new TSqlStandardParser().ParseSQL(tokenized);
            var outputSql = new TSqlStandardFormatter(options).FormatSQLTree(parsed);
            var tokenizedAgain = new TSqlStandardTokenizer().TokenizeSQL(outputSql);
            var parsedAgain = new TSqlStandardParser().ParseSQL(tokenizedAgain);
            var formattedAgain = new TSqlStandardFormatter(options).FormatSQLTree(parsedAgain);

            Utils.StripWhiteSpaceFromSqlTree(parsed);
            Utils.StripWhiteSpaceFromSqlTree(parsedAgain);

            Assert.AreEqual(outputSql, formattedAgain, "first-pass formatted vs reformatted");
            Assert.AreEqual(parsed.OuterXml.ToLower(), parsedAgain.OuterXml.ToLower(), "first parse xml vs reparse xml");
        }

        [TestMethod]
        [DynamicData("TestFiles")]
        public void StandardFormatExpectedOutput(FileInfo inputFile, FileInfo parsedFile, FileInfo formattedFile)
        {
            if (formattedFile == null)
                Assert.Inconclusive("No formatted sql file for this input file.");

            var inputSql = inputFile.GetAllText();

            if (inputSql.Contains(Utils.InvalidSqlWarning))
                Assert.Inconclusive(Utils.InvalidSqlWarning);

            if (inputSql.Contains(Utils.ReformattingInconsistencyWarning))
                Assert.Inconclusive(Utils.ReformattingInconsistencyWarning);

            var expectedSql = formattedFile.GetAllText();
            var options = Utils.GetConfig(inputFile.Name);

            var tokenized = new TSqlStandardTokenizer().TokenizeSQL(inputSql);
            var parsed = new TSqlStandardParser().ParseSQL(tokenized);
            var formatted = new TSqlStandardFormatter(options).FormatSQLTree(parsed);

            Assert.AreEqual(expectedSql, formatted);
        }
    }
}
