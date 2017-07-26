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

using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoorMansTSqlFormatterRedux.Formatters;
using PoorMansTSqlFormatterRedux.Interfaces;
using PoorMansTSqlFormatterRedux.Parsers;
using PoorMansTSqlFormatterRedux.Tokenizers;

namespace PoorMansTSqlFormatterRedux.Tests
{
    [TestClass]
    public class TSqlStandardFormatterTests
    {
        ISqlTokenizer _tokenizer;
        ISqlTokenParser _parser;
        //TSqlStandardFormatter _treeFormatter;
        Dictionary<string, TSqlStandardFormatter> _formatters;

        public TSqlStandardFormatterTests()
        {
            _tokenizer = new TSqlStandardTokenizer();
            _parser = new TSqlStandardParser();
            _formatters = new Dictionary<string, TSqlStandardFormatter>(StringComparer.OrdinalIgnoreCase);
        }

        private TSqlStandardFormatter GetFormatter(string configString)
        {
            TSqlStandardFormatter outFormatter;
            if (!_formatters.TryGetValue(configString, out outFormatter))
            {
                //defaults are as per the object, except disabling colorized/htmlified output
                var options = new TSqlStandardFormatterOptions(configString);
                options.HTMLColoring = false;
                outFormatter = new TSqlStandardFormatter(options);
            }
            return outFormatter;
        }

        [TestMethod, DataSource("PoorMansTSqlFormatterTests.Utils.GetInputSqlFileNames")]
        public void StandardFormatReparsingReformatting(string FileName)
        {
            var inputSQL = Utils.GetTestMethodFileContent(FileName, Utils.INPUTSQLFOLDER);
            var _treeFormatter = GetFormatter("");
            var tokenized = _tokenizer.TokenizeSQL(inputSQL);
            var parsed = _parser.ParseSQL(tokenized);
            var outputSQL = _treeFormatter.FormatSQLTree(parsed);
            var tokenizedAgain = _tokenizer.TokenizeSQL(outputSQL);
            var parsedAgain = _parser.ParseSQL(tokenizedAgain);
            var formattedAgain = _treeFormatter.FormatSQLTree(parsedAgain);
            if (!inputSQL.Contains(Utils.REFORMATTING_INCONSISTENCY_WARNING) && !inputSQL.Contains(Utils.INVALID_SQL_WARNING))
            {
                Assert.AreEqual(outputSQL, formattedAgain, "first-pass formatted vs reformatted");
                Utils.StripWhiteSpaceFromSqlTree(parsed);
                Utils.StripWhiteSpaceFromSqlTree(parsedAgain);
                Assert.AreEqual(parsed.OuterXml.ToUpper(), parsedAgain.OuterXml.ToUpper(), "first parse xml vs reparse xml");
            }
        }

        public IEnumerable<string> GetStandardFormatSqlFileNames()
        {
            return Utils.FolderFileNameIterator(Utils.GetTestMethodContentFolder("StandardFormatSql"));
        }

        [TestMethod, DataSource("GetStandardFormatSqlFileNames")]
        public void StandardFormatExpectedOutput(string FileName)
        {
            var expectedSql = Utils.GetTestMethodFileContent(FileName, Utils.STANDARDFORMATSQLFOLDER);
            var inputSql = Utils.GetTestMethodFileContent(Utils.StripFileConfigString(FileName), Utils.INPUTSQLFOLDER);
            var _treeFormatter = GetFormatter(Utils.GetFileConfigString(FileName));

            var tokenized = _tokenizer.TokenizeSQL(inputSql);
            var parsed = _parser.ParseSQL(tokenized);
            var formatted = _treeFormatter.FormatSQLTree(parsed);

            Assert.AreEqual(expectedSql, formatted);
        }

    }
}
