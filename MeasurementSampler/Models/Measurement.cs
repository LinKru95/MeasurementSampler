using MeasurementSamplerApp.Enums;

namespace MeasurementSamplerApp.Models
{
    public class Measurement
    {
        public DateTime MeasurementTime { get; set; }
        public double MeasurementValue { get; set; }
        public MeasurementType Type { get; set; }

        public Measurement(DateTime time, MeasurementType type, double value)
        {
            MeasurementTime = time;
            MeasurementValue = value;
            Type = type;
        }
    }
}