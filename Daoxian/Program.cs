using System;
using MathNet;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Data.Text;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using Levellib;


namespace LevelApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string s1 = @"C:\Users\TT\Desktop\1.in2";
            string s2 = @"C:\Users\TT\Desktop\平面控制网.in2";
            Data data = new Data(s1);
            var ll = data.Jioadu();
            var ll1 =  data.Zuobiao(ll);
            LinkedListNode<Station> current = new LinkedListNode<Station>(null);
            current = ll1.First;

            while (current != null)
            {
                Console.WriteLine(current.Value.Name+"："+ current.Value.AjdustX+"," + current.Value.AjdustY);
                current = current.Next;
            }


        }
    }
    
}
