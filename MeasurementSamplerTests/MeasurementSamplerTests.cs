using MeasurementSamplerApp.Enums;
using MeasurementSamplerApp.Models;

public class MeasurementSamplerTests
{
    [Fact]
    public void Sample_EmptyInput_ReturnsEmptyDictionary()
    {
        // Arrange
        MeasurementSampler sampler = new MeasurementSampler();
        DateTime startOfSampling = new DateTime(2023, 1, 17, 0, 0, 0);
        List<Measurement> unsampledMeasurements = new List<Measurement>();

        // Act
        Dictionary<MeasurementType, List<Measurement>> result = sampler.Sample(startOfSampling, unsampledMeasurements);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Sample_SingleMeasurement_ReturnsSingleMeasurementInCorrectInterval()
    {
        // Arrange
        MeasurementSampler sampler = new MeasurementSampler();
        DateTime startOfSampling = new DateTime(2023, 1, 17, 0, 0, 0);
        DateTime measurementTime = new DateTime(2023, 1, 17, 0, 3, 0);
        List<Measurement> unsampledMeasurements = new List<Measurement>
        {
            new Measurement(measurementTime, MeasurementType.TEMP, 36.6)
        };

        // Act
        Dictionary<MeasurementType, List<Measurement>> result = sampler.Sample(startOfSampling, unsampledMeasurements);

        // Assert
        Assert.Single(result);
        Assert.Single(result[MeasurementType.TEMP]);
        Assert.Equal(new DateTime(2023, 1, 17, 0, 5, 0), result[MeasurementType.TEMP].First().MeasurementTime);
        Assert.Equal(36.6, result[MeasurementType.TEMP].First().MeasurementValue);
    }

    [Fact]
    public void Sample_MultipleMeasurementsInSameInterval_ReturnsLastMeasurement()
    {
        // Arrange
        MeasurementSampler sampler = new MeasurementSampler();
        DateTime startOfSampling = new DateTime(2023, 1, 17, 0, 0, 0);
        List<Measurement> unsampledMeasurements = new List<Measurement>
        {
            new Measurement(new DateTime(2023, 1, 17, 0, 1, 0), MeasurementType.TEMP, 36.6),
            new Measurement(new DateTime(2023, 1, 17, 0, 2, 0), MeasurementType.TEMP, 36.7),
            new Measurement(new DateTime(2023, 1, 17, 0, 3, 0), MeasurementType.TEMP, 36.8)
        };

        // Act
        Dictionary<MeasurementType, List<Measurement>> result = sampler.Sample(startOfSampling, unsampledMeasurements);

        // Assert
        Assert.Single(result);
        Assert.Single(result[MeasurementType.TEMP]);
        Assert.Equal(new DateTime(2023, 1, 17, 0, 5, 0), result[MeasurementType.TEMP].First().MeasurementTime);
        Assert.Equal(36.8, result[MeasurementType.TEMP].First().MeasurementValue);
    }

    [Fact]
    public void Sample_MeasurementsAcrossMultipleIntervals_ReturnsCorrectIntervals()
    {
        // Arrange
        MeasurementSampler sampler = new MeasurementSampler();
        DateTime startOfSampling = new DateTime(2023, 1, 17, 0, 0, 0);
        List<Measurement> unsampledMeasurements = new List<Measurement>
        {
            new Measurement(new DateTime(2023, 1, 17, 0, 1, 0), MeasurementType.TEMP, 36.6),
            new Measurement(new DateTime(2023, 1, 17, 0, 6, 0), MeasurementType.TEMP, 36.7),
            new Measurement(new DateTime(2023, 1, 17, 0, 11, 0), MeasurementType.TEMP, 36.8)
        };

        // Act
        Dictionary<MeasurementType, List<Measurement>> result = sampler.Sample(startOfSampling, unsampledMeasurements);

        // Assert
        Assert.Single(result);
        Assert.Equal(3, result[MeasurementType.TEMP].Count);
        Assert.Equal(new DateTime(2023, 1, 17, 0, 5, 0), result[MeasurementType.TEMP][0].MeasurementTime);
        Assert.Equal(new DateTime(2023, 1, 17, 0, 10, 0), result[MeasurementType.TEMP][1].MeasurementTime);
        Assert.Equal(new DateTime(2023, 1, 17, 0, 15, 0), result[MeasurementType.TEMP][2].MeasurementTime);
    }

    [Fact]
    public void Sample_MeasurementsOfDifferentTypes_AreHandledCorrectly()
    {
        // Arrange
        MeasurementSampler sampler = new MeasurementSampler();
        DateTime startOfSampling = new DateTime(2023, 10, 1, 0, 0, 0);
        List<Measurement> unsampledMeasurements = new List<Measurement>
        {
            new Measurement(new DateTime(2023, 10, 1, 0, 1, 0), MeasurementType.TEMP, 36.6),
            new Measurement(new DateTime(2023, 10, 1, 0, 2, 0), MeasurementType.SPO2, 98),
            new Measurement(new DateTime(2023, 10, 1, 0, 3, 0), MeasurementType.TEMP, 36.7),
            new Measurement(new DateTime(2023, 10, 1, 0, 4, 0), MeasurementType.SPO2, 97)
        };

        // Act
        Dictionary<MeasurementType, List<Measurement>> result = sampler.Sample(startOfSampling, unsampledMeasurements);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Single(result[MeasurementType.TEMP]);
        Assert.Single(result[MeasurementType.SPO2]);
        Assert.Equal(new DateTime(2023, 10, 1, 0, 5, 0), result[MeasurementType.TEMP].First().MeasurementTime);
        Assert.Equal(36.7, result[MeasurementType.TEMP].First().MeasurementValue);
        Assert.Equal(new DateTime(2023, 10, 1, 0, 5, 0), result[MeasurementType.SPO2].First().MeasurementTime);
        Assert.Equal(97, result[MeasurementType.SPO2].First().MeasurementValue);
    }
}