﻿using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class MissingWithPropertiesForAssertionTable : TestBase
    {
        [Test]
        public void SheetConverterShowsErrorIfWithPropertiesCellMissingForAssertionTable()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            var worksheet = FirstWorksheet(@"TestExcelFiles\MissingWithPropertiesForAssertionTable\MissingWithPropertiesForAssertionTable.xlsx");

            string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, worksheet, ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

            StringAssert.Contains("assertion table starting at B8", generatedCode);

            StringAssert.Contains("D9 should be 'With Properties', but is 'AnyProperty'", generatedCode);
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfWithPropertiesCellMissingForAssertionTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\MissingWithPropertiesForAssertionTable\");

            Assert.AreNotEqual(0, results.ErrorCode);

            StringAssert.Contains("Workbook 'MissingWithPropertiesForAssertionTable'", results.LogMessages); 

            StringAssert.Contains("Worksheet 'NoWithPropertiesAssertionTable'", results.LogMessages);

            StringAssert.Contains("assertion table starting at B8", results.LogMessages);

            StringAssert.Contains("D9 should be 'With Properties', but is 'AnyProperty'", results.LogMessages);
        }
    }
}