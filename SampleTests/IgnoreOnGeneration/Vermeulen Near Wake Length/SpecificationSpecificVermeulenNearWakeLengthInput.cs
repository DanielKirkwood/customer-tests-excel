﻿using System;
using CustomerTestsExcel;
using SampleSystemUnderTest.VermeulenNearWakeLength;

namespace SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length
{
    internal class SpecificationSpecificVermeulenNearWakeLengthInput : ReportsSpecificationSetup, IVermeulenNearWakeLengthInput
    {
        public double Velocity_mps { get; private set; }
        public double AmbientTurbuluence { get; private set; }
        public double RevolutionsPerMinute { get; private set; }
        public double ThrustCoefficient { get; private set; }
        public ITurbineGeometry TurbineGeometry { get; private set; }

        internal SpecificationSpecificVermeulenNearWakeLengthInput Velocity_of(int velocity_mps)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, velocity_mps);

            this.Velocity_mps = velocity_mps;

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInput Ambient_Turbulence_of(double ambientTurbuluence)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ambientTurbuluence);

            this.AmbientTurbuluence = ambientTurbuluence;

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInput RevolutionsPerMinute_of(double revolutionsPerMinute)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, revolutionsPerMinute);

            this.RevolutionsPerMinute = revolutionsPerMinute;

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInput Thrust_Coefficient_of(double thrustCoefficient)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, thrustCoefficient);

            this.ThrustCoefficient = thrustCoefficient;

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInput TurbineGeometry_of(SpecificationSpecificTurbineGeometry turbineGeometry)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, turbineGeometry);

            this.TurbineGeometry = turbineGeometry;

            return this;
        }
    }
}