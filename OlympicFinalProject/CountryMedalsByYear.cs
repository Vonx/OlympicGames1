using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OlympicFinalProject
{
    public class CountryMedalsByYear
    {
        public string CountryName { get; set; }
        public int MedalCount { get; set; }
        public int OlympicGameYear { get; set; }
        public string HostingCity{ get; set; }
        public string Season { get; set; }
    }
}