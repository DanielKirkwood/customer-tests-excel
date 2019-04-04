﻿using CustomerTestsExcel;
using SampleSystemUnderTest.Calculator;
using System;

namespace SampleTests.IgnoreOnGeneration.Calculator
{
    public class SpecificationSpecificCalculator : ReportsSpecificationSetup, ICalculation
    {
        public double FirstValue { get; internal set; }
        public double SecondValue { get; internal set; }
        public Operation Operation { get; internal set; }
        public double Result { get; internal set; }

        internal void FirstValue_of(double firstValue)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, firstValue);

            FirstValue = firstValue;
        }

        internal void SecondValue_of(double secondValue)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, secondValue);

            SecondValue = secondValue;
        }

        internal void Operation_of(Operation operation)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, operation);

            Operation = operation;
        }

        internal void Perform_Operation()
        {
            Result = new SampleSystemUnderTest.Calculator.Calculator().Calculate(this);
        }
    }
}