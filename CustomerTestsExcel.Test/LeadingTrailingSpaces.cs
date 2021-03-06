﻿using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class LeadingTrailingSpaces : TestBase
    {
        string generatedCode;
        ITabularBook workbook;

        [SetUp]
        public void SetUp()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            workbook = Workbook(@"TestExcelFiles\Leading Trailing Spaces .xlsx");

            generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME).Code;
        }


        [TearDown]
        public void TearDown()
        {
            workbook.Dispose();
        }

        [Test]
        public void SheetConverterTrimsLeadingSpacesFromProperties() =>
            StringAssert.DoesNotContain("_Property-", generatedCode);

        [Test]
        public void SheetConverterTrimsTrailingSpacesFromProperties() =>
            StringAssert.Contains("Property_of", generatedCode);

        [Test]
        public void SheetConverterConvertsEnclosedSpacesInPropertiesToUnderscores() =>
            StringAssert.Contains("Property-_-Property", generatedCode);

        [Test]
        public void SheetConverterTrimsLeadingSpacesFromListProperties() =>
            StringAssert.DoesNotContain("_ListProperty-", generatedCode);

        [Test]
        public void SheetConverterTrimsTrailingSpacesFromListProperties() =>
            StringAssert.Contains("ListProperty_list_of", generatedCode);

        [Test]
        public void SheetConverterConvertsEnclosedSpacesInListPropertiesToUnderscores() =>
            StringAssert.Contains("ListProperty-_-ListProperty", generatedCode);

        [Test]
        public void SheetConverterTrimsLeadingSpacesFromTableProperties() =>
            StringAssert.DoesNotContain("_TableProperty-", generatedCode);

        [Test]
        public void SheetConverterTrimsTrailingSpacesFromTableProperties() =>
            StringAssert.Contains("TableProperty_table_of", generatedCode);

        [Test]
        public void SheetConverterConvertsEnclosedSpacesInTablePropertiesToUnderscores() =>
            StringAssert.Contains("TableProperty-_-TableProperty", generatedCode);

        [Test]
        public void SheetConverterTrimsLeadingSpacesFromPropertyClassNames() =>
        StringAssert.DoesNotContain("_TableClass-", generatedCode);

        [Test]
        public void SheetConverterTrimsTrailingSpacesFromPropertyClassNames() =>
            StringAssert.Contains("-TableClass", generatedCode);

        [Test]
        public void SheetConverterConvertsEnclosedSpacesInPropertyClassNamesToUnderscores() =>
            StringAssert.Contains("TableClass-_-TableClass", generatedCode);

        [Test]
        public void SheetConverterTrimsLeadingSpacesFromAction() =>
            StringAssert.DoesNotContain("_Calculate-", generatedCode);

        [Test]
        public void SheetConverterTrimsTrailingSpacesFromAction() =>
            StringAssert.DoesNotContain("-Calculate_", generatedCode);

        [Test]
        public void SheetConverterConvertsEnclosedSpacesInActionToUnderscores() =>
            StringAssert.Contains("Calculate-_-Calculate", generatedCode);

        [Test]
        public void SheetConverterTrimsLeadingSpacesFromAssertions() =>
            StringAssert.DoesNotContain("_Assertion-", generatedCode);

        [Test]
        public void SheetConverterTrimsTrailingSpacesFromAssertions() =>
            StringAssert.DoesNotContain("-Assertion_", generatedCode);

        [Test]
        public void SheetConverterConvertsEnclosedSpacesInAssertionsToUnderscores() =>
            StringAssert.Contains("Assertion-_-Assertion", generatedCode);

    }
}
