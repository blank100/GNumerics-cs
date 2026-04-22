using System;
using Gal.Core;

namespace FixedNumeric.Benchmark
{
    public static class DebugRunner
    {
        public static void Run() {
            Console.WriteLine($"PI = new({Fixed40_24.PI.Raw}); //{Fixed40_24.PI}");
            Console.WriteLine($"LN2 = new({Fixed40_24.LN2.Raw}); //{Fixed40_24.LN2}");

            Console.WriteLine($"SPadeA1 = new({Fixed40_24.SPadeA1.Raw}); //{Fixed40_24.SPadeA1}");
            Console.WriteLine($"SPadeA2 = new({Fixed40_24.SPadeA2.Raw}); //{Fixed40_24.SPadeA2};");

            Console.WriteLine($"Deg2Rad = new({Fixed40_24.Deg2Rad.Raw}); //{Fixed40_24.Deg2Rad}");
            Console.WriteLine($"Rad2Deg = new({Fixed40_24.Rad2Deg.Raw}); //{Fixed40_24.Rad2Deg}");

            Console.WriteLine($"SSinCoeff3 = new({Fixed40_24.SSinCoeff3.Raw}); //{Fixed40_24.SSinCoeff3}");
            Console.WriteLine($"SSinCoeff5 = new({Fixed40_24.SSinCoeff5.Raw}); //{Fixed40_24.SSinCoeff5};");
            Console.WriteLine($"SSinCoeff7 = new({Fixed40_24.SSinCoeff7.Raw}); //{Fixed40_24.SSinCoeff7};");

            // int COUNT = 200;
            // var fixed64List = new Fixed64[COUNT];
            // var doubleList = new double[COUNT];
            // for (var i = 0; i < COUNT; i++) {
            //     var v = Pcg.Default.Next(int.MinValue, int.MaxValue) / 10000d;
            //     fixed64List[i] = (Fixed64)v;
            //     doubleList[i] = v;
            // }
            //
            // var sum = (Fixed64)0;
            // for (var i = 0; i < COUNT; i++) {
            //     for (var j = 0; j < COUNT; j++) {
            //         var v = fixed64List[i] + fixed64List[j];
            //         // sum += fixed64List[i] * fixed64List[j];
            //     }
            // }
        }
    }
}
