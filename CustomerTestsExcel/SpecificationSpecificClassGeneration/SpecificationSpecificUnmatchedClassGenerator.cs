﻿using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificUnmatchedClassGenerator : SpecificationSpecificClassGeneratorBase
    {
        public SpecificationSpecificUnmatchedClassGenerator(ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher, GivenClass excelGivenClass)
            : base(excelCsharpPropertyMatcher, excelGivenClass)
        {
        }

        public string CsharpCode(
            string testNamespace,
            IEnumerable<string> usings)
        {
            var usingStatements = UsingStatements(usings);

            var functions =
                excelGivenClass
                .Functions
                .Select(Function);

            var simplePropertyDeclarations = 
                excelGivenClass
                .SimpleProperties
                .Select(SimplePropertyDeclarationOnSelf);

            var simpleProperties =
                excelGivenClass
                .SimpleProperties
                .Select(SimplePropertySetterOnSelf);

            var complexPropertyDeclarations =
                excelGivenClass
                .ComplexProperties
                .Select(ComplexPropertyDeclarationOnSelf);

            var complexProperties = 
                excelGivenClass
                .ComplexProperties
                .Select(ComplexPropertySetterOnSelf);

            var listPropertyDeclarations =
                excelGivenClass
                .ComplexListProperties
                .Select(ListPropertyDeclarationOnSelf);


            var listPropertyInitialisers =
                excelGivenClass
                .ComplexListProperties
                .Select(ListPropertyInitialisationOnSelf);

            var listPropertyFunctions = 
                excelGivenClass
                .ComplexListProperties
                .Select(ListPropertySetterOnSelf);

            var code = 
$@"{usingStatements}

namespace {testNamespace}.Setup
{{
    // A class with the name {excelGivenClass.Name} was found in the Excel tests, but
    // no matching interface could be found in the assembliesUnderTest that were
    // specified on the command line parameters to GenerateCodeFromExcelTest. This file
    // is here so that the generated code can compile, to provide an example of
    // the custom file that you need to create, and point out anything that would stop
    // it matching an interface in your system under test.
    
    // The easiest thing to do is to make the names in the Excel match the names of your
    // Interfaces and their properties, and let the framework generate the Specification 
    // Specific setup classes for you. However you can write custom ones if you would
    // prefer, and they do allow you do some more esoteric things, such as instantiating
    // classes instead of mocking interfaces.
    // You can see examples of excel tests and matching interfaces in the SampleTests
    // and SampleSystemUnderTest folders of the framework repository
    // - https://github.com/resgroup/customer-tests-excel/tree/master/SampleTests
    // - https://github.com/resgroup/customer-tests-excel/tree/master/SampleSystemUnderTest

    // Custom classes should go in the 'Setup' folder.
    // If the custom class filename is '{SpecificationSpecificClassName}Override.cs',
    // then it will be used instead of this function. If it is called something else,
    // say '{SpecificationSpecificClassName}Partial.cs', then this class will remain, and
    // the custom class can add to it.

    // Please see VermeulenNearWakeLengthInput.cs for an example of setting up simple and
    // complex properties
    // - https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/Setup/VermeulenNearWakeLengthInput.cs
    // Please see Group.cs for an example of setting up list / table properties
    // - https://github.com/resgroup/customer-tests-excel/blob/master/SampleTests/Setup/Cargo.cs

    public partial class {SpecificationSpecificClassName} : ReportsSpecificationSetup
    {{
{string.Join(NewLine, simplePropertyDeclarations)}

{string.Join(NewLine, complexPropertyDeclarations)}

{string.Join(NewLine, listPropertyDeclarations)}

        public {SpecificationSpecificClassName}()
        {{
{string.Join(NewLine, listPropertyInitialisers)}
        }}

{string.Join(NewLine, functions)}

{string.Join(NewLine, simpleProperties)}

{string.Join(NewLine, complexProperties)}

{string.Join(NewLine, listPropertyFunctions)}
    }}
}}
";

            return RemoveConsecutiveBlankLines(code);
        }

        string Function(IGivenClassProperty excelGivenProperty)
        {
            var functionName = excelGivenProperty.Name;

            return 
$@"        // No sensible implementation can be generated for functions, so please 
        // add the function below in a (partial) custom class.
        // public void {functionName}() {{ .. }} ";
        }

    }
}