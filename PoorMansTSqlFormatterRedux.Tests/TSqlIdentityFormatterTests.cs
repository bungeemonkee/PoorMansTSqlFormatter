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

using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoorMansTSqlFormatterRedux.Formatters;
using PoorMansTSqlFormatterRedux.Interfaces;
using PoorMansTSqlFormatterRedux.Parsers;
using PoorMansTSqlFormatterRedux.Tokenizers;

namespace PoorMansTSqlFormatterRedux.Tests
{
    [TestClass]
    public class TSqlIdentityFormatterTests
    {
        [TestMethod, DataSource("PoorMansTSqlFormatterTests.Utils.GetInputSqlFileNames")]
        public void ContentUnchangedByIdentityTokenFormatter(string FileName)
        {
            var inputSQL = Utils.GetTestMethodFileContent(FileName, Utils.INPUTSQLFOLDER);
            var tokenized = new TSqlStandardTokenizer().TokenizeSQL(inputSQL);
            var outputSQL = new TSqlIdentityFormatter().FormatSQLTokens(tokenized);
            if (!inputSQL.Contains(Utils.INVALID_SQL_WARNING))
                Assert.AreEqual(outputSQL, inputSQL);
        }

        [TestMethod, DataSource("PoorMansTSqlFormatterTests.Utils.GetInputSqlFileNames")]
        public void ContentUnchangedByIdentityTreeFormatter(string FileName)
        {
            var inputSQL = Utils.GetTestMethodFileContent(FileName, Utils.INPUTSQLFOLDER);
            var tokenized = new TSqlStandardTokenizer().TokenizeSQL(inputSQL);
            var parsed = new TSqlStandardParser().ParseSQL(tokenized);
            var outputSQL = new TSqlIdentityFormatter().FormatSQLTree(parsed);
            if (!inputSQL.Contains(Utils.INVALID_SQL_WARNING))
                Assert.AreEqual(outputSQL, inputSQL);
        }
    }
}
