using MediaManager;
using System.Diagnostics;
using System.Net;

namespace music
{


    public partial class SearchPage : ContentPage
    {
        public TimeSpan currentDuration = CrossMediaManager.Current.Position;
        private TimeSpan completeDuration = TimeSpan.FromSeconds(100); // Example complete duration, replace with actual duration

        public static TimeSpan CurrentDuration
        {
            get => CrossMediaManager.Current.Position;
        }

        public TimeSpan CompleteDuration
        {
            get => completeDuration;
            set
            {
                completeDuration = value;
                OnPropertyChanged(nameof(CompleteDuration));
            }
        }
        public SearchPage()
        {
            InitializeComponent();
            BindingContext = this;
            //CrossMediaManager.Current.Init();
            MusicListView.ItemsSource = SaavnApi.songsList;
        }
        async void OnTextChanged(object sender, EventArgs e)
        {
            try
            {
            SearchBar searchBar = (SearchBar)sender;
            Device.BeginInvokeOnMainThread(() =>{
                SearchSongs(searchBar.Text);
            });

            Debug.WriteLine(searchBar.Text);
            }catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
            }
        }

        private async Task SearchSongs(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                await SaavnApi.GetSongsAsync(query, page: 1, limit: 10);
            }
        }





        private async void OnPlayButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string ID)
            {
                await Player.PlaySong(SaavnApi.songsList.Where(x => x.ID == ID).ElementAt(0));
                CompleteDuration = TimeSpan.FromSeconds(Player.currentSong.duration);
            }
        }
        private async void OnDownloadButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string ID)
            {
                await Player.DownloadSong(SaavnApi.songsList.Where(x => x.ID == ID).ElementAt(0));
            }
        }

    }
}