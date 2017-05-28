using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncBot.Core.Dialogs
{
    //[LuisModel("model id", "subs key")]
    //[LuisModel("632a1601-bd2a-447b-ae02-412cdfb081ef", "848ed4e5f0d84d0086a3738a92216299")]

    // 이승무대리 Luis
    [LuisModel("4bba1dbf-a2f8-4f24-998e-bffa52d0f749", "a525ffedbc0547aeb3542351f89b96b2")]
    [Serializable]
    public class LyncLuisDialog : LuisDialog<object>
    {
        private PresenceService _presenceService;

        public LyncLuisDialog(PresenceService presenceService)
        {
            _presenceService = presenceService;
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            //await context.PostAsync("I'm sorry. I didn't understand you.");
            // Dont do anything. Pretend I am busy.
            context.Wait(MessageReceived);
        }

        [LuisIntent("HiGreetings")]
        public async Task HiGreetings(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            var activity = await message;
            string name = GetName(activity.From);
            await context.PostOnlyOnceAsync(Responses.HiGreetingsResponse(name), nameof(HiGreetings));
            context.Wait(MessageReceived);
        }

        [LuisIntent("GoodMorningGreetings")]
        public async Task GoodMorningGreetings(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            string name = string.Empty;
            if (!context.PrivateConversationData.ContainsKey(nameof(HiGreetings)))
            {
                var activity = await message;
                name = GetName(activity.From);
            }
            await context.PostOnlyOnceAsync(Responses.GoodMorningGreetingsResponse(name), nameof(GoodMorningGreetings));
            context.Wait(MessageReceived);
        }

        [LuisIntent("Call")]
        public async Task Call(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            await context.PostOnlyOnceAsync(Responses.CallResponse(), nameof(Call));
            _presenceService.SetPresenceBusy();
            context.Wait(MessageReceived);
        }

        [LuisIntent("INQUIRY_TEAM_BUDGET")]
        public async Task INQUIRY_TEAM_BUDGET(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            //System.Console.WriteLine("<LyncLuisDialog> LuisIntent [INQUIRY_TEAM_BUDGET] message : " + message);
            System.Console.WriteLine("<LyncLuisDialog> LuisIntent [INQUIRY_TEAM_BUDGET] result : " + result.Query);

            /*
            var intents = new List<IntentRecommendation>(result.Intents);
            var entities = new List<EntityRecommendation>(result.Entities);

            System.Console.WriteLine("<LyncLuisDialog> LuisIntent [INQUIRY_TEAM_BUDGET] result : " + result.Query);
            System.Console.WriteLine("--> result count : " + intents.Count);
            foreach (var intent in intents)
            {
                System.Console.WriteLine("--> intent : [" + intent.Score + "],[" + intent.Intent + "]");
            }

            foreach (var entity in entities)
            {
                System.Console.WriteLine("--> entity : [" + entity.Score + "],[" + entity.Entity + "]");
            }
            */
            await context.PostAsync(Responses.INQUIRY_TEAM_BUDGETResponse(result), nameof(INQUIRY_TEAM_BUDGET));
            context.Wait(MessageReceived);
        }

        private static string GetName(ChannelAccount from)
        {
            string name = string.Empty;
            if (string.IsNullOrEmpty(from.Name))
                return name;

            var res = from.Name.Split(' ');
            foreach (var item in res)
            {
                if (item.Length > 1)
                {
                    name = item;
                    break;
                }
            }
            return name;
        }
    }
}
