/*
Poor Man's T-SQL Formatter - a small free Transact-SQL formatting 
library for .Net 2.0, written in C#. 
Copyright (C) 2011 Tao Klerks

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
using PoorMansTSqlFormatterRedux.Parsers;
using PoorMansTSqlFormatterRedux.Tokenizers;

namespace PoorMansTSqlFormatterRedux.Tests
{
    [TestClass]
    public class TSqlStandardParserTests
    {
        public static IEnumerable<object[]> TestFiles => Utils.GetTestFiles();

        [TestMethod]
        [DynamicData("TestFiles")]
        public void ExpectedParseTree(FileInfo inputFile, FileInfo parsedFile, FileInfo formattedFile)
        {
            if (parsedFile == null)
                Assert.Inconclusive("No parsed sql file for this input file.");
            
            var expectedText = parsedFile.GetAllText();

            var inputSql = inputFile.GetAllText();

            var tokenized = new TSqlStandardTokenizer().TokenizeSQL(inputSql);
            var parsed = new TSqlStandardParser().ParseSQL(tokenized);

            Assert.AreEqual(expectedText, parsed.OuterXml);
        }
    }
}