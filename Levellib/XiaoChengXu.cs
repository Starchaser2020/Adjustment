using System;
using System.Collections.Generic;
using System.Text;

namespace Levellib
{
    public class XiaoChengXu
    {
        //方位角
        public static double Fangweijiao(double x1, double y1, double x2, double y2)
        {
            double Fangweijiao;
            Fangweijiao = Math.Atan2(y2 - y1, x2 - x1);
            if (Fangweijiao < 0) { Fangweijiao += Math.PI * 2; }
            return Fangweijiao;

        }
        //度数转换为弧度
        public static double DegreeToRadian(double degree)
        {

            double d, m, s;
            d = Math.Floor(degree);
            m = Math.Floor((degree - d) * 100) / 60;
            s = (((degree - d) * 100 - Math.Floor((degree - d) * 100)) * 100) / 3600;
            return (d+m+s)*Math.PI/180;
        }

        //弧度转换为度数
        public static string RadianToDegree(double radian1)
        {
            double radian = Math.Abs(radian1);
            double dgree, d, m, s;
            dgree = radian * (180 / (Math.PI));//转为度数
            d = Math.Floor(dgree);
            m = Math.Floor((dgree - d) * 60);
            s = Math.Round((((dgree - d) * 60 - m) * 60), 1);
            if (radian1 >=0)
            {
                return d + "°" + m + "′" + s + "″";

            }
            else
            {
                return "-" + d + "°" + m + "′" + s + "″";

            }

        }
    }
}
