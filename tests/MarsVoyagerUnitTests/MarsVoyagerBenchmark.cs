using BenchmarkDotNet.Attributes;
using MarsVoyager.Inmutable;
using MarsVoyagerUnitTests;
using Shouldly;

using InmutableCardinalPoint = MarsVoyager.Inmutable.CardinalPoint;
using MutableCardinalPoint = MarsVoyager.Mutable.CardinalPoint;
using RecordCardinalPoint = MarsVoyager.MutableRecord.CardinalPoint;

using OriginalRover = MarsVoyager.Original.Rover;
using InmutableRover = MarsVoyager.Inmutable.Rover;
using MutableRover = MarsVoyager.Mutable.Rover;
using RecordRover = MarsVoyager.MutableRecord.Rover;

namespace MarsVoyagerUnitTests;

public class MarsVoyagerBenchmark
{
    [Benchmark]
    [ArgumentsSource(typeof(RoverMovesData), nameof(RoverMovesData.BenchmarkData))]
    public void OriginalRoverReceiveCommad(int x, int y, char pointingAt,
        string commandSequence, int resultantX, int resultantY, char pointingAtResultant)
    {
        OriginalRover sut = new(x, y, InmutableCardinalPoint.From(pointingAt).ToString());

        sut.Receive(commandSequence);
    }

    [Benchmark]
    [ArgumentsSource(typeof(RoverMovesData), nameof(RoverMovesData.BenchmarkData))]
    public void ImprovedInmutableRoverReceiveCommad(int x, int y, char pointingAt,
        string commandSequence, int resultantX, int resultantY, char pointingAtResultant)
    {
        InmutableRover sut = new(x, y, InmutableCardinalPoint.From(pointingAt));

        sut.Receive(commandSequence);
    }

    [Benchmark]
    [ArgumentsSource(typeof(RoverMovesData), nameof(RoverMovesData.BenchmarkData))]
    public void ImprovedMutableRoverReceiveCommad(int x, int y, char pointingAt,
       string commandSequence, int resultantX, int resultantY, char pointingAtResultant)
    {
        MutableRover sut = new(x, y, MutableCardinalPoint.From(pointingAt));

        sut.Receive(commandSequence);
    }

    [Benchmark]
    [ArgumentsSource(typeof(RoverMovesData), nameof(RoverMovesData.BenchmarkData))]
    public void ImprovedRecordRoverReceiveCommad(int x, int y, char pointingAt,
       string commandSequence, int resultantX, int resultantY, char pointingAtResultant)
    {
        RecordRover sut = new(x, y, RecordCardinalPoint.From(pointingAt));

        sut.Receive(commandSequence);
    }
}
