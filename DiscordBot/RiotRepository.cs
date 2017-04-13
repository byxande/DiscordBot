using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp;
using RiotSharp.SummonerEndpoint;

namespace DiscordBot
{
    class RiotRepository
    {
        public RiotRepository()
        { }

        public RiotSharp.Region Region(string region)
        {
            region.ToLower();
            if (region == "br")
                return RiotSharp.Region.br;
            else if (region == "na")
                return RiotSharp.Region.na;
            else
                return RiotSharp.Region.kr;
        }
        
        private RiotApi api = RiotApi.GetInstance("RGAPI-D894C917-C402-40F9-82C9-1B40F02FEF47", 10, 500);

        public Summoner ByName(RiotSharp.Region region, string name)
        {
            var inv = api.GetSummoner(region, name);
            return inv;
        }
       

    }
}
