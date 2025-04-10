using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class RadialBarChartDto
    {
        public decimal TotalCount { get; set; }
        public decimal CountCurrentMonth { get; set; }
        public bool HasRatioIncreased { get; set; }
        public int[] Series { get; set; }
    }

    public class LineChartVM
    {
        public List<ChartData> Series { get; set; }
        public string[] Categories { get; set; }
    }
    public class ChartData
    {
        public string Name { get; set; }
        public int[] Data { get; set; }
    }
}
