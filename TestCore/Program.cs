﻿using PyLibSharp.Requests;
using System;

namespace TestCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = Requests.Get("https://124.156.146.248/setu/",new ReqParams()
            {
                Timeout = 3000
            });

            Console.WriteLine("结束");
            Console.WriteLine(str.Json());

            foreach (char c in str)
            {
                Console.Write(c);
            }

            Console.ReadKey();
        }
        private static void Requests_ReqExceptionHandler(object sender, Requests.AggregateExceptionArgs e)
        {
            
            Console.WriteLine(e.AggregateException);
        }
    }
}
