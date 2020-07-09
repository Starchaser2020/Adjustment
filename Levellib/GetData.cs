using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LinkList;
namespace Levellib

{
    public class Data
    {
        public XianYanData XianYanData { get; set; }
        public List<Station> controlList { get; set; }
        public string FilePath { get; set; }
        public Data(string s)
        {
            FilePath = s;
        }

        public string GetData()
        {
            string s;
            XianYanData xianYanData = new XianYanData();
            List<Station> controlList = new List<Station>();

            #region 读取先验信息
            StreamReader sr = new StreamReader(FilePath, Encoding.Default);
            s = sr.ReadLine();
            string[] xianyanxinxi = s.Split(',');
            xianYanData.m = double.Parse(xianyanxinxi[0]);
            xianYanData.a = double.Parse(xianyanxinxi[1]);
            xianYanData.b = double.Parse(xianyanxinxi[2]);
            this.XianYanData = xianYanData;
            #endregion

            #region 读取已知点信息
            for (int i = 0; i < 4; i++)
            {
                Station station = new Station();//不懂放在外面就不行；
                s = sr.ReadLine();
                station.Name = (s.Split(',', '\n'))[0];
                station.X = double.Parse((s.Split(','))[1]);
                station.Y = double.Parse((s.Split(','))[2]);
                controlList.Add(station);

            }
            this.controlList = controlList;
            #endregion

            return s = sr.ReadToEnd().Trim();
        }

        public LinkedList<Station> GetShujukai(string s)
        {
            string[] str;
            string[] str1;
            List<int> stationNumber = new List<int>();
            Station station = new Station();
            LinkedListNode<Station> linkedListNode = new LinkedListNode<Station>(null);
            LinkedList<Station> ll = new LinkedList<Station>();

            str = s.Split('\n');

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Split(',', '\n').Length == 1)
                {
                    stationNumber.Add(i);
                }
            }//获取每个数据块的开始行号；

            for (int i = 0; i < stationNumber.Count - 1; i++)
            {
                str1 = str[stationNumber[i]..stationNumber[i + 1]];
                station = GetStation(str1);
                linkedListNode = GetStationLinkNode(station);
                ll.AddLast(linkedListNode);
            }
            str1 = str[(str.Length - 5)..(str.Length)];
            station = GetStation(str1);
            linkedListNode = GetStationLinkNode(station);
            ll.AddLast(linkedListNode);


