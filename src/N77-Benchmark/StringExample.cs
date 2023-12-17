using System.Text;
using BenchmarkDotNet.Attributes;

namespace N77_Benchmark;

public class StringExample
{
    [Params("Hello", "#$%^&*()")] public string ExampleText { get; set; }

    [Benchmark]
    public void ConcatenationWithString()
    {
        var result = string.Empty;
        foreach (var i in Enumerable.Range(0, 1000)) result += ExampleText;
    }

    [Benchmark]
    public void ConcatenationWithStringBuilder()
    {
        var result = new StringBuilder();
        foreach (var i in Enumerable.Range(0, 1000)) result.Append(ExampleText);
    }
}