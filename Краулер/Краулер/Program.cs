using System;
using Краулер.Helpers;

namespace Crawler {
    internal class Program {
        private static readonly string domain = new string("http://www.ellegirl.ru/articles/smi-raskryli-nazvanie-tretei-chasti-cheloveka-pauka-s-tomom-khollandom/");

        //string domain = "https://www.ozon.ru/brand/acuvue-148736994/";
        //private static readonly string domain = "https://www.sports.ru";
        static readonly ReportCreater _reportCreater = new ReportCreater();

        static readonly string _parsingPage = _reportCreater.PageParse(domain);

        public static void Main(string[] args) {
            Console.WriteLine(_parsingPage);
            Console.Read();
        }
    }
}