using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

public class SongInfo
{
    public string ID { get; set; }
    public string Name { get; set; }
    public int duration { get; set; }
    public string Album { get; set; }
    public string Artists { get; set; }
    public string Url { get; set; }
    public string ImageUrl { get; set; }
    public string DownloadUrl { get; set; }  // Add the download URL property
}
public class SaavnApi
{
    public ObservableCollection<SongInfo> songsList = new ObservableCollection<SongInfo>();
    private const string ApiUrl = "https://saavn-api.nandanvarma.com/search/songs";

    public async Task GetSongsAsync(string query, int page = 1, int limit = 10)
    {
        string apiUrl = $"{ApiUrl}?query={query}&page={page}&limit={limit}";

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

                        foreach (var result in results)
                        {
                            SongInfo song = new()
                            {
                                ID = result["id"].ToString(),
                                Name = result["name"].ToString(),
                                duration = Int32.Parse(result["duration"].ToString()),
                                Album = result["album"]["name"].ToString(),
                                Artists = result["primaryArtists"].ToString(),
                                Url = result["url"].ToString(),
                                ImageUrl = result["image"][1]["link"].ToString(), // Modify this to suit your needs
                                DownloadUrl = result["downloadUrl"][4]["link"].ToString() // Modify this to suit your needs
                            };
                            songsList.Add(song);
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
