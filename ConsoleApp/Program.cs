// See https://aka.ms/new-console-template for more information
using ConsoleApp;
using System.Net;



Console.WriteLine("请输入要下载的txt文件 !(视频地址一行一个)");

var filename = Console.ReadLine();
if (!File.Exists(filename))
{
    Console.WriteLine("文件不存在");
    return;
}
var urls = File.ReadAllLines(filename).Where(m => m.StartsWith("http")).Distinct();


var md = new MediaDownload();

var num = 0;
var download = 0;
while (urls != null)
{
    var url = urls.Skip(num * 5).Take(5);
    num++;

    if (!url.Any())
        break;

    var iteminfos = await md.GetVideoItemInfoAsync(url.ToArray());
    foreach (var item in iteminfos)
    {

        download++;
        Console.WriteLine($"{download}/{urls.Count()} download {item.aweme_id}");
        await md.DownLoadFile(item.video.play_addr.playUrl);
        await Task.Delay(2000);
    }
}


Console.WriteLine("操作完成");
Console.ReadLine();

