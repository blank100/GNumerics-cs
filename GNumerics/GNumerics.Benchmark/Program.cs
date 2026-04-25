
using Gal.Benchmarks;
#if !DEBUG
using BenchmarkDotNet.Running;

#endif

namespace FixedNumeric.Benchmark {
	public class Program {
		public static void Main(string[] args) {
#if DEBUG
			DebugRunner.Run();
#else
            var summary1 = BenchmarkRunner.Run<Fixed64Benchmarks>();
            var summary2 = BenchmarkRunner.Run<Fixed64VsDoubleBenchmarks>();
#endif
		}
	}
}
