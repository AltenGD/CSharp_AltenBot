using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Discord;
using Discord.Commands;
using NReco.ImageGenerator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using CSharp_AltenBot;
namespace CSharp_AltenBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        public async Task Echo ([Remainder]string message)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Echoed message");
            embed.WithDescription(message);
            embed.WithColor(new Color(30, 255, 30));

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("egg")]
        public async Task Egg ()
        {
            await Context.Channel.SendMessageAsync("DECEARING EGG");
        }

        [Command("htmltest")]
        [RequireOwner(Group = "184061887212814336")]
        public async Task HtmlTest (string color = "red")
        {
            string css = "<style>\n h1{\n color: " + color + ";\n }\n</style>\n";
            string html = string.Format("<h1>ya mom gay, {0} </h1>", Context.User.Username);
            var converter = new HtmlToImageConverter
            {
                Width = 350,
                Height = 70
            };
            var pngBytes = converter.GenerateImage(css = html, NReco.ImageGenerator.ImageFormat.Png);
            await Context.Channel.SendFileAsync(new MemoryStream(pngBytes), "Hello.png");
        }

        //Will Update later
        [Command("level")]
        public async Task level (string level, string page = "0")
        {
            string[] PacketSplit = Utilities.GetLevelPacket(level, page);
            string Desc = Encoding.UTF8.GetString(Convert.FromBase64String(PacketSplit[35]));
            string song = WebUtility.UrlDecode(PacketSplit[68]);
            var embed = new EmbedBuilder(); 
            embed.AddField($"<:info:453605020529721356> Level info:", $"{PacketSplit[3]} by {PacketSplit[54]}\n**Description: **{Desc}\n**Difficulty: ** {PacketSplit[27]}☆\n<:download:453603249027678239> {PacketSplit[13]} <:like:453605515302535189> {PacketSplit[19]}", true);
            embed.AddField($"<:info:453605020529721356> Song info:", $"**Song ID: **{PacketSplit[56]}\n**Song Name: **[{PacketSplit[58]}]({song})\n**By: **{PacketSplit[62]}", true);
            embed.WithColor(new Color(40, 255, 197));
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        // cleaned up a little bit of this  
        [Command("user")]
        public async Task user (string user)
        {
            // First post request (gets the ID)
            string egg = Utilities.SendPostRequest("http://boomlings.com/database/getGJUsers20.php", $"gameVersion=21&binaryVersion=35&gdw=0&str={user}&secret=Wmfd2893gb7");
            string[] json = egg.Replace('~', ' ').Split('|', ':');
            Console.WriteLine(egg);

            // Second post request (gets the actual user info)
            string egg2 = Utilities.SendPostRequest("http://boomlings.com/database/getGJUserInfo20.php", $"gameVersion=21&binaryVersion=35&gdw=0&accountID=0&gjp=0&targetAccountID={json[21]}&secret=Wmfd2893gb7");
            string[] json2 = egg2.Replace('~', ' ').Split('|', ':');
            Console.WriteLine(egg2);

            var embed = new EmbedBuilder();

            // YT = 27; Twitter = 53; Twitch = 55
            /*
            YT, Twitt, Twitc
            YT, Twitt
            YT, Twitc
            Twitt, Twitc
            YT
            Twitt
            Twitc
             */

            if (json2[27] != "" && json2[53] != "" && json2[55] != "")
            {
                embed.AddField(json2[1], $"[<:youtube:454688131208314882> Open channel](https://www.youtube.com/channel/{json2[27]}) [<:twitter:454688993297039360> @{json2[53]}](https://twitter.com/{json2[53]}) [<:Twitch_icon:454688993259290654> {json2[55]}](https://www.twitch.tv/{json2[55]})");
            }
            else if (json2[27] != "" && json2[53] != "" && json2[55] == "")
            {
                embed.AddField(json2[1], $"[<:youtube:454688131208314882> Open channel](https://www.youtube.com/channel/{json2[27]}) [<:twitter:454688993297039360> @{json2[53]}](https://twitter.com/{json2[53]})");
            }
            else if (json2[27] != "" && json2[53] == "" && json2[55] != "")
            {
                embed.AddField(json2[1], $"[<:youtube:454688131208314882> Open channel](https://www.youtube.com/channel/{json2[27]}) [<:Twitch_icon:454688993259290654> {json2[55]}](https://www.twitch.tv/{json2[55]})");
            }
            else if (json2[27] == "" && json2[53] != "" && json2[55] != "")
            {
                embed.AddField(json2[1], $"[<:twitter:454688993297039360> @{json2[53]}](https://twitter.com/{json2[53]}) [<:Twitch_icon:454688993259290654> {json2[55]}](https://www.twitch.tv/{json2[55]})");
            }
            else if (json2[27] != "" && json2[53] == "" && json2[55] == "")
            {
                embed.AddField(json2[1], $"[<:youtube:454688131208314882> Open channel](https://www.youtube.com/channel/{json2[27]})");
            }
            else if (json2[27] == "" && json2[53] != "" && json2[55] == "")
            {
                embed.AddField(json2[1], $"[<:twitter:454688993297039360> @{json2[53]}](https://twitter.com/{json2[53]})");
            }
            else if (json2[27] == "" && json2[53] == "" && json2[55] != "")
            {
                embed.AddField(json2[1], $"[<:Twitch_icon:454688993259290654> {json2[55]}](https://www.twitch.tv/{json2[55]})");
            }
            else
            {
                embed.AddField(json2[1], "_(No linked sites found)_");
            }
            embed.AddField("User stats", $"**Global rank: **{json2[47]}\n**Stars: **{json2[13]}\n**Diamonds: **{json2[15]}\n**Official coins: **{json2[5]}\n**User coins: **{json2[7]}");

            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("shutdown")]
        [RequireOwner(Group = "184061887212814336")]
        public async Task shutdown ()
        {
            await Context.Channel.SendMessageAsync("Shutting down...");
            await Context.Client.SetStatusAsync(UserStatus.Invisible);

            await Context.Client.StopAsync();
            await Task.Delay(5000);
            Environment.Exit(0);
        }

        [Command("setgame")]
        [RequireOwner(Group = "184061887212814336")]
        public async Task game ([Remainder]string gameMessage)
        {
            await Context.Channel.SendMessageAsync($"Setting game status to: {gameMessage}");
            await Context.Client.SetGameAsync(gameMessage);
        }

        [Command("texttoimage")]
        [Alias("t2i")]
        public async Task texttoimage ([Remainder] string message)
        {
            string css = "<style>\n h1{\n color: rgb(51,51,51);\n }\n</style>\n";
            string html = string.Format("<h1>{0}</h1>", message);
            var converter = new HtmlToImageConverter
            {
                Width = 1920,
                Height = 600
            };
            var pngBytes = converter.GenerateImage(css = html, NReco.ImageGenerator.ImageFormat.Png);
            await Context.Channel.SendFileAsync(new MemoryStream(pngBytes), "gya.png");
        }

        [Command("renderhtml")]
        [Alias("rhtml", "reht")]
        public async Task RenderHTML ([Remainder]string url)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        string myContent = await content.ReadAsStringAsync();
                        string html = string.Format("{0}", myContent);  
                        var converter = new HtmlToImageConverter
                        {
                            Width = 1920,
                            Height = 1080
                        };
                        var pngBytes = converter.GenerateImage(html, NReco.ImageGenerator.ImageFormat.Png);
                        await Context.Channel.SendFileAsync(new MemoryStream(pngBytes), "gya.png");
                    }
                }
            }
        }

        [Command("color")]
        [Alias("col", "colour")]
        public async Task color ([Remainder] string col)
        {
            string html = string.Format("<body bgcolor=\"{0}\">", col);
            var converter = new HtmlToImageConverter
            {
                Width = 800,
                Height = 800
            };
            var pngBytes = converter.GenerateImage(html, NReco.ImageGenerator.ImageFormat.Png);
            await Context.Channel.SendFileAsync(new MemoryStream(pngBytes), "gya.png");
        }

        [Command("help")]
        [Alias("commands")]
        public async Task help ()
        {
            var embed = new EmbedBuilder();
            embed.AddField("Echo, Aliases: None", "Echoes whatever you want");
            embed.AddField("Egg, Aliases: None", "egg");
            embed.AddField("Level, Aliases: None", "Shows information about a level");
            embed.AddField("User, Aliases: None", "Shows information on a user");
            embed.AddField("TextToImage, Aliases: T2I", "Makes a text into an image");
            embed.AddField("RenderHTML, Aliases: rhtml, reht", "Renders a site (css will be missing if its on a different file)");
            embed.AddField("Color, Aliases: col, colour", "Shows a color of your choice");
            embed.AddField("Help, Aliases: Commands", "Shows all the available commands");
            embed.WithColor(new Color(114, 137, 218));

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
