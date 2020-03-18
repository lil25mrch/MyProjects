using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace NetConsoleApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            WebClient client = new WebClient();

            //var domain = "http://www.ellegirl.ru";
            var domain = "https://salon-moon.ru/catalog/svadebnye-platya/pyshnye/cveta-champan/";
            Uri uri = new Uri(domain);

            //Документ сайта, который мы проверяем
            string content = client.DownloadString(uri); //все содержимое сайта
            File.WriteAllText(uri.Host + ".txt", content);
            HtmlParser xDoc = new HtmlParser();
            AngleSharp.Html.Dom.IHtmlDocument htmlDocument = xDoc.ParseDocument(content);



            ///////////////////////////////////////////////////////////////////////////////////////////////

            //число всех ссылок на странице
            int AllaHref = 0;
            foreach (IElement elementAllaHref in htmlDocument.QuerySelectorAll("a"))
            {
                string allaHref = elementAllaHref.GetAttribute("href");
                //Console.WriteLine(elementAllaHref.GetAttribute("href"));
                AllaHref++;
            }

            //число тегов <img src> 
            int src = 0;
            foreach (IElement elementSrc in htmlDocument.QuerySelectorAll("img"))
            {
                string countSrc = elementSrc.GetAttribute("src");
                src++;
            }

            //число стилей "background-image: url" 
            int bgurl = 0;
            string bgurlReg = content;
            Regex regex = new Regex(@"background-image: url");
            MatchCollection matches = regex.Matches(bgurlReg);
            foreach (Match match in matches)
            {
                bgurl++;
            }

            //число внутренних ссылок <a href> у которых вместо анкора <img>
            int aHrefImg = 0;
            foreach (IElement elementaHrefImg in htmlDocument.QuerySelectorAll("a > img"))
            {
                string countaHrefImg = elementaHrefImg.GetAttribute("img");
                aHrefImg++;
            }

            //Общее число внутренних ссылок на странице
            //Общее число уникальны внутренних ссылок на странице (по URL)
            int aInSite = 0;
            HashSet<string> listAInSyte = new HashSet<string>();
            foreach (IElement elementaInSite in htmlDocument.QuerySelectorAll("a"))
            {
                string countaHrefImg = elementaInSite.GetAttribute("href");
                if (countaHrefImg.StartsWith("/") || countaHrefImg.Contains(uri.Host))
                {
                    aInSite++;
                    if (listAInSyte.Contains(countaHrefImg) != true)
                    {
                        listAInSyte.Add(countaHrefImg);
                    }
                }
            }
        

            //Значение параметра <html lang="">, или пометка об его отсутствии
            foreach (IElement elementHtmlLang in htmlDocument.QuerySelectorAll("html"))
            {
                string countHtmlLang = elementHtmlLang.GetAttribute("xml:lang");
                if (countHtmlLang == null)
                {
                    Console.WriteLine("Нет присвоенного значения < html lang >");
                }
                else
                {
                    Console.WriteLine("Значение параметра < html lang > = {0}", countHtmlLang);
                }
            }

            //общее количество атрибутов title на странице
            int title = 0;
            foreach (IElement elementTitle in htmlDocument.QuerySelectorAll("[title]"))
            {
                string countTitle = elementTitle.GetAttribute("title");
                title++;
            }

            //наличие ссылки на каждую из указанных соцсетей
            aforMessag("instagram", htmlDocument);
            aforMessag("twitter", htmlDocument);
            aforMessag("facebook", htmlDocument);
            aforMessag("youtube", htmlDocument);
            aforMessag("vk", htmlDocument);
            aforMessag("google", htmlDocument);

            //число заголовков h2-h6 (отдельно)
            countHNumforMessage("h2", htmlDocument);
            countHNumforMessage("h3", htmlDocument);
            countHNumforMessage("h4", htmlDocument);
            countHNumforMessage("h5", htmlDocument);
            countHNumforMessage("h6", htmlDocument);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////// START

            /*if (domain != uri.Host)
            {
                //число всех ссылок на странице, исключая те, которые есть на главной странице
                int AllaHrefSTARTMinus = 0;
                int AllaHrefSTART = 0;
                foreach (IElement elementAllaHrefstart in htmlDocument.QuerySelectorAll("a"))
                {
                    string allaHref = elementAllaHref.GetAttribute("href");
                    //Console.WriteLine(elementAllaHref.GetAttribute("href"));
                    AllaHref++;
                }
            }
            */





            Console.WriteLine("Count all a href = {0}", AllaHref);
            Console.WriteLine("Count img src = {0}", src);
            Console.WriteLine("Count background-image: url = {0}", bgurl);
            Console.WriteLine("Count a href у которых вместо анкора img = {0}", aHrefImg);
            Console.WriteLine("Count a href у которых только внутренние ссылки = {0}", aInSite);
            Console.WriteLine("Количество уникальных внутренних ссылок на странице: {0}", listAInSyte.Count);
            Console.WriteLine("Count title = {0}", title);

            Console.Read();
        }

        private static void aforMessag(string name, AngleSharp.Html.Dom.IHtmlDocument htmlDocument)
        {
            IHtmlCollection<IElement> list = htmlDocument.QuerySelectorAll("a");

            var links = new HashSet<string>();

            foreach (IElement elementNameSS in list)
            {
                string countNameSS = elementNameSS.GetAttribute("href");
                if (countNameSS.Contains(name))
                {
                    links.Add(countNameSS);
                }
            }

            if (links.Count > 0)
            {
                Console.WriteLine("this tag is here: {0}", name);
            }
            else
            {
                Console.WriteLine("This tag is not here: {0}", name);
            }

        }

        private static void countHNumforMessage(string tagname, AngleSharp.Html.Dom.IHtmlDocument htmlDocument)
        {
            int countHNum = 0;
            foreach (IElement elementcountHNum in htmlDocument.QuerySelectorAll(tagname))
            {
                countHNum++;
            }

            Console.WriteLine("Количество заголовков " + tagname + " равно {0}", countHNum);
        }









    }
    
}