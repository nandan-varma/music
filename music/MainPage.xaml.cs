using MediaManager;
using System.Diagnostics;
using System.Net;

namespace music
{


    public partial class MainPage : ContentPage
    {
        private readonly SaavnApi saavnApi;
        private SongInfo currentSong;
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
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            //CrossMediaManager.Current.Init();
            saavnApi = new SaavnApi();
            MusicListView.ItemsSource = saavnApi.songsList;
        }
        async void OnTextChanged(object sender, EventArgs e)
        {
            try
            {
            SearchBar searchBar = (SearchBar)sender;
            await SearchSongs(searchBar.Text);
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
                await saavnApi.GetSongsAsync(query, page: 1, limit: 10);
            }
        }

        private async Task PlaySong(SongInfo song)
        {
            currentSong = song;
            await CrossMediaManager.Current.Play(song.DownloadUrl);
            CompleteDuration = TimeSpan.FromSeconds(song.duration);
            CurrentImage.Source = song.ImageUrl;
        }

        private async Task DownloadSong(SongInfo song)
        {
            // Check if the platform is Android or iOS
            using (WebClient wc = new WebClient())
            {
                // wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(
                    new Uri(song.DownloadUrl),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", song.Name + ".m4a")
                );
            }
        }

        private async void OnPlayButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string ID)
            {
                await PlaySong(saavnApi.songsList.Where(x => x.ID == ID).ElementAt(0));
            }
        }
        private async void OnDownloadButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string ID)
            {
                await DownloadSong(saavnApi.songsList.Where(x => x.ID == ID).ElementAt(0));
            }
        }

        private void OnPlayPauseButtonClicked(object sender, EventArgs e)
        {
            CrossMediaManager.Current.PlayPause();
        }
        private void OnForwardClicked(object sender, EventArgs e)
        {
            CrossMediaManager.Current.StepForward();
        }
        private void OnBackWardClicked(object sender, EventArgs e)
        {
            CrossMediaManager.Current.StepBackward();
        }

    }
}