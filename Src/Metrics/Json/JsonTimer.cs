﻿
using System.Collections.Generic;
using Metrics.MetricData;
using Metrics.Utils;

namespace Metrics.Json
{
    public class JsonTimer
    {
        public class RateData
        {
            public double MeanRate { get; set; }
            public double OneMinuteRate { get; set; }
            public double FiveMinuteRate { get; set; }
            public double FifteenMinuteRate { get; set; }
        }

        public class HistogramData
        {
            public double LastValue { get; set; }
            public string LastUserValue { get; set; }

            public double Max { get; set; }
            public string MaxUserValue { get; set; }

            public double Mean { get; set; }

            public double Min { get; set; }
            public string MinUserValue { get; set; }

            public double StdDev { get; set; }

            public double Median { get; set; }
            public double Percentile75 { get; set; }
            public double Percentile95 { get; set; }
            public double Percentile98 { get; set; }
            public double Percentile99 { get; set; }
            public double Percentile999 { get; set; }

            public int SampleSize { get; set; }
        }

        public string Name { get; set; }
        public long Count { get; set; }
        public RateData Rate { get; set; }
        public HistogramData Histogram { get; set; }
        public string Unit { get; set; }
        public string RateUnit { get; set; }
        public string DurationUnit { get; set; }

        public static JsonTimer FromTimer(TimerValueSource timer)
        {
            return new JsonTimer
            {
                Name = timer.Name,
                Count = timer.Value.Rate.Count,
                Rate = ToRate(timer.Value.Rate.Scale(timer.RateUnit)),
                Histogram = ToHistogram(timer.Value.Histogram.Scale(timer.DurationUnit)),
                Unit = timer.Unit.Name,
                RateUnit = timer.RateUnit.Unit(),
                DurationUnit = timer.DurationUnit.Unit()
            };
        }

        private static HistogramData ToHistogram(HistogramValue histogram)
        {
            return new HistogramData
            {
                LastValue = histogram.LastValue,
                LastUserValue = histogram.LastUserValue,

                Max = histogram.Max,
                MaxUserValue = histogram.MaxUserValue,

                Mean = histogram.Mean,

                Min = histogram.Min,
                MinUserValue = histogram.MinUserValue,

                StdDev = histogram.StdDev,

                Median = histogram.Median,
                Percentile75 = histogram.Percentile75,
                Percentile95 = histogram.Percentile95,
                Percentile98 = histogram.Percentile98,
                Percentile99 = histogram.Percentile99,
                Percentile999 = histogram.Percentile999,

                SampleSize = histogram.SampleSize,
            };
        }

        private static RateData ToRate(MeterValue rate)
        {
            return new RateData
            {
                MeanRate = rate.MeanRate,
                OneMinuteRate = rate.OneMinuteRate,
                FiveMinuteRate = rate.FiveMinuteRate,
                FifteenMinuteRate = rate.FifteenMinuteRate
            };
        }

        public JsonObject ToJsonTimer()
        {
            return new JsonObject(ToJsonProperties());
        }

        public IEnumerable<JsonProperty> ToJsonProperties()
        {
            yield return new JsonProperty("Name", this.Name);
            yield return new JsonProperty("Count", this.Count);

            yield return new JsonProperty("Rate", ToJsonProperties(this.Rate));
            yield return new JsonProperty("Histogram", ToJsonProperties(this.Histogram));

            yield return new JsonProperty("Unit", this.Unit);
            yield return new JsonProperty("RateUnit", this.RateUnit);
            yield return new JsonProperty("DurationUnit", this.DurationUnit);
        }

        private static IEnumerable<JsonProperty> ToJsonProperties(RateData rate)
        {
            yield return new JsonProperty("MeanRate", rate.MeanRate);
            yield return new JsonProperty("OneMinuteRate", rate.OneMinuteRate);
            yield return new JsonProperty("FiveMinuteRate", rate.FiveMinuteRate);
            yield return new JsonProperty("FifteenMinuteRate", rate.FifteenMinuteRate);
        }

        private static IEnumerable<JsonProperty> ToJsonProperties(HistogramData histogram)
        {
            bool hasUserValues = histogram.LastUserValue != null || histogram.MinUserValue != null || histogram.MaxUserValue != null;

            yield return new JsonProperty("LastValue", histogram.LastValue);

            if (hasUserValues)
            {
                yield return new JsonProperty("LastUserValue", histogram.LastUserValue);
            }
            yield return new JsonProperty("Min", histogram.Min);
            if (hasUserValues)
            {
                yield return new JsonProperty("MinUserValue", histogram.MinUserValue);
            }
            yield return new JsonProperty("Mean", histogram.Mean);
            if (hasUserValues)
            {
                yield return new JsonProperty("MaxUserValue", histogram.MaxUserValue);
            }

            yield return new JsonProperty("StdDev", histogram.StdDev);
            yield return new JsonProperty("Median", histogram.Median);
            yield return new JsonProperty("Percentile75", histogram.Percentile75);
            yield return new JsonProperty("Percentile95", histogram.Percentile95);
            yield return new JsonProperty("Percentile98", histogram.Percentile98);
            yield return new JsonProperty("Percentile99", histogram.Percentile99);
            yield return new JsonProperty("Percentile999", histogram.Percentile999);
            yield return new JsonProperty("SampleSize", histogram.SampleSize);
        }


    }
}
