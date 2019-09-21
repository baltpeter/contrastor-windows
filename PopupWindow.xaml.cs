using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Contrastor
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        protected HwndSource _source;
        protected IntPtr _handle;

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem aboutMenuItem;
        private System.Windows.Forms.MenuItem showHideMenuItem;
        private System.Windows.Forms.MenuItem pickFirstMenuItem;
        private System.Windows.Forms.MenuItem pickSecondMenuItem;
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

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _handle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_handle);
            _source.AddHook(HwndHook);

            SetupHotkeys();
        }

        protected override void OnInitialized(EventArgs e)
        {
            // It seems like we need to show the window once before the hotkeys start working.
            Show();
            Hide();

            aboutMenuItem = new System.Windows.Forms.MenuItem("A&bout (License)", (_s, _e) => { MessageBox.Show(new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Contrastor.LICENSE")).ReadToEnd()); });
            showHideMenuItem = new System.Windows.Forms.MenuItem("S&how/Hide", (_s, _e) => { Toggle(); });
            pickFirstMenuItem = new System.Windows.Forms.MenuItem("Pick f&oreground color (Ctrl + Alt + F)", (_s, _e) => { PickColor(0); Show(); });
            pickSecondMenuItem = new System.Windows.Forms.MenuItem("Pick b&ackground color (Ctrl + Alt + B)", (_s, _e) => { PickColor(1); Show(); });
            quitMenuItem = new System.Windows.Forms.MenuItem("Q&uit", (_s, _e) => { Application.Current.Shutdown(); });
            contextMenu = new System.Windows.Forms.ContextMenu(new[] { aboutMenuItem, showHideMenuItem, pickFirstMenuItem, pickSecondMenuItem, quitMenuItem });

            // Adapted after: https://stackoverflow.com/a/1475775
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Text = "Contrastor";
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_Click);
            notifyIcon.Icon = new System.Drawing.Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("Contrastor.app.ico"));
            notifyIcon.ContextMenu = contextMenu;
            notifyIcon.Visible = true;

            base.OnInitialized(e);
        }

        // See: https://www.fluxbytes.com/csharp/how-to-register-a-global-hotkey-for-your-application-in-c/
        protected void SetupHotkeys()
        {
            var res0 = RegisterHotKey(_handle, 0, (int)KeyModifier.Control | (int)KeyModifier.Alt, System.Windows.Forms.Keys.F.GetHashCode());
            var res1 = RegisterHotKey(_handle, 1, (int)KeyModifier.Control | (int)KeyModifier.Alt, System.Windows.Forms.Keys.B.GetHashCode());
        }

        protected void RemoveHotkeys()
        {
            UnregisterHotKey(_handle, 0);
            UnregisterHotKey(_handle, 1);
        }

        public void Toggle()
        {
            Visibility = IsVisible ? Visibility.Hidden : Visibility.Visible;
            if (IsVisible) Activate();
        }

        public void PickColor(int forIndex)
        {
            if (forIndex != 0 && forIndex != 1) return;

            var button = forIndex == 0 ? popupControl.firstEyeDropper.Content : popupControl.secondEyeDropper.Content;
            if (button is System.Windows.Controls.Button)
            {
                (button as System.Windows.Controls.Button).RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
            }
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

        IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;

            if (msg == WM_HOTKEY)
            {
                int id = wParam.ToInt32();
                PickColor(id);
                Show();
            }

            return IntPtr.Zero;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _source.RemoveHook(HwndHook);
            _source = null;
            RemoveHotkeys();

            notifyIcon.Icon.Dispose();
            notifyIcon.Dispose();
            notifyIcon = null;
        }
    }
}
