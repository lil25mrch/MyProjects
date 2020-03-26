using Краулер.Helpers;

namespace Crawler {
    internal class Program {
        public static void Main(string[] args) {
            //var domain = "http://www.ellegirl.ru/articles/smi-raskryli-nazvanie-tretei-chasti-cheloveka-pauka-s-tomom-khollandom/";
            string domain = "https://www.ozon.ru/brand/acuvue-148736994/";
            //string domain = "https://www.sports.ru";
            ReportCreater reportCreater = new ReportCreater();
            reportCreater.PageParse(domain);
        }
    }
}