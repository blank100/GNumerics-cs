using System;
using BenchmarkDotNet.Attributes;

using Gal.Core;

namespace General.Benchmark {
	[MemoryDiagnoser()]
	public class Fixed64Benchmark
	{
		private const int COUNT = 100;
		
		private double[]  doubleList;
		private Fixed64[] fixed64List;

		[GlobalSetup]
		public void Setup() {
			fixed64List = new Fixed64[COUNT];
			doubleList = new double[COUNT];
			for (var i = 0; i < COUNT; i++) {
				var v = Random.Shared.Next(int.MinValue, int.MaxValue) * Random.Shared.Next(int.MinValue, int.MaxValue) / 10000d;
				fixed64List[i] = (Fixed64)v;
				doubleList[i] = v;
			}
		}

		[Benchmark(Baseline = true)]
		public void DoubleArithmetic_Add() {
			var sum = 0d;
			for (var i = 0; i < COUNT; i++) {
				for (var j = 0; j < COUNT; j++) {
					sum += doubleList[i] + doubleList[j];
				}
			}
		}
		
		[Benchmark()]
		public void Fixed64Arithmetic_Add() {
			var sum = (Fixed64) 0;
			for (var i = 0; i < COUNT; i++) {
				for (var j = 0; j < COUNT; j++) {
					sum += fixed64List[i] + fixed64List[j];
				}
			}
		}
		
		[Benchmark()]
		public void DoubleArithmetic_Sub() {
			var sum = 0d;
			for (var i = 0; i < COUNT; i++) {
				for (var j = 0; j < COUNT; j++) {
					sum += doubleList[i] - doubleList[j];
				}
			}
		}
		
		[Benchmark()]
		public void Fixed64Arithmetic_Sub() {
			var sum = (Fixed64) 0;
			for (var i = 0; i < COUNT; i++) {
				for (var j = 0; j < COUNT; j++) {
					sum += fixed64List[i] - fixed64List[j];
				}
			}
		}
		
		[Benchmark()]
		public void DoubleArithmetic_Mul() {
			// var sum = 0d;
			for (var i = 0; i < COUNT; i++) {
				for (var j = 0; j < COUNT; j++) {
					var v= doubleList[i] * doubleList[j];
				}
			}
		}
		
		[Benchmark()]
		public void Fixed64Arithmetic_Mul() {
			// var sum = (Fixed64) 0;
			for (var i = 0; i < COUNT; i++) {
				for (var j = 0; j < COUNT; j++) {
					var v = fixed64List[i] * fixed64List[j];
					// sum += fixed64List[i] * fixed64List[j];
				}
			}
		}

		[Benchmark()]
		public void DoubleArithmetic_Div() {
			var sum = 0d;
			for (var i = 0; i < COUNT; i++) {
				for (var j = 0; j < COUNT; j++) {
					sum += doubleList[i] / doubleList[j];
				}
			}
		}
		
		[Benchmark()]
		public void Fixed64Arithmetic_Div() {
			var sum = (Fixed64) 0;
			for (var i = 0; i < COUNT; i++) {
				for (var j = 0; j < COUNT; j++) {
					sum += fixed64List[i] / fixed64List[j];
				}
			}
		}
	}
}