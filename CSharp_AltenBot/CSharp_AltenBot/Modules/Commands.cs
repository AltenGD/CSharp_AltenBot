using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using NReco.ImageGenerator;
using System.Net.Http;


namespace CSharp_AltenBot.Modules
{
    public class NormalCommands
    {
        [Command("echo")]
        public async Task Echo(CommandContext context, params string[] message)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Echoed message",
                Description = string.Join(" ", message),
                Color = new DiscordColor(0x1EFF1E),
            };

            await context.Message.RespondAsync("", false, embed);
        }


        [Command("egg")]
        public async Task Eggs(CommandContext Context)
        {
            await Context.Channel.SendMessageAsync("DECEARING EGG");
        }

        [Command("htmltest")]
        public async Task HtmlTest(CommandContext Context, string color = "red")
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
        public async Task level(CommandContext Context, string level)
        {
            string[] PacketSplit = Utilities.GetLevelPacket(level);
            string Desc = Encoding.UTF8.GetString(Convert.FromBase64String(PacketSplit[35]));
            string song = WebUtility.UrlDecode(PacketSplit[68]);
            var embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(0x28FFC5),
            };
            embed.AddField($"<:info:453605020529721356> Level info:", $"{PacketSplit[3]} by {PacketSplit[54]}\n**Description: **{Desc}\n**Difficulty: ** {PacketSplit[27]}☆\n<:download:453603249027678239> {PacketSplit[13]} <:like:453605515302535189> {PacketSplit[19]}", true);
            embed.AddField($"<:info:453605020529721356> Song info:", $"**Song ID: **{PacketSplit[56]}\n**Song Name: **[{PacketSplit[58]}]({song})\n**By: **{PacketSplit[62]}", true);
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("user")]
        public async Task User(CommandContext Context, params string[] u)
        {
            string user = string.Join(" ", u);
            // First post request (gets the ID)

            WebRequest request = WebRequest.Create("http://boomlings.com/database/getGJUsers20.php");
            request.Method = "POST";

            string postData = $"gameVersion=21&binaryVersion=35&gdw=0&str={user}&secret=Wmfd2893gb7";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);

            dataStream.Close();
            WebResponse response = request.GetResponse();

            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            // 21
            string[] json = responseFromServer.Replace('~', ' ').Split('|', ':');

            Console.WriteLine($"Original text: {responseFromServer}");

            reader.Close();
            dataStream.Close();
            response.Close();

            // Second post request (gets the actual user info)

            WebRequest request2 = WebRequest.Create("http://boomlings.com/database/getGJUserInfo20.php");
            request2.Method = "POST";

            string postData2 = $"gameVersion=21&binaryVersion=35&gdw=0&accountID=0&gjp=0&targetAccountID={json[21]}&secret=Wmfd2893gb7";
            byte[] byteArray2 = Encoding.UTF8.GetBytes(postData2);

            request2.ContentType = "application/x-www-form-urlencoded";
            request2.ContentLength = byteArray2.Length;

            Stream dataStream2 = request2.GetRequestStream();
            dataStream2.Write(byteArray2, 0, byteArray2.Length);

            dataStream2.Close();
            WebResponse response2 = request2.GetResponse();

            Console.WriteLine(((HttpWebResponse)response2).StatusDescription);
            dataStream2 = response2.GetResponseStream();

            StreamReader reader2 = new StreamReader(dataStream2);
            string responseFromServer2 = reader2.ReadToEnd();

            // 21
            string[] json2 = responseFromServer2.Replace('~', ' ').Split('|', ':');

            Console.WriteLine($"Original text: {responseFromServer2}");


            var embed = new DiscordEmbedBuilder();

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

            reader2.Close();
            dataStream2.Close();
            response2.Close();
        }

        [Command("shutdown")]
        public async Task Shutdown(CommandContext Context)
        {
            await Context.Channel.SendMessageAsync("Shutting down...");
            await Context.Client.DisconnectAsync();
            Environment.Exit(0);
        }

        [Command("setgame")]
        public async Task SetGame(CommandContext Context, params string[] gameMessage)
        {
            string game = string.Join(" ", gameMessage);
            await Context.Channel.SendMessageAsync($"Setting game status to: {game}");
            await Context.Client.UpdateStatusAsync(new DiscordGame(game));
        }

        [Command("texttoimage")]
        [Aliases("t2i")]
        public async Task TextToImage(CommandContext Context, params string[] msg)
        {
            string message = string.Join(" ", msg);
            string css = "<style>\n h1{\n color: rgb(51,51,51);\n }\n</style>\n";
            string html = string.Format("<h1>{0}</h1>", message);
            var converter = new HtmlToImageConverter
            {
                Width = 1920,
                Height = 600
            };
            var pngBytes = converter.GenerateImage(css = html, ImageFormat.Png);
            await Context.Channel.SendFileAsync(new MemoryStream(pngBytes), "gya.png");
        }

        [Command("renderhtml")]
        public async Task RenderHTML(CommandContext Context, params string[] url)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(string.Join(" ", url)))
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
        public async Task Color(CommandContext Context, params string[] col)
        {
            string html = string.Format("<body bgcolor=\"{0}\">", col);
            var converter = new HtmlToImageConverter
            {
                Width = 800,
                Height = 800
            };
            var pngBytes = converter.GenerateImage(html, ImageFormat.Png);
            await Context.Channel.SendFileAsync(new MemoryStream(pngBytes), "gya.png");
        }

        [Command("help")]
        public async Task Help(CommandContext Context)
        {
            var embed = new DiscordEmbedBuilder();
            embed.AddField("Echo, Aliases: None", "Echoes whatever you want");
            embed.AddField("Egg, Aliases: None", "egg");
            embed.AddField("Level, Aliases: None", "Shows information about a level");
            embed.AddField("User, Aliases: None", "Shows information on a user");
            embed.AddField("TextToImage, Aliases: T2I", "Makes a text into an image");
            embed.AddField("RenderHTML, Aliases: rhtml, reht", "Renders a site (css will be missing if its on a different file)");
            embed.AddField("Color, Aliases: col, colour", "Shows a color of your choice");
            embed.AddField("Help, Aliases: Commands", "Shows all the available commands");
            embed.WithColor(new DiscordColor(0x7289DA));

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
