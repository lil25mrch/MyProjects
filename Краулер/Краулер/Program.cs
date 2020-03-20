using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Crawler
{
    class Program
    {
        public static void Main(string[] args)
        {
            WebClient client = new WebClient();
            
            var domain = "http://www.ellegirl.ru/articles/smi-raskryli-nazvanie-tretei-chasti-cheloveka-pauka-s-tomom-khollandom/";
            //var domain = "https://www.ozon.ru/brand/acuvue-148736994/";
            Uri uri = new Uri(domain);
            string uriHost = uri.Host.ToString();
            
            //Документ сайта, который мы проверяем
            string content = client.DownloadString(uri); 
            File.WriteAllText(uri.Host + ".txt", content);
            HtmlParser xDoc = new HtmlParser();
            AngleSharp.Html.Dom.IHtmlDocument htmlDocument = xDoc.ParseDocument(content);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            //Документ главной страницы сайта, который мы проверяем
            var uriStart = uri.Scheme.ToString() + "://" + uri.Host.ToString();
            
            Uri uriStartPage = new Uri(uriStart);
            string uriStartPageHost = uri.Host.ToString();
            
            string contentStart = client.DownloadString(uriStartPage); 
            File.WriteAllText(uriStartPage.Host + "start.txt", contentStart);
            HtmlParser xDocStart = new HtmlParser();
            AngleSharp.Html.Dom.IHtmlDocument htmlDocumentSP = xDocStart.ParseDocument(contentStart);

            ///////////////////////////////////////////////////////////////////////////////////////////////
            //число всех ссылок на странице
            int AllaHref = 0;
            foreach (IElement elementAllaHref in htmlDocument.QuerySelectorAll("a"))
            {
                string allaHref = elementAllaHref.GetAttribute("href");
                AllaHref++;
            }
            /////////////////////////////////////////////////////////////////////////////////////////////
           
            
            
            //////////////////////////////////////////////////////////////////////////////////////
            //число тегов <img src> 
            int src = 0;
            HashSet<string> srcList = new HashSet<string>();
            foreach (IElement elementSrc in htmlDocument.QuerySelectorAll("img"))
            {
                string countSrc = elementSrc.GetAttribute("src");
                srcList.Add(countSrc);
                src++;
            }
            //число тегов <img src>, исключая теги, которые есть на главной странице
            int srcSP = 0;
            foreach (IElement elementSrcSP in htmlDocumentSP.QuerySelectorAll("img"))
            {
                string countSrc = elementSrcSP.GetAttribute("src");
                if (srcList.Contains(countSrc))
                {
                    srcSP++;
                }
            }
            
            int diffSrc = src - srcSP;
            
            
            //число стилей "background-image: url" 
            
            /*int bgurl = 0;
            string bgurlReg = contentStart;
            Regex regex = new Regex(@"background-image: url");
            MatchCollection matches = regex.Matches(bgurlReg);
            foreach (Match match in matches)
            {
                bgurl++;
            }*/
            /*
            int bgurl = 0;
            foreach (IElement elementBgImgSP in htmlDocument.QuerySelectorAll("img"))
            {
                string countSrc = elementBgImgSP.GetAttribute("src");
                if (srcList.Contains(countSrc))
                {
                    srcSP++;
                }
            }
           
            string bgurlReg = contentStart;
            Regex regex = new Regex(@"background-image: url");
            MatchCollection matches = regex.Matches(bgurlReg);
            foreach (Match match in matches)
            {
                bgurl++;
            }
            */
        
            //число внутренних ссылок <a href> у которых вместо анкора <img>
            int aHrefImg = 0;
            HashSet<string> aHrefImgList = new HashSet<string>();
            foreach (IElement elementaHrefImg in htmlDocument.QuerySelectorAll("a"))
            {
                string countaHrefImg = elementaHrefImg.GetAttribute("href");
                if ((countaHrefImg.StartsWith("/") || countaHrefImg.StartsWith(uriHost)) && countaHrefImg.Contains("img"))
                {
                    aHrefImg++;
                    aHrefImgList.Add(countaHrefImg);
                }
            }
            
            int aHrefImgSP = 0;
            foreach (IElement elementaHrefSP in htmlDocumentSP.QuerySelectorAll("a"))
            {
                string aHrefSP = elementaHrefSP.GetAttribute("href");
                if (aHrefImgList.Contains(aHrefSP))
                {
                    aHrefImgSP++;
                }
            }
            
            int DiffaHrefImg = aHrefImg - aHrefImgSP;
  
            
           
            
            
            
           
            //Общее число внутренних ссылок на странице 
            //Общее число уникальны внутренних ссылок на странице (по URL)
            int aInSite = 0;
            int aMoreOne = 0;
            HashSet<string> UniclistAInSyte = new HashSet<string>();
            HashSet<string> listAInSyte = new HashSet<string>();
            
            foreach (IElement elementaInSite in htmlDocument.QuerySelectorAll("a"))
            {
                string countaHrefImg = elementaInSite.GetAttribute("href");
                if (countaHrefImg.StartsWith("/") || countaHrefImg.Contains(uriHost))
                {
                    aInSite++;
                    if (UniclistAInSyte.Contains(countaHrefImg) != true)
                    {
                        UniclistAInSyte.Add(countaHrefImg);
                    }
                    else 
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
            int TitleLength = 0;
            foreach (IElement elementTitle in htmlDocument.QuerySelectorAll("[title]"))
            {
                string countTitle = elementTitle.GetAttribute("title");
                TitleLength += countTitle.Length;
                title++;
            }
            
            //средняя длина значения атрибутов title на странице
            decimal avlTitle = TitleLength / title;
            Console.WriteLine(Math.Ceiling(avlTitle));

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

          





            Console.WriteLine("Count all a href = {0}", AllaHref);
            Console.WriteLine("Count img src = {0}", src);
            //Console.WriteLine("Count background-image: url = {0}", bgurl);
            Console.WriteLine("Count a href у которых вместо анкора img = {0}", aHrefImg);
            Console.WriteLine("Count a href у которых только внутренние ссылки = {0}", aInSite);
            Console.WriteLine("Количество уникальных внутренних ссылок на странице: {0}", UniclistAInSyte.Count);
            Console.WriteLine("Количество внутренних ссылок, встречающихся более 1 раза на странице: {0}", listAInSyte.Count);
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