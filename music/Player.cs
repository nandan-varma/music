using MediaManager;
using System.Collections.ObjectModel;
using System.Net;

namespace music
{
    static class Player
    {
        public static SongInfo currentSong;
        public static ObservableCollection<SongInfo> downloadedSongsList = new ObservableCollection<SongInfo>();

        public static async Task PlaySong(SongInfo song)
        {
            currentSong = song;
            if(song.DownloadUrl == "") {
                await SaavnApi.GetSongs(song.ID);
                song = SaavnApi.GetSongsList[0];
            }
            await CrossMediaManager.Current.Play(song.DownloadUrl);

        }
        public static async Task DownloadSong(SongInfo song)
        {
            if (song.DownloadUrl == "")
            {
                await SaavnApi.GetSongs(song.ID);
                song = SaavnApi.GetSongsList[0];
            }
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", song.Name + ".m4a");
            // Check if the platform is Android or iOS
            using (WebClient wc = new())
            {
                // wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(
                    new Uri(song.DownloadUrl),
                    downloadPath
                );
                SongInfo downloadedSongInfo = (SongInfo)song.Clone();
                downloadedSongInfo.DownloadUrl = downloadPath;
                downloadedSongsList.Add( downloadedSongInfo );
            }
            void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
            {
                //progressBar.Value = e.ProgressPercentage;
            }
        }
    }
}
