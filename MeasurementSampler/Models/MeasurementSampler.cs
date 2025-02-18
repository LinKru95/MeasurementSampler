using MeasurementSamplerApp.Enums;

namespace MeasurementSamplerApp.Models
{
    public class MeasurementSampler
    {
        public Dictionary<MeasurementType, List<Measurement>> Sample(DateTime startOfSampling, List<Measurement> unsampledMeasurements)
        {
            Dictionary<MeasurementType, List<Measurement>> result = new Dictionary<MeasurementType, List<Measurement>>();
            var groupedByType = unsampledMeasurements.GroupBy(m => m.Type);

            foreach (var group in groupedByType)
            {
                List<Measurement> measurements = group.OrderBy(m => m.MeasurementTime).ToList();
                List<Measurement> sampled = new List<Measurement>();
                DateTime intervalStart = startOfSampling;
                DateTime intervalEnd = intervalStart.AddMinutes(5);

                while (measurements.Count > 0)
                {
                    List<Measurement> measurementsInInterval = measurements
                        .Where(m => m.MeasurementTime > intervalStart && m.MeasurementTime <= intervalEnd)
                        .ToList();

                    if (measurementsInInterval.Any())
                    {
                        Measurement lastMeasurement = measurementsInInterval.Last();
                        sampled.Add(new Measurement(intervalEnd, lastMeasurement.Type, lastMeasurement.MeasurementValue));
                        measurements.RemoveAll(m => measurementsInInterval.Contains(m));
                    }

                    intervalStart = intervalEnd;
                    intervalEnd = intervalStart.AddMinutes(5);
                }

                result[group.Key] = sampled;
            }

            return result;
        }
    }
}
