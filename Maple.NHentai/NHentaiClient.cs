using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using Maple.NHentai.Models;

namespace Maple.NHentai
{
    public class NHentaiClient
    {
        private const string NHentaiUrl = "https://nhentai.net";
        private readonly HttpClient _http;

        public NHentaiClient()
        {
            _http = new HttpClient();
        }

        public async Task<Doujin> GetDoujinById(int id)
        {
            return await FetchDoujin($"{NHentaiUrl}/g/{id}");
        }

        public async Task<Doujin> GetRandomDoujin()
        {
            return await FetchDoujin($"{NHentaiUrl}/random");
        }

        private async Task<Doujin> FetchDoujin(string url)
        {
            var res = await _http.GetAsync(url);
            res.EnsureSuccessStatusCode();

            var doujin = new Doujin
            {
                Url = url
            };

            var doc = await BrowsingContext.New(Configuration.Default)
                .OpenAsync(async (req) => req.Content(await res.Content.ReadAsStringAsync()));

            doujin.Id = int.Parse(doc.GetElementById("gallery_id").TextContent.Substring(1));

            doujin.CoverArtUrl = doc.GetElementsByTagName("img")
                .First(w => w.ClassName == "lazyload").GetAttribute("data-src");

            foreach (var titleElem in doc.GetElementsByClassName("title"))
            {
                switch (titleElem.LocalName)
                {
                    case "h1":
                        doujin.Title = titleElem.TextContent;
                        break;

                    case "h2":
                        doujin.NativeTitle = titleElem.TextContent;
                        break;
                }
            }

            foreach (var tag in doc.GetElementsByClassName("tag"))
            {
                switch (tag.GetAttribute("href").Substring(1).Split('/')[0])
                {
                    case "tag":
                        doujin.Tags
                            .Add(tag.Children
                                .First(e => e.ClassName == "name").Text());
                        break;

                    case "language":
                        var langText = doujin.Language = tag.Children
                            .First(e => e.ClassName == "name").Text();

                        if (langText.ToLower() != "translated")
                        {
                            doujin.Language = langText;
                        }

                        break;

                    case "character":
                        doujin.Characters
                            .Add(tag.Children
                                .First(e => e.ClassName == "name").Text());
                        break;

                    case "parody":
                        doujin.Parodies
                            .Add(tag.Children
                                .First(e => e.ClassName == "name").Text());
                        break;

                    case "search":
                        doujin.Pages = int.Parse(tag.Children.FirstOrDefault().Text());
                        break;
                }
            }

            var elemDate = doc.GetElementsByTagName("time")
                .First(e => e.ClassName == "nobold")
                .GetAttribute("datetime").Split('T');
            var date = elemDate[0].Split('-');
            var time = elemDate[1].Split('.')[0].Split(':');

            doujin.PublishDate = new DateTime(
                int.Parse(date[0]), // Year
                int.Parse(date[1]), // Month
                int.Parse(date[2]), // Day
                int.Parse(time[0]), // Hour
                int.Parse(time[1]), // Minute
                int.Parse(time[2]) // Second
            );

            return doujin;
        }
    }
}