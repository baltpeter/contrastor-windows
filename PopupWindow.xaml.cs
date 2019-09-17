using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Contrastor
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem aboutMenuItem;
        private System.Windows.Forms.MenuItem showHideMenuItem;
        private System.Windows.Forms.MenuItem quitMenuItem;

        public PopupWindow()
        {
            InitializeComponent();

            SizeChanged += (o, e) =>
            {
                Left = SystemParameters.WorkArea.Right - ActualWidth;
                Top = SystemParameters.WorkArea.Bottom - ActualHeight;
            };
            KeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape) Visibility = Visibility.Hidden;
            };
        }

        protected override void OnInitialized(EventArgs e)
        {
            Visibility = Visibility.Hidden;

            aboutMenuItem = new System.Windows.Forms.MenuItem("A&bout (License)", (_s, _e) => { MessageBox.Show(new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Contrastor.LICENSE")).ReadToEnd()); });
            showHideMenuItem = new System.Windows.Forms.MenuItem("S&how/Hide", (_s, _e) => { Toggle(); });
            quitMenuItem = new System.Windows.Forms.MenuItem("Q&uit", (_s, _e) => { Application.Current.Shutdown(); });
            contextMenu = new System.Windows.Forms.ContextMenu(new[] { aboutMenuItem, showHideMenuItem, quitMenuItem });

            // Adapted after: https://stackoverflow.com/a/1475775
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Text = "Contrastor";
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_Click);
            notifyIcon.Icon = new System.Drawing.Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("Contrastor.app.ico"));
            notifyIcon.ContextMenu = contextMenu;
            notifyIcon.Visible = true;

            base.OnInitialized(e);
        }

        public void Toggle()
        {
            Visibility = IsVisible ? Visibility.Hidden : Visibility.Visible;
            if (IsVisible) Activate();
        }

        void notifyIcon_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) Toggle();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Make the window movable, see: https://stackoverflow.com/a/7418629
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }
    }
}
