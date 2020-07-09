using System;
using System.Collections.Generic;
using System.Text;

namespace Levellib
{
    public class XianYanData//第一行数据
    {
        public double m { get; set; }//测角中误差
        public double a { get; set; }//测距固定误差
        public double b { get; set; }//测距比例误差（PPM）
    }
    public class Station
    {
        public Station()
        {

        }
        public Station(string name)
        {
            this.Name = name;
        }
        public string BackPoint { get; set; }
        public double BackPointAngle { get; set; }
        public double BackPointDistance { get; set; }
        public string NextPoint { get; set; }
        public double NextPointAngle { get; set; }
        public double NextPointDistance { get; set; }
        public bool IsControlPoint { get; set; }
        public string Name { get; set; }
        public double DY { get; set; }
        public double DX { get; set; }//近似坐标差
        public double X { get; set; }
        public double Y { get; set; }//近似坐标；
        public double AjdustDY { get; set; }
        public double AjdustDX { get; set; }
        public double AjdustY { get; set; }
        public double AjdustX { get; set; }
        public double Fangweijiao { get; set; }//近似方位角
        public double AFangweijiao { get; set; }//调整后方位角
    }

}
