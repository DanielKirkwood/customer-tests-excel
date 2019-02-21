﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CustomerTestsExcel;
using System.IO;
using CustomerTestsExcel.ExcelToCode;

namespace CustomerTestsExcel.Test
{
    public class TestBase
    {
        protected const string ANY_STRING = "asdkasdj";
        protected const string ANY_ROOT_NAMESPACE = "asdkasdj";
        protected const string ANY_WORKBOOKNAME = "asdkasdj";
        protected readonly IEnumerable<string> NO_USINGS = new List<String>();

        protected TestBase()
        {
        }

        protected ITabularPage FirstWorksheet(string excelFileNameRelativeToOutputFolder)
        {
            var excel = new ExcelTabularLibrary();

            using (var workbookFile = GetExcelFileStream(Path.Combine(TestContext.CurrentContext.TestDirectory, excelFileNameRelativeToOutputFolder)))
            {
                using (var workbook = excel.OpenBook(workbookFile))
                {
                    return workbook.GetPage(0);
                }
            }
        }

        Stream GetExcelFileStream(string excelFile, string temporaryFolder = null)
        {
            var templateStream = new MemoryStream();

            if (string.IsNullOrEmpty(temporaryFolder))
                temporaryFolder = Path.GetTempPath();

            var tempFileName = Path.Combine(temporaryFolder, Guid.NewGuid().ToString("N") + ".tmp");

            try
            {
                File.Copy(excelFile, tempFileName, true);

                var attr = File.GetAttributes(tempFileName);
                File.SetAttributes(tempFileName, (attr | FileAttributes.Temporary) & ~FileAttributes.ReadOnly);

                using (var fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read))
                {
                    fs.CopyTo(templateStream);
                }
            }
            finally
            {
                File.Delete(tempFileName);
            }

            templateStream.Seek(0, SeekOrigin.Begin);
            return templateStream;
        }

        protected TestLogger GenerateTestsAndReturnLog(string specificationFolderRelativeToOutputFolder)
        {
            var logger = new TestLogger();

            TestProjectCreator.Create(
                Path.Combine(TestContext.CurrentContext.TestDirectory, specificationFolderRelativeToOutputFolder),
                "DummyProject.csproj",
                "",
                ANY_ROOT_NAMESPACE,
                NO_USINGS,
                ANY_STRING,
                new ExcelTabularLibrary(),
                logger);

            return logger;
        }
    }
}