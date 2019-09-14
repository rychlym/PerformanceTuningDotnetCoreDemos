﻿using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haken.PerformanceTuningDotnetCoreDemos.CodingAndAlgorithms
{
	[SimpleJob(warmupCount: 1, launchCount: 1, targetCount: 200, invocationCount: 1000000)]
	[MemoryDiagnoser]
	public class StringConcatConditionConsolidationBenchmarks
	{
		private string s1 = "Blah";
		private string s2 = "Dooh";
		private string s3 = "Hey!!!!!!!";
		private bool condition = (DateTime.Now.Hour >= 0);

		[Benchmark]
		public string MultipleConcats()
		{
			var s = s1 + s2;
			if (condition)
			{
				s = s + s3;
			}
			return s;
		}

		[Benchmark]
		public string ConsolidatedConcats()
		{
			return condition ? s1 + s2 + s3 : s1 + s2;
		}
	}
}

//|              Method |     Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
//|-------------------- |---------:|----------:|----------:|-------:|------:|------:|----------:|
//|     MultipleConcats | 31.80 ns | 0.1856 ns | 0.7589 ns | 0.0260 |     - |     - |     112 B |
//| ConsolidatedConcats | 21.11 ns | 0.2352 ns | 0.9619 ns | 0.0150 |     - |     - |      64 B |

// https://github.com/dotnet/corefx/issues/40739