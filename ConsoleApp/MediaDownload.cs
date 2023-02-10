
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class MediaDownload
    {
        private readonly HttpClient client;
        private readonly WebClient webClient;
        

        public MediaDownload()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 Edg/104.0.1293.70");

        }

        public async Task<int> DownLoadFile(string url)
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var fileName = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + ".mp4";

            var filepath = Path.Combine(dir, fileName);
            try
            {
                var resp = await client.GetStreamAsync(url);
                using (var fs = new FileStream(filepath, FileMode.Create))
                {
                    await resp.CopyToAsync(fs);
                }
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.Delay(1000);
                //throw;
            }
            return 0;


        }


        public async Task<VideoItemInfo_Item[]> GetVideoItemInfoAsync(params string[] itemIds)
        {
            var newIds = itemIds.Where(m => m.StartsWith("http")).Select(m => new Uri(m).LocalPath.Replace("/video/", String.Empty));
            var item_ids = string.Join(",", newIds);
            var itemurl = $"https://www.iesdouyin.com/web/api/v2/aweme/iteminfo/?item_ids={item_ids}";
            var videoInfo = await client.GetStringAsync(itemurl);

            var jsonObj = JsonSerializer.Deserialize<VideoItemInfoModel>(videoInfo);

            if (jsonObj != null && jsonObj.status_code == 0)
            {
                return jsonObj.item_list.Where(m => m.video != null).Where(m => m.video.play_addr != null).ToArray();

            }
            return null;
        }
    }
}
