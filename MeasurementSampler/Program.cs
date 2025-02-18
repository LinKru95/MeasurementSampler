using MeasurementSamplerApp.Enums;
using MeasurementSamplerApp.Models;

namespace MeasurementSamplerApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Measurement> measurements = new List<Measurement>
            {
                new Measurement(DateTime.Parse("2017-01-03T10:04:45"), MeasurementType.TEMP, 35.79),
                new Measurement(DateTime.Parse("2017-01-03T10:01:18"), MeasurementType.SPO2, 98.78),
                new Measurement(DateTime.Parse("2017-01-03T10:09:07"), MeasurementType.TEMP, 35.01),
                new Measurement(DateTime.Parse("2017-01-03T10:03:34"), MeasurementType.SPO2, 96.49),
                new Measurement(DateTime.Parse("2017-01-03T10:02:01"), MeasurementType.TEMP, 35.82),
                new Measurement(DateTime.Parse("2017-01-03T10:05:00"), MeasurementType.SPO2, 97.17),
                new Measurement(DateTime.Parse("2017-01-03T10:05:01"), MeasurementType.SPO2, 95.08)
            };
            MeasurementSampler sampler = new MeasurementSampler();
            var sampled = sampler.Sample(DateTime.Parse("2017-01-03T10:00:00"), measurements);

            foreach (var type in sampled.Keys)
            {
                Console.WriteLine($"{type}:");

                foreach (var m in sampled[type])
                {
                    Console.WriteLine($"{{ {m.MeasurementTime}, {m.Type}, {m.MeasurementValue} }}");
                }
            }
        }
    }
}