            return ll;
        }
        public Station GetStation(string[] str)
        {
            Station station = new Station
            {
                Name = str[0].Trim(),
                BackPoint = str[1].Split(',')[0],
                BackPointAngle = 0,
                BackPointDistance = double.Parse(str[3].Split(',')[2]),
                NextPoint = str[2].Split(',')[0],
                NextPointAngle = double.Parse(str[2].Split(',')[2]),
                NextPointDistance = double.Parse(str[4].Split(',')[2]),
            };
            return station;
        }
        public LinkedListNode<Station> GetStationLinkNode(Station station)
        {
            LinkedListNode<Station> linkedListNode = new LinkedListNode<Station>(station);
            return linkedListNode;
        }
        public LinkedList<Station> AddControlPoint(LinkedList<Station> ll, List<Station> controlList)
        {

            for (int j = 0; j < controlList.Count; j++)
            {
                if (ll.First.Value.BackPoint == controlList[j].Name)
                {
                    ll.AddFirst(GetStationLinkNode(controlList[j]));
                    ll.First.Value.NextPoint = ll.First.Next.Value.Name;
                    ll.First.Value.BackPoint = null;
                }

                else if (ll.Last.Value.NextPoint == controlList[j].Name)
                {
                    ll.AddLast(GetStationLinkNode(controlList[j]));
                    ll.Last.Value.NextPoint = null;
                    ll.Last.Value.BackPoint = ll.Last.Previous.Value.Name;
                }
            }
            for (int j = 0; j < controlList.Count; j++)
            {
                if (ll.First.Value.Name == controlList[j].Name)
                {
                    ll.First.Value.X = controlList[j].X;
                    ll.First.Value.Y = controlList[j].Y;
                    ll.First.Value.IsControlPoint = true;
                }
                else if (ll.First.Next.Value.Name == controlList[j].Name)
                {
                    ll.First.Next.Value.X = controlList[j].X;
                    ll.First.Next.Value.Y = controlList[j].Y;
                    ll.First.Next.Value.IsControlPoint = true;

                }
                else if (ll.Last.Value.Name == controlList[j].Name)
                {
                    ll.Last.Value.X = controlList[j].X;
                    ll.Last.Value.Y = controlList[j].Y;
                    ll.Last.Value.IsControlPoint = true;

                }
                else if (ll.Last.Previous.Value.Name == controlList[j].Name)
                {
                    ll.Last.Previous.Value.X = controlList[j].X;
                    ll.Last.Previous.Value.Y = controlList[j].Y;
                    ll.Last.Value.IsControlPoint = true;

                }

            }
            return ll;

        }
        public LinkedList<Station> DataAction()
        {
            LinkedList<Station> ll = this.GetShujukai(this.GetData());
            ll = this.AddControlPoint(ll, this.controlList);
            return ll;


        }
        public LinkedList<Station> Jioadu()
        {
            LinkedList<Station> ll = this.DataAction();
            LinkedListNode<Station> current;
            var StartFangweijiao = XiaoChengXu.Fangweijiao(ll.First.Next.Value.X, ll.First.Next.Value.Y,
                ll.First.Value.X, ll.First.Value.Y);
            var EndFangweijiao = XiaoChengXu.Fangweijiao(ll.Last.Previous.Value.X, ll.Last.Previous.Value.Y,
                ll.Last.Value.X, ll.Last.Value.Y);
            var n = XiaoChengXu.Fangweijiao(ll.First.Value.X, ll.First.Value.Y,
                ll.First.Next.Value.X, ll.First.Next.Value.Y);
            double practiceAngle = n;
            current = ll.First.Next;
            while (current.Next != null)
            {
                practiceAngle += XiaoChengXu.DegreeToRadian(current.Value.NextPointAngle) - Math.PI;
                if (practiceAngle>2*Math.PI)
                {
                    practiceAngle -= 2 * Math.PI;
                }
                else if(practiceAngle<0)
                {
                    practiceAngle += 2 * Math.PI;
                }
                current.Value.Fangweijiao = practiceAngle;
                current = current.Next;
                
            }

            double angleError = (practiceAngle - EndFangweijiao);

            double angleAjdustment = angleError / (ll.Count - 2);
            current = ll.First.Next;
            while (current.Next != ll.Last)
            {
                current.Value.AFangweijiao = current.Value.Fangweijiao - angleAjdustment;
                current = current.Next;
            }

            return ll;
        }

        public LinkedList<Station>  Zuobiao(LinkedList<Station> ll)
        {
            LinkedListNode<Station> current = ll.First.Next;
            /*            while (current.Next != ll.Last)
                        {
                            Console.WriteLine(current.Value.Name + ":         " + XiaoChengXu.RadianToDegree(current.Value.Fangweijiao));
                            current = current.Next;
                        }*/
/*            current = ll.First.Next;
            while (current != ll.Last.Previous.Previous)
            {
                
                current.Next.Value.X = current.Value.X+ current.Value.NextPointDistance * Math.Cos(current.Value.AFangweijiao);
                current.Next.Value.Y = current.Value.Y+current.Value.NextPointDistance * Math.Sin(current.Value.AFangweijiao);
                Console.WriteLine(current.Next.Value.Name+" :"+current.Next.Value.X);
                current = current.Next;

            }*/

            

            current = ll.First.Next;
            while (current != ll.Last.Previous)
            {
/*                Console.WriteLine(current.Value.Fangweijiao);
                Console.WriteLine(current.Value.AFangweijiao);*/
                current.Value.DX = (current.Value.NextPointDistance + current.Next.Value.BackPointDistance) / 2.0 * Math.Cos(current.Value.AFangweijiao);
                current.Value.DY = (current.Value.NextPointDistance + current.Next.Value.BackPointDistance) / 2.0 * Math.Sin(current.Value.AFangweijiao);
                current = current.Next;
            }

            current = ll.First.Next;
            while (current != ll.Last.Previous.Previous)
            {
                current.Next.Value.X = current.Value.X + current.Value.DX;
                current.Next.Value.Y = current.Value.Y + current.Value.DY;
                current = current.Next;
            }

            current = ll.First.Next;
            double SumOfDX = 0;
            double SumOfDY = 0;
            double EorroOfDX;
            double EorroOfDY;
            double changdu = 0;

            while (current !=ll.Last.Previous)
            {
                changdu += current.Value.NextPointDistance;
                SumOfDX += current.Value.DX;
                SumOfDY += current.Value.DY;
                current = current.Next;
            }

            EorroOfDX = SumOfDX - (ll.Last.Previous.Value.X - ll.First.Next.Value.X);
            EorroOfDY = SumOfDY - (ll.Last.Previous.Value.Y - ll.First.Next.Value.Y);

            current = ll.First.Next;
            while (current != ll.Last.Previous)
            {
                current.Value.AjdustDX = -current.Value.NextPointDistance / changdu * EorroOfDX;
                current.Value.AjdustDY = -current.Value.NextPointDistance / changdu * EorroOfDY;
                current = current.Next;
            }

            current = ll.First.Next.Next;
            while (current != ll.Last.Previous)
            {
                current.Value.AjdustX = current.Value.X+current.Previous.Value.AjdustDX;
                current.Value.AjdustY = current.Value.Y+current.Previous.Value.AjdustDY;
                current = current.Next;
            }
            
            return ll;

        }
    }
}




