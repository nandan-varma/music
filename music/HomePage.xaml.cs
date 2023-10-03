namespace music
{


    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            RecommendedListView.ItemsSource = SaavnApi.RecommendedSongsList;
            Device.BeginInvokeOnMainThread(() =>
            {
                SaavnApi.GetRecommendedAsync();
            });
        }
        private async void OnPlayButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string ID)
            {
                await Player.PlaySong(SaavnApi.RecommendedSongsList.Where(x => x.ID == ID).ElementAt(0));
            }
        }
        private async void OnDownloadButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string ID)
            {
                await Player.DownloadSong(SaavnApi.RecommendedSongsList.Where(x => x.ID == ID).ElementAt(0));
            }
        }
    }
}