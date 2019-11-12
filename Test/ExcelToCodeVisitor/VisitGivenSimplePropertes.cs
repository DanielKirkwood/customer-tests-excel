﻿using System;
using System.Collections.Generic;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    [TestFixture]
    public class VisitGivenSimpleProperties : TestBase
    {
        [Test]
        public void ExcelToCodeVisitsSimpleProperties()
        {
            var visitRecorder = new GivenSimplePropertyVisitRecorder();

            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));
            sheetConverter.AddVisitor(visitRecorder);

            using (var workbook = Workbook(@"TestExcelFiles\PropertyTypes.xlsx"))
            {
                sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                AssertContains(visitRecorder, "Null of", "null", ExcelPropertyType.Null);
                AssertContains(visitRecorder, "Null of", "null", ExcelPropertyType.Null);
                AssertContains(visitRecorder, "StringNull of", "null", ExcelPropertyType.StringNull);
                AssertContains(visitRecorder, "DateTime of", "DateTime.Parse(\"2019-01-01T00:00:00\")", ExcelPropertyType.DateTime);
                AssertContains(visitRecorder, "TimeSpan of", "TimeSpan.Parse(\"01:15:00\")", ExcelPropertyType.Timespan);
                AssertContains(visitRecorder, "Enum of", "EnumType.EnumValue", ExcelPropertyType.Enum);
                AssertContains(visitRecorder, "Number of", "1", ExcelPropertyType.Number);
                AssertContains(visitRecorder, "Decimal of", "1m", ExcelPropertyType.Decimal);
                AssertContains(visitRecorder, "False of", "false", ExcelPropertyType.Boolean);
                AssertContains(visitRecorder, "True of", "true", ExcelPropertyType.Boolean);
                AssertContains(visitRecorder, "String of", "\"hello\"", ExcelPropertyType.String);
                AssertContains(visitRecorder, "QuotedString of", "\"1\"", ExcelPropertyType.String);
            }
        }

        static void AssertContains(
            GivenSimplePropertyVisitRecorder visitRecorder,
            string propertyOrFunctionName,
            string cSharpCodeRepresentation,
            ExcelPropertyType excelPropertyType)
            =>
            CollectionAssert.Contains(
                visitRecorder.RecordedSimpleProperties,
                new GivenSimpleProperty(
                    propertyOrFunctionName,
                    cSharpCodeRepresentation,
                    excelPropertyType
                )
           );
    }
}