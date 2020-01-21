﻿using CustomerTestsExcel.ExcelToCode;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomerTestsExcel.Test.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificClassGeneratorTest : TestBase
    {
        interface ITarget
        {
            int IntegerProperty { get; }
            float FloatProperty { get; set; }
            string StringProperty { get; set; }
            void StringFunction(string parameter);
            void FunctionWithoutParameter();
            void FunctionWithTwoParameters(int parameter1, int parameter2);
            double FunctionWithReturnValue(string parameter);
            IEnumerable<ITarget> IEnumerableProperty { get; }
            List<ITarget> ListProperty { get; }
            IReadOnlyList<ITarget> IReadOnlyListProperty { get; }
            ICollection<ITarget> ICollectionProperty { get; }
            ITarget ComplexProperty { get; }
        }

        [Test]
        public void SupportsSimpleProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("IntegerProperty", ExcelPropertyType.Number),
                new GivenClassSimpleProperty("FloatProperty", ExcelPropertyType.Number),
                new GivenClassSimpleProperty("StringProperty", ExcelPropertyType.String)
            );

            var actual = new SpecificationSpecificClassGenerator(
                new ExcelCsharpPropertyMatcher()
                , new CodeNameToExcelNameConverter("")
                ).cSharpCode(
                    "SampleTests",
                    new List<string> { "SampleSystemUnderTest.VermeulenNearWakeLength" },
                    typeof(ITarget),
                    excelGivenClass
                );

            var expected =
@"using static System.Reflection.MethodBase;
using Moq;
using CustomerTestsExcel;
using SampleSystemUnderTest.VermeulenNearWakeLength;

namespace SampleTests
{
    internal class SpecificationSpecificTarget : ReportsSpecificationSetup
    {
        readonly Mock<ITarget> target;

        public ITarget Target =>
            target.Object;

        public SpecificationSpecificTarget()
        {
            target = new Mock<ITarget>();
        }

        internal SpecificationSpecificTarget IntegerProperty_of(Int32 integerProperty)
        {
            valueProperties.Add(GetCurrentMethod(), integerProperty);

            target.Setup(m => m.IntegerProperty).Returns(integerProperty);

            return this;
        }

        internal SpecificationSpecificTarget FloatProperty_of(Single floatProperty)
        {
            valueProperties.Add(GetCurrentMethod(), floatProperty);

            target.Setup(m => m.FloatProperty).Returns(floatProperty);

            return this;
        }

        internal SpecificationSpecificTarget StringProperty_of(String stringProperty)
        {
            valueProperties.Add(GetCurrentMethod(), stringProperty);

            target.Setup(m => m.StringProperty).Returns(stringProperty);

            return this;
        }

    }
}
";

            IncrementallyAssertEqual(expected, actual);
        }

        void IncrementallyAssertEqual(string expected, string actual)
        {
            if (RemoveWhitespaceAndNoiseAndLowerCase(expected) != RemoveWhitespaceAndNoiseAndLowerCase(actual))
            {
                Assert.AreEqual(
                    RemoveWhitespaceAndNoiseAndLowerCase(expected),
                    RemoveWhitespaceAndNoiseAndLowerCase(actual),
                    "Expected and actual code don't match, even when all converted to lowercase and all whitespace and noise are stripped out");
            }

            if (RemoveWhitespaceAndNoise(expected) != RemoveWhitespaceAndNoise(actual))
            {
                Assert.AreEqual(
                    RemoveWhitespaceAndNoise(expected),
                    RemoveWhitespaceAndNoise(actual),
                    "Expected and actual code don't match due to a lower case / upper case problem");
            }

            if (RemoveWhitespaceAndLowerCase(expected) != RemoveWhitespaceAndLowerCase(actual))
            {
                Assert.AreEqual(
                    RemoveWhitespaceAndLowerCase(expected),
                    RemoveWhitespaceAndLowerCase(actual),
                    $"Expected and actual code don't match due to a problem with noise characters ('{noiseCharacters}')");
            }

            if (RemoveNoiseAndLowerCase(expected) != RemoveNoiseAndLowerCase(actual))
            {
                Assert.AreEqual(
                    RemoveNoiseAndLowerCase(expected),
                    RemoveNoiseAndLowerCase(actual),
                    $"Expected and actual code don't match due to a whitespace problem");
            }

            if (RemoveWhitespace(expected) != RemoveWhitespace(actual))
            {
                Assert.AreEqual(
                    RemoveWhitespace(expected),
                    RemoveWhitespace(actual),
                    $"Expected and actual code don't match due to a problem with casing or noise characters ('{noiseCharacters}')");
            }

            if (RemoveNoise(expected) != RemoveNoise(actual))
            {
                Assert.AreEqual(
                    RemoveNoise(expected),
                    RemoveNoise(actual),
                    "Expected and actual code don't match due to a problem with casing or whitespace");
            }

            Assert.AreEqual(
                expected,
                actual,
                "Expected and actual code don't match, and it isn't due to noise, casing or whitespace characters");
        }

        string RemoveWhitespaceAndNoiseAndLowerCase(string value) =>
            RemoveWhitespaceAndNoise(value.ToLowerInvariant());

        string RemoveNoiseAndLowerCase(string value) =>
            RemoveNoise(value.ToLowerInvariant());

        string RemoveWhitespaceAndLowerCase(string value) =>
            RemoveWhitespace(value.ToLowerInvariant());

        string RemoveWhitespaceAndNoise(string value) =>
            RemoveWhitespace(RemoveNoise(value));

        string RemoveWhitespace(string value) =>
            Regex.Replace(value, @"\s", "");

        string noiseCharacters =>
            @"(){};,=>.<:";

        string RemoveNoise(string value)
        {
            string removableChars = Regex.Escape(noiseCharacters);
            string pattern = "[" + removableChars + "]";
            return Regex.Replace(value, pattern, "");
        }
    }
}
