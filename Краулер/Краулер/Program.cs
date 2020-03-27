using System;
using Краулер.Helpers;

namespace Crawler {
    internal class Program {
        private static readonly string domain = new string("http://www.ellegirl.ru/articles/smi-raskryli-nazvanie-tretei-chasti-cheloveka-pauka-s-tomom-khollandom/");

        //string domain = "https://www.ozon.ru/brand/acuvue-148736994/";
        //string domain = "https://www.sports.ru";
        static readonly ReportCreater reportCreater = new ReportCreater();

        static readonly string _parsingPage = reportCreater.PageParse(domain);

        public static void Main(string[] args) {
            Console.WriteLine(_parsingPage);
            Console.Read();
        }
    }
}