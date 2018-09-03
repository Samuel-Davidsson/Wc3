using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using SamuelWC3MatchData.Models;

namespace SamuelWC3MatchData.Controllers
{
    public class DataAccessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllData()
        {
            var playerName = "Fatso";
            var region = "Northrend";
            var pageNo = 1;
            var gameDetailsList = new List<GameDetail>();

            var url = "http://classic.battle.net/war3/ladder/w3xp-player-logged-games.aspx?Gateway=" + region + "&PlayerName=" + playerName + "&SortField=Game_Date&SortDir=Asc&PageNo=" + pageNo;

            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            if (doc == null)
            {
                return NotFound("Could not get data from the page.");
            }

            var maxPageString = doc.DocumentNode.SelectNodes("//td[@class='rankingFiller']//a").Last().InnerText.Trim(' ');
            var maxPageNumber = Convert.ToInt32(maxPageString);

            for (int p = 1; p <= maxPageNumber; p++)
            {
                url = "http://classic.battle.net/war3/ladder/w3xp-player-logged-games.aspx?Gateway=" + region + "&PlayerName=" + playerName + "&SortField=Game_Date&SortDir=Asc&PageNo=" + p;

                web = new HtmlWeb();
                doc = await web.LoadFromWebAsync(url);
                if (doc == null)
                {
                    return NotFound("Could not get data from the page.");
                }

                var htmlRankingRankingRowDataAll = doc.DocumentNode.SelectNodes("//tr[@class='rankingRow']//td");
                if (htmlRankingRankingRowDataAll == null)
                {
                    return NotFound("Could not get data from the page.");
                }

                var rankingRowList = new List<string>();
                foreach (var item in htmlRankingRankingRowDataAll)
                {
                    rankingRowList.Add(item.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                }

                for (int i = 0; i < rankingRowList.Count; i += 11)
                {
                    var gameDetail = new GameDetail()
                    {
                        Id = p * 100 + (i / 11),
                        Date = rankingRowList[i + 1],
                        GameType = rankingRowList[i + 2],
                        Map = rankingRowList[i + 3],
                        Allies = rankingRowList[i + 6],
                        Opponents = rankingRowList[i + 8],
                        GameLength = rankingRowList[i + 9],
                        Result = rankingRowList[i + 10],
                    };
                    gameDetailsList.Add(gameDetail);
                }
            }
            
            // Getting all games that I want here with the info I want now I need to store it in DB
            // Maybe show it in a model of some kind at the page instead not sure.
            return Json(gameDetailsList);
        }

    }
}