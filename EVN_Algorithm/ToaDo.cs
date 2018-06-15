using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVN
{
    public class ToaDo
    {
        private double xDaiDien;
        private double yDaiDien;
        public ToaDo(double x, double y)
        {
            this.xDaiDien = x;
            this.yDaiDien = y;
        }
        public double getX()
        {
            return xDaiDien;
        }
        public double getY()
        {
            return yDaiDien;
        }
    }
}
