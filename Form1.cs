using System;
using System.Windows.Forms;
using OAuthProxy.Properties;

namespace OAuthProxy
{
    public partial class Form1 : Form
    {
        private bool _allowVisible; // ContextMenu's Show command used
        private bool _allowClose; // ContextMenu's Exit command used

        private ContextMenuStrip _contextMenu;
        private readonly Proxy _proxy;

        public Form1()
        {
            InitializeComponent();

            AddContextMenu();

            notifyIcon.ShowBalloonTip(2, "OAuth Proxy", "Starting...", ToolTipIcon.Info);
            _proxy = new Proxy();

            OAuthTransformationRule a = new OAuthTransformationRule(
                m => m.Host == "localhost:8050" && m.RequestURI.StartsWith("/api"),
                t =>
                {
                    t.Host = "api.scop-it.com";
                    return t;
                },
                new OAuthSecurityInfo
                {
                    Authority = "https://login.windows.net/scop-it.com",
                    Resource = "http://SCOPIT.onmicrosoft.com/api",
                    ClientId = "d28a8da6-df29-4d79-86d4-012b8abdf073",
                    RedirectUri = new Uri("http://SCOPIT.onmicrosoft.com/apiproxy")
                }
            );
            _proxy.AddRule(a);

            _proxy.Start();
        }

        private void AddContextMenu()
        {
            _contextMenu = new ContextMenuStrip();
            ToolStripItem quitMenuItem = new ToolStripMenuItem("E&xit", Resources.logout, (sender, args) =>
            {
                notifyIcon.ShowBalloonTip(2, "OAuth Proxy", "Stopping...", ToolTipIcon.Info);
                _allowClose = true;
                _proxy.Stop();
                Close();
            });
            _contextMenu.Items.Add(quitMenuItem);

            notifyIcon.ContextMenuStrip = _contextMenu;
        }

        protected override void SetVisibleCore(bool value)
        {
            if (!_allowVisible)
            {
                value = false;
                if (!IsHandleCreated) CreateHandle();
            }
            base.SetVisibleCore(value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_allowClose)
            {
                Hide();
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }
    }
}