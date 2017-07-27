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

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using PoorMansTSqlFormatterRedux.Formatters;
using PoorMansTSqlFormatterRedux.Interfaces;

namespace PoorMansTSqlFormatterRedux.Tests
{
    public static class Utils
    {
        public const string InputSqlFolder = "Input";
        public const string ParsedSqlFolder = "Parsed";
        public const string StandardFormatSqlFolder = "Formatted";

        public const string InvalidSqlWarning = "THIS TestMethod FILE IS NOT VALID SQL";
        public const string ReformattingInconsistencyWarning = "KNOWN SQL REFORMATTING INCONSISTENCY";

        public static IEnumerable<object[]> GetTestFiles()
        {
            var inputFolder = Path.Combine(".", InputSqlFolder);
            var inputDirectory = new DirectoryInfo(inputFolder);

            foreach (var inputFile in inputDirectory.GetFiles())
            {
                var parsedFile = new FileInfo(Path.Combine(".", ParsedSqlFolder, inputFile.Name));
                var formattedFile = new FileInfo(Path.Combine(".", StandardFormatSqlFolder, inputFile.Name));

                yield return new object[] {inputFile, parsedFile.Exists ? parsedFile : null, formattedFile.Exists ? formattedFile : null };
            }
        }

        public static string GetAllText(this FileInfo file)
        {
            using (var fileStream = file.OpenRead())
            using (var textReader = new StreamReader(fileStream))
            {
                return textReader.ReadToEnd();
            }
        }

        public static void StripWhiteSpaceFromSqlTree(XmlDocument sqlTree)
        {
            StripElementNameFromXml(sqlTree, SqlXmlConstants.ENAME_WHITESPACE);
        }

        public static void StripCommentsFromSqlTree(XmlDocument sqlTree)
        {
            StripElementNameFromXml(sqlTree, SqlXmlConstants.ENAME_COMMENT_MULTILINE);
            StripElementNameFromXml(sqlTree, SqlXmlConstants.ENAME_COMMENT_SINGLELINE);
            StripElementNameFromXml(sqlTree, SqlXmlConstants.ENAME_COMMENT_SINGLELINE_CSTYLE);
        }

        private static void StripElementNameFromXml(XmlNode sqlTree, string elementName)
        {
            var deletionCandidates = sqlTree.SelectNodes(string.Format("//*[local-name() = '{0}']", elementName));
            foreach (XmlElement deletionCandidate in deletionCandidates)
                deletionCandidate.ParentNode.RemoveChild(deletionCandidate);
        }

        public static TSqlStandardFormatterOptions GetConfig(string fileName)
        {
            var openParens = fileName.IndexOf("(", StringComparison.Ordinal);
            if (openParens < 0) return new TSqlStandardFormatterOptions {HTMLColoring = false};

            var closeParens = fileName.IndexOf(")", openParens, StringComparison.Ordinal);
            if (closeParens < 0) return new TSqlStandardFormatterOptions {HTMLColoring = false};

            var optionsString = fileName.Substring(openParens + 1, (closeParens - openParens) - 1);
            return new TSqlStandardFormatterOptions(optionsString) {HTMLColoring = false};
        }
    }
}
