using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBot.Core
{
    public static class Responses
    {
        public static List<string> HiGreetingsResponse(string name)
        {
            var greetings = new List<string> { "hi", "hello", "hey" };
            return AppendName(greetings, name);
        }
        public static List<string> GoodMorningGreetingsResponse(string name)
        {
            var greetings = GetGreeting();
            return AppendName(greetings, name);
        }

        public static List<string> CallResponse()
        {
            return new List<string>
            {
                "I am little busy right now. Can we talk after an hour?",
                "In a meeting",
                "Busy now. Can we talk later?"
            };
        }

        public static List<string> INQUIRY_TEAM_BUDGETResponse(LuisResult result)
        {
            var intents = new List<IntentRecommendation>(result.Intents);
            var entities = new List<EntityRecommendation>(result.Entities);

            //System.Console.WriteLine("<LyncLuisDialog> LuisIntent [INQUIRY_TEAM_BUDGET] result : " + result.Query);
            System.Console.WriteLine("--> INQUIRY_TEAM_BUDGETResponse result count : " + intents.Count);
            foreach (var intent in intents)
            {
                System.Console.WriteLine("--> intent : [" + intent.Score + "],[" + intent.Intent + "]");
            }

            foreach (var entity in entities)
            {
                System.Console.WriteLine("--> entity : [" + entity.Score + "],[" + entity.Entity + "]");
            }

            return new List<string> { "한글" };
        }

        private static List<string> GetGreeting()
        {
            var afternoon = 12;
            var evening = 16;
            if (DateTime.Now.Hour < afternoon)
                return new List<string> { "Good Morning", "gm", "vgm" };
            else if (DateTime.Now.Hour < evening)
                return new List<string> { "Good Afternoon" };
            else
                return new List<string> { "Good Evening" };
        }

        private static List<string> AppendName(List<string> responses, string name)
        {
            if (!string.IsNullOrEmpty(name))
                responses = responses.Concat(responses.Select(t => t + " " + name)).ToList();
            return responses;
        }
    }
}
