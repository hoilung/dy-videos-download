using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class VideoItemInfoModel
    {

        public int status_code { get; set; }

        public VideoItemInfo_Item[] item_list { get; set; }

    }
    public class VideoItemInfo_Item
    {

        public string aweme_id { get; set; }

        public string desc { get; set; }

        public VideoItemInfo_Video video { get; set; }

    }

    public class VideoItemInfo_Video
    {
        public VideoItemInfo_Video_Play_Addr play_addr { get; set; }
    }

    public class VideoItemInfo_Video_Play_Addr
    {
        public string uri { get; set; }

        public string[] url_list { get; set; }

        [JsonIgnore]
        public string playUrl
        {
            get
            {
                if (url_list != null && url_list.Length > 0)
                {
                    return url_list[0].Replace("/playwm/", "/play/");
                }
                return string.Empty;
            }
        }
    }

}
