using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using System.Web;

public class SongInfo : ICloneable
{
    public string ID { get; set; }
    public string Name { get; set; }
    public int duration { get; set; }
    public string Album { get; set; }
    public string Artists { get; set; }
    public string Url { get; set; }
    public string ImageUrl { get; set; }
    public string DownloadUrl { get; set; }  // Add the download URL property
    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
public static class SaavnApi
{
    public static ObservableCollection<SongInfo> songsList = new ObservableCollection<SongInfo>();
    public static ObservableCollection<SongInfo> RecommendedSongsList = new ObservableCollection<SongInfo>();
    public static ObservableCollection<SongInfo> GetSongsList = new ObservableCollection<SongInfo>();
    private const string ApiUrl = "https://saavn-api.nandanvarma.com/";
    private static void DecodeSongList(JArray results,ObservableCollection<SongInfo> collection)
    {
        foreach (var result in results)
        {
            SongInfo song = new()
            {
                ID = result["id"].ToString(),
                Name = HttpUtility.HtmlDecode(result["name"].ToString()),
                duration = Int32.Parse(result["duration"].ToString()),
                Album = result["album"]["name"].ToString(),
                //Artists = result["primaryArtists"].ToString(),
                Url = result["url"].ToString(),
                ImageUrl = result["image"][1]["link"].ToString(),
                //DownloadUrl = result["downloadUrl"][4]["link"].ToString()
                DownloadUrl = ""
            };
            collection.Add(song);
        }
    }
    public static async Task GetSongsAsync(string query, int page = 1, int limit = 10)
    {
        query.Replace(" ", "+");
        string apiUrl = $"{ApiUrl}search/songs?query={query}&page={page}&limit={limit}";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseData);

                    if (json["status"].ToString() == "SUCCESS")
                    {
                        JArray results = json["data"]["results"] as JArray;
                        songsList.Clear();

                        DecodeSongList(results, songsList);
                    }
                    else
                    {
                        Console.WriteLine("API request failed.");
                    }
                }
                else
                {
                    Console.WriteLine("API request failed with status code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
    public static async Task GetRecommendedAsync(string languages="telugu")
    {
        string apiUrl = $"{ApiUrl}modules?language={languages}";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseData);

                    if (json["status"].ToString() == "SUCCESS")
                    {
                        JArray results = json["data"]["trending"]["songs"] as JArray;
                        RecommendedSongsList.Clear();
                        DecodeSongList(results, RecommendedSongsList);
                    }
                    else
                    {
                        Console.WriteLine("API request failed.");
                    }
                }
                else
                {
                    Console.WriteLine("API request failed with status code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
    public static async Task GetSongs(string SongIDs)
        {
        string apiUrl = $"{ApiUrl}songs?id={SongIDs}";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseData);

                    if (json["status"].ToString() == "SUCCESS")
                    {
                        JArray results = json["data"] as JArray;
                        GetSongsList.Clear();

                        foreach (var result in results)
                        {
                            SongInfo song = new()
                            {
                                ID = result["id"].ToString(),
                                Name = HttpUtility.HtmlDecode(result["name"].ToString()),
                                duration = Int32.Parse(result["duration"].ToString()),
                                Album = result["album"]["name"].ToString(),
                                Artists = result["primaryArtists"].ToString(),
                                Url = result["url"].ToString(),
                                ImageUrl = result["image"][1]["link"].ToString(), // Modify this to suit your needs
                                DownloadUrl = result["downloadUrl"][4]["link"].ToString() // Modify this to suit your needs
                            };
                            GetSongsList.Add(song);
                        }
                    }
                    else
                    {
                        Console.WriteLine("API request failed.");
                    }
                }
                else
                {
                    Console.WriteLine("API request failed with status code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }

}
