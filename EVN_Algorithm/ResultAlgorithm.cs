using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVN
{
    public class ResultAlgorithm
    {
        public double total_time { get; set; }
        public int param_nrMayCat { get; set; }
        public int param_nrDaoTuDong { get; set; }
        public int param_nrDenBao { get; set; }
        public List<string> may_cat { get; set; }
        public List<string> dao_tu_dong { get; set; }
        public List<string> den_bao { get; set; }
        public DateTime run_at { get; set; }
    }
}
