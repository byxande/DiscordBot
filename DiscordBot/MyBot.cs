using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Drawing;
using System.Drawing.Imaging;
using RiotSharp;
using RiotSharp.LeagueEndpoint.Enums;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace DiscordBot
{
    internal class MyBot
    {
        private readonly CommandService comandos;
        private readonly DiscordClient discord;
        private List<string> memesList;


        public MyBot()
        {

            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });

            #region Comandos

            comandos = discord.GetService<CommandService>();
            comandos.CreateCommand("Oi")
                .Parameter("user", ParameterType.Required)
                .Do(async e =>
                {
                    var t = e.GetArg("user");
                    var toReturn = $"Olá {e.GetArg("user")}!";
                    await e.Channel.SendMessage(toReturn);
                });

            comandos.CreateCommand("addMeme")
                .Parameter("memeURL", ParameterType.Required)
                .Do(async e =>
                {
                    addMemesList(e.GetArg("memeURL"));
                    await e.Channel.SendMessage("Meme adicionado com sucesso!");
                });

            comandos.CreateCommand("PM")
                .Parameter("user", ParameterType.Unparsed)
                .Do(async e =>
                {
                    User user = null;
                    try
                    {
                        user = e.Server.FindUsers(e.GetArg("user")).First();
                        Channel x = await e.User.CreatePMChannel();
                    }
                    catch (InvalidOperationException)
                    {
                        await e.Channel.SendMessage($"Usuário {e.GetArg("user")} não existe.");
                        return ;
                    }

                });

            comandos.CreateCommand("testeRiot")
                .Parameter("nome", ParameterType.Unparsed)
                .Do(async e =>
                {
                   
                    RiotRepository riot = new RiotRepository();
                    GenerateImage();
                    var a = riot.ByName(riot.Region("br"), e.GetArg("nome"));

                    //foreach (var b in a.GetStatsRanked())
                    //{

                    //}
                    foreach (var league in a.GetLeagues())
                    {
                        
                        await e.Channel.SendFile(@"C:\Users\Alexandre\Documents\Visual Studio 2015\Projects\DiscordBot\DiscordBot\bin\Debug\imageTestando.png");

                        await e.Channel.SendMessage(league.Tier.ToString() + " " + league.Entries.First().Division.ToString());
                        await e.Channel.SendMessage(league.Entries.First().LeaguePoints.ToString() + "pdl");
                        break;
                    }


                });

            comandos.CreateCommand("summ")
                .Parameter("nome", ParameterType.Required)
                .Parameter("region", ParameterType.Unparsed)
                .Do(async e =>
                {
                    RiotRepository riot = new RiotRepository();
                    

                    var a = riot.ByName(riot.Region(e.GetArg("region")), e.GetArg("nome"));
                    var b = a.GetStatsRanked();
                    
                    
                    foreach (var teste in b)
                    {
                        
                        await e.Channel.SendMessage(teste.Stats.RankedSoloGamesPlayed.ToString());
                        await e.Channel.SendMessage(teste.Stats.MaxTimePlayed.ToString());
                    }
                });


            comandos.CreateCommand("meme").
                Do(async e => { await e.Channel.SendMessage(randomMeme()); });

            comandos.CreateCommand("kick")
                .Parameter("user", ParameterType.Required)
                .Do(async e =>
                {
                    if (e.User.ServerPermissions.KickMembers)
                    {
                        User user = null;
                        try
                        {
                            user = e.Server.FindUsers(e.GetArg("user")).First();
                            await e.User.Kick();
                        }
                        catch (InvalidOperationException)
                        {
                            await e.Channel.SendMessage($"Usuário {e.GetArg("User")} não existe.");
                            return;
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage($"{e.User.Name} não tem permissão para kickar.");
                    }

                });


            discord.ExecuteAndWait(
                async () =>
                {
                    await discord.Connect("Mjk0NTgxMTg4MjUxODc3Mzc3.C7YiZg.6MR5mbIWewmmX7vRDtEkm4BfbiQ", TokenType.Bot);
                });


            #endregion
        }


        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.Write(e.Message);
        }

        #region Metodos

        public static void GenerateImage()
        {
            Image image = HtmlRender.RenderToImageGdiPlus("<p><html><head><title>HTML Table Cellpadding</title></head><body><table border=\"1\" cellpadding=\"5\" cellspacing=\"5\"><tr><th>Name</th><th>Salary</th></tr><tr><td>Ramesh Raman</td><td>5000</td></tr><tr><td>Shabbir Hussein</td><td>7000</td></tr></table></body></html></p>");
            image.Save("imageTestando.png", ImageFormat.Png);
            
        }

        public void addMemesList(string memeUrl)
        {
            memesList = new List<string>();
            memesList.Add(memeUrl);
        }

        public string randomMeme()
        {
            var random = new Random();
            var index = random.Next(memesList.Count);
            return memesList.ElementAt(index);
        }

        #endregion
    }
}
