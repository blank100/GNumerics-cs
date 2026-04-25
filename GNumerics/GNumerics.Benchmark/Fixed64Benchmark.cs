using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Gal.Core;

namespace Gal.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(warmupCount: 3, iterationCount: 5)]
    public class Fixed64Benchmarks
    {
        private Fixed64 _value1;
        private Fixed64 _value2;
        private Fixed64 _smallValue;
        private Fixed64 _largeValue;
        private Fixed64 _angle;
        private int _intValue;
        private double _doubleValue;

        [GlobalSetup]
        public void Setup()
        {
            _value1 = (Fixed64)3.14159;
            _value2 = (Fixed64)2.71828;
            _smallValue = (Fixed64)0.0001;
            _largeValue = (Fixed64)1000000;
            _angle = Fixed64.PI / 4; // 45 degrees
            _intValue = 42;
            _doubleValue = 3.14159;
        }

        #region 类型转换基准测试

        [Benchmark]
        public Fixed64 ConvertFromInt() => (Fixed64)_intValue;

        [Benchmark]
        public Fixed64 ConvertFromDouble() => (Fixed64)_doubleValue;

        [Benchmark]
        public int ConvertToInt() => (int)_value1;

        [Benchmark]
        public double ConvertToDouble() => (double)_value1;

        [Benchmark]
        public Fixed64 ParseFromString() => (Fixed64)"3.14159265";

        #endregion

        #region 基础算术运算基准测试

        [Benchmark]
        public Fixed64 Addition() => _value1 + _value2;

        [Benchmark]
        public Fixed64 Subtraction() => _value1 - _value2;

        [Benchmark]
        public Fixed64 Multiplication() => _value1 * _value2;

        [Benchmark]
        public Fixed64 FastMultiplication() => Fixed64.FastMul(_value1, _value2);

        [Benchmark]
        public Fixed64 MultiplicationWithInt() => _value1 * _intValue;

        [Benchmark]
        public Fixed64 FastMultiplicationWithInt() => Fixed64.FastMul(_value1, _intValue);

        [Benchmark]
        public Fixed64 Division() => _value1 / _value2;

        [Benchmark]
        public Fixed64 DivisionByInt() => _value1 / _intValue;

        [Benchmark]
        public Fixed64 Modulo() => _value1 % _value2;

        [Benchmark]
        public Fixed64 Negation() => -_value1;

        #endregion

        #region 比较运算基准测试

        [Benchmark]
        public bool Equality() => _value1 == _value2;

        [Benchmark]
        public bool LessThan() => _value1 < _value2;

        [Benchmark]
        public bool GreaterThan() => _value1 > _value2;

        [Benchmark]
        public int CompareTo() => _value1.CompareTo(_value2);

        #endregion

        #region 数学函数基准测试

        [Benchmark]
        public Fixed64 Abs() => Fixed64.Abs(-_value1);

        [Benchmark]
        public Fixed64 Min() => Fixed64.Min(_value1, _value2);

        [Benchmark]
        public Fixed64 Max() => Fixed64.Max(_value1, _value2);

        [Benchmark]
        public Fixed64 Clamp() => Fixed64.Clamp(_value1, Fixed64.Zero, Fixed64.One);

        [Benchmark]
        public Fixed64 Floor() => Fixed64.Floor(_value1);

        [Benchmark]
        public Fixed64 Ceiling() => Fixed64.Ceiling(_value1);

        [Benchmark]
        public Fixed64 Round() => Fixed64.Round(_value1);

        [Benchmark]
        public Fixed64 Truncate() => Fixed64.Truncate(_value1);

        [Benchmark]
        public int Sign() => Fixed64.Sign(_value1);

        #endregion

        #region 高级数学函数基准测试

        [Benchmark]
        public Fixed64 Sqrt() => Fixed64.Sqrt(_value1);

        [Benchmark]
        public Fixed64 FastSqrt() => Fixed64.FastSqrt(_value1);

        [Benchmark]
        public Fixed64 InvSqrt() => Fixed64.InvSqrt(_value1);

        [Benchmark]
        public Fixed64 PowInt() => Fixed64.Pow(_value1, 3);

        [Benchmark]
        public Fixed64 PowFixed() => Fixed64.Pow(_value1, _value2);

        [Benchmark]
        public Fixed64 Pow2() => Fixed64.Pow2(_value1);

        [Benchmark]
        public Fixed64 Exp() => Fixed64.Exp(_value1);

        [Benchmark]
        public Fixed64 Log() => Fixed64.Log(_value1);

        [Benchmark]
        public Fixed64 Log2() => Fixed64.Log2(_value1);

        [Benchmark]
        public Fixed64 Log10() => Fixed64.Log10(_value1);

        #endregion

        #region 三角函数基准测试

        [Benchmark]
        public Fixed64 Sin() => Fixed64.Sin(_angle);

        [Benchmark]
        public Fixed64 Cos() => Fixed64.Cos(_angle);

        [Benchmark]
        public Fixed64 Tan() => Fixed64.Tan(_angle);

        [Benchmark]
        public Fixed64 Asin() => Fixed64.Asin(Fixed64.Dot5);

        [Benchmark]
        public Fixed64 Acos() => Fixed64.Acos(Fixed64.Dot5);

        [Benchmark]
        public Fixed64 Atan() => Fixed64.Atan(_value1);

        [Benchmark]
        public Fixed64 Atan2() => Fixed64.Atan2(_value1, _value2);

        #endregion

        #region 综合场景基准测试

        [Benchmark]
        public Fixed64 ComplexCalculation()
        {
            // 模拟一个复杂的物理计算场景
            var a = _value1 * _value2;
            var b = Fixed64.Sqrt(a);
            var c = Fixed64.Sin(_angle);
            return b + c;
        }

        [Benchmark]
        public Fixed64 VectorMagnitude()
        {
            // 计算 2D 向量长度
            var x = _value1;
            var y = _value2;
            return Fixed64.Sqrt(x * x + y * y);
        }

        [Benchmark]
        public Fixed64 VectorMagnitudeFast()
        {
            // 使用快速平方根计算 2D 向量长度
            var x = _value1;
            var y = _value2;
            return Fixed64.FastSqrt(x * x + y * y);
        }

        [Benchmark]
        public Fixed64 RotationCalculation()
        {
            // 模拟旋转计算
            var cos = Fixed64.Cos(_angle);
            var sin = Fixed64.Sin(_angle);
            return _value1 * cos - _value2 * sin;
        }

        #endregion
    }

    // 对比测试：Fixed64 vs double
    [MemoryDiagnoser]
    [SimpleJob(warmupCount: 3, iterationCount: 5)]
    public class Fixed64VsDoubleBenchmarks
    {
        private Fixed64 _fixed1;
        private Fixed64 _fixed2;
        private double _double1;
        private double _double2;

        [GlobalSetup]
        public void Setup()
        {
            _fixed1 = (Fixed64)3.14159;
            _fixed2 = (Fixed64)2.71828;
            _double1 = 3.14159;
            _double2 = 2.71828;
        }

        [Benchmark]
        public Fixed64 Fixed64_Multiply() => _fixed1 * _fixed2;

        [Benchmark]
        public double Double_Multiply() => _double1 * _double2;

        // [Benchmark]
        // public Fixed64 Fixed64_Divide() => _fixed1 / _fixed2;
        //
        // [Benchmark]
        // public double Double_Divide() => _double1 / _double2;
        //
        // [Benchmark]
        // public Fixed64 Fixed64_Sqrt() => Fixed64.Sqrt(_fixed1);
        //
        // [Benchmark]
        // public double Double_Sqrt() => Math.Sqrt(_double1);
        //
        // [Benchmark]
        // public Fixed64 Fixed64_Sin() => Fixed64.Sin(_fixed1);
        //
        // [Benchmark]
        // public double Double_Sin() => Math.Sin(_double1);
        //
        // [Benchmark]
        // public Fixed64 Fixed64_Exp() => Fixed64.Exp(_fixed1);
        //
        // [Benchmark]
        // public double Double_Exp() => Math.Exp(_double1);
    }
}
