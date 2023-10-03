using MediaManager;
using System.Diagnostics;
using System.Net;

namespace music
{


    public partial class LibraryPage : ContentPage
    {
        public LibraryPage()
        {
            InitializeComponent();
            DownloadsListView.ItemsSource = Player.downloadedSongsList;
        }
        private async void OnPlayButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string ID)
            {
                await Player.PlaySong(SaavnApi.songsList.Where(x => x.ID == ID).ElementAt(0));
            }
        }
    }
}