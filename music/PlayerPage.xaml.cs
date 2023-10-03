using MediaManager;

namespace music
{


    public partial class PlayerPage : ContentPage
    {
        public PlayerPage()
        {
            InitializeComponent();
            TitleLabel.Text = Player.currentSong?.Name;
            AlbumLabel.Text = Player.currentSong?.Album;
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