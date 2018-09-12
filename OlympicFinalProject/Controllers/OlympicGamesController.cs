using OlympicFinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OlympicFinalProject.Controllers
{
    public class OlympicGamesController : Controller
    {
        OlympicGames3Entities olympicModel = new OlympicGames3Entities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CountryInput()
        {
            List<Country> countries = olympicModel.Countries.OrderBy(q => q.CountryName).ToList();
            List<MedalType> medalTypes = olympicModel.MedalTypes.OrderBy(q => q.MedalType1).ToList();

            ViewBag.countries = countries;
            ViewBag.medalTypes = medalTypes;
            return View();
        }
        public ActionResult ShowQuery()
        {
            var top50Medalists =
                            (from m in olympicModel.Medalists
                             join a in olympicModel.Athletes on m.AthleteID equals a.AthleteID
                             join c in olympicModel.Countries on a.CountryID equals c.CountryID
                             group m by new { m.AthleteID, a.AthleteName, c.CountryName } into g
                             orderby g.Count() descending
                             select new top50Model
                             {
                                 AthleteID = g.Key.AthleteID,
                                 AthleteName = g.Key.AthleteName,
                                 MedalCount = g.Count(),
                                 CountryName = g.Key.CountryName
                             }

                         ).Take(50);

            ViewBag.Data = top50Medalists;

            return View();
        }

        public ActionResult ShowEconomyImpact()
        {
            var grpdCntriesAndMedals =
                             (from m in olympicModel.Medalists
                              join a in olympicModel.Athletes on m.AthleteID equals a.AthleteID
                              join c in olympicModel.Countries on a.CountryID equals c.CountryID
                              group m by new { c.CountryID, c.CountryName, c.CountryGDP } into g
                              orderby g.Count() descending
                              select new countriesGrpdMedals
                              {
                                  CountryName = g.Key.CountryName,
                                  CountryId = g.Key.CountryID,
                                  MedalCount = g.Count(),
                                  GDP = (double)g.Key.CountryGDP
                              }
                          );

            int top20Percent = (int)(grpdCntriesAndMedals.Count() * 20 / 100);
            var top20PercentRichestCountries = (from c in grpdCntriesAndMedals
                                                orderby c.GDP descending
                                                select c).Take(top20Percent);
            var lastRow = top20PercentRichestCountries.Skip(top20PercentRichestCountries.Count() - 1).FirstOrDefault();
            double minHighGDP = lastRow.GDP;

            //find the last country from the 20 % list and use it's medal count as min medal count to qualify in the 20% list
            var top20PercentMedalistsCountries = (from c in grpdCntriesAndMedals
                                                  orderby c.MedalCount descending
                                                  select c).Take(top20Percent);
            lastRow = top20PercentMedalistsCountries.Skip(top20PercentMedalistsCountries.Count() - 1).FirstOrDefault();
            int minHighMedalCount = lastRow.MedalCount;

            //var medalAvg = (from country in grpdCntriesAndMedals
            //                select country.MedalCount).Average();

            //var gdpAvg = (from country in olympicModel.Countries
            //                select country.CountryGDP).Average();

            //ViewBag.MedalAverage = Math.Round(medalAvg);

            var lowGDPCountries =
               from country in grpdCntriesAndMedals
               where (country.MedalCount > minHighMedalCount) && (country.GDP < minHighGDP)
               orderby country.MedalCount descending
               select country;

            var highGDPCountries =
               from country in grpdCntriesAndMedals
               where (country.MedalCount > minHighMedalCount) && (country.GDP > minHighGDP)
               orderby country.MedalCount descending
               select country;

            var highGDPlowMedalsCountries =
               from country in grpdCntriesAndMedals
               where (country.MedalCount < minHighMedalCount) && (country.GDP > minHighGDP)
               orderby country.MedalCount descending
               select country;

            ViewBag.highGDPlowMedalsCountries = highGDPlowMedalsCountries;
            ViewBag.lowGDPCountries = lowGDPCountries;
            ViewBag.highGDPCountries = highGDPCountries;
            return View();
        }


        public ActionResult ShowCountryMdedalsByYear()
        {
            string country = Request["country"].ToString();
            string medal = Request["medal"].ToString();
            ViewBag.selectedCountry = country;
            ViewBag.selectedMedal = medal;
            var countryEvolution =
                             (from m in olympicModel.Medalists
                              join a in olympicModel.Athletes on m.AthleteID equals a.AthleteID
                              join c in olympicModel.Countries on a.CountryID equals c.CountryID
                              join o in olympicModel.OlympicGames on m.OlympicGameId equals o.OlympicGameID
                              join t in olympicModel.MedalTypes on m.MedalID equals t.MedaTypelD
                              where c.CountryAbr == country && (medal == "Select Medal" || medal == "All Medals" ||(medal != "Select Medal" && t.MedalType1 == medal))
                              group m by new {o.OlympicGameID, o.Year, o.HostingCity, c.CountryName, o.Season} into g
                              orderby g.Count() descending
                              select new CountryMedalsByYear
                              {
                                  CountryName = g.Key.CountryName,
                                  MedalCount = g.Count(),
                                  OlympicGameYear = g.Key.Year,
                                  HostingCity = g.Key.HostingCity,
                                  Season = g.Key.Season
                              }
                          );

            var orderedByYear = (from c in countryEvolution
                                 orderby c.OlympicGameYear descending
                                 select c);
           

            ViewBag.countryEvolution = orderedByYear;
            List<Country> countries = olympicModel.Countries.OrderBy(q => q.CountryName).ToList();
            List<MedalType> medalTypes = olympicModel.MedalTypes.OrderBy(q => q.MedalType1).ToList();
            ViewBag.countries = countries;
            ViewBag.medalTypes = medalTypes;
            return View();
        }
	}
}