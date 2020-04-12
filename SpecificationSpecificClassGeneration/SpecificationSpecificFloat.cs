﻿using static System.Reflection.MethodBase;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificFloat : ReportsSpecificationSetup
    {
        public double Float { get; private set; }

        public SpecificationSpecificFloat Float_of(double value)
        {
            valueProperties.Add(GetCurrentMethod(), value);

            Float = value;

            return this;
        }
    }
}