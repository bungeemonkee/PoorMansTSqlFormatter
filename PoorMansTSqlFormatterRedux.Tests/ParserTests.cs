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
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoorMansTSqlFormatterRedux.Parsers;
using PoorMansTSqlFormatterRedux.Tokenizers;

namespace PoorMansTSqlFormatterRedux.Tests
{
    [TestClass]
    [Ignore]
    public class ParserTests
    {
        public TestContext Context { get; set; }

        public IEnumerable<string> GetParsedSqlFileNames()
        {
            return Utils.FolderFileNameIterator(Utils.GetTestMethodContentFolder(Utils.PARSEDSQLFOLDER));
        }

        [TestMethod, DataSource("GetParsedSqlFileNames")]
        public void ExpectedParseTree(string fileName)
        {
            var path = Path.Combine(Utils.GetTestMethodContentFolder(Utils.PARSEDSQLFOLDER), fileName);
            var expectedXmlDoc = new XmlDocument();
            expectedXmlDoc.PreserveWhitespace = true;
            using (var stream = File.OpenRead(path))
            {
                expectedXmlDoc.Load(stream);
            }

            var inputSql = Utils.GetTestMethodFileContent(fileName, Utils.INPUTSQLFOLDER);

            var tokenized = new TSqlStandardTokenizer().TokenizeSQL(inputSql);
            var parsed = new TSqlStandardParser().ParseSQL(tokenized);

            Assert.AreEqual(expectedXmlDoc.OuterXml, parsed.OuterXml);
        }
    }
}
