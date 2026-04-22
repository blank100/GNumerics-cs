#if !DEBUG
using BenchmarkDotNet.Running;

using General.Benchmark;

#endif

namespace FixedNumeric.Benchmark {
	public class Program {
		public static void Main(string[] args) {
#if DEBUG
			DebugRunner.Run();
#else
			BenchmarkRunner.Run<Fixed64Benchmark>();
#endif
		}
	}
}