using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using TaterNotify.Properties;

namespace TaterNotify
{
    class CustomApplicationContext : ApplicationContext
    {
        private const int BalloonMillis = 25000;
        private const int PollDelay = 30000;
        private const int NewDayCutoffHours = -8;

        private static bool _inErrorState;

        private static readonly WebClient WebClient = new WebClient();
        private static readonly SoundPlayer HomeRunSound = new SoundPlayer(Resources.Ball_Hit_Cheer);
        private readonly NotifyIcon _tray;
        private readonly Worker _theWorker;

        private readonly ToolStripMenuItem _menuDisplayForm;

        private List<Tater> _taters;
        private List<TaterStandingsRow> _standings;

        private string _dateString;

        public CustomApplicationContext()
        {
            WebClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2062.103 Safari/537.36");
            _menuDisplayForm = new ToolStripMenuItem("Live Tater Standings");
            ToolStripMenuItem menuSettings = new ToolStripMenuItem("Settings");
            ToolStripSeparator menuSep = new ToolStripSeparator();
            ToolStripMenuItem menuExit = new ToolStripMenuItem("Exit");
            ContextMenuStrip theMainMenu = new ContextMenuStrip();
            theMainMenu.Items.AddRange(new ToolStripItem[] {_menuDisplayForm, menuSettings, menuSep, menuExit});

            _tray = new NotifyIcon
            {
                Icon = Resources.potato,
                ContextMenuStrip = theMainMenu,
                Text = Resources.CustomApplicationContext_TaterNotify,
                Visible = true
            };

            ThreadExit += OnThreadExit;
            _menuDisplayForm.Click += MenuDisplayForm_Click;
            menuSettings.Click += MenuSettings_Click;
            menuExit.Click += MenuExit_Click;
            _tray.DoubleClick += Tray_DoubleClick;

            _taters = new List<Tater>();
            _standings = new List<TaterStandingsRow>();

            _theWorker = new Worker(_tray) {WorkerReportsProgress = true, WorkerSupportsCancellation = true};
            _theWorker.DoWork += DoWork;
            _theWorker.ProgressChanged += ProgressChanged;
            _theWorker.Disposed += DisposeWorker;
            _theWorker.RunWorkerAsync();

        }

        private void DoWork(object sender, EventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker == null) return;

            while (true)
            {
                try
                {
                    if (!_standings.Any()) GetStandings();

                    string newDateString = DateTime.Now.AddHours(NewDayCutoffHours).ToString("yyyyMMdd");
                    if (!newDateString.Equals(_dateString))
                    {
                        _taters.Clear();
                        _dateString = newDateString;
                        GetStandings();
                    }

                    List<Tater> latestTaters = GetLatestTaters();
                    List<Tater> taterDiff = latestTaters
                        .Except(_taters)
                        .OrderByDescending(x => x.Owner)
                        .ThenByDescending(x => x.Taters)
                        .ToList();

                    if (taterDiff.Any()) worker.ReportProgress(0, taterDiff);
                    _taters = latestTaters;

                    if (_inErrorState) ExitErrorState();
                }
                catch (WebException)
                {
                    if (!_inErrorState) EnterErrorState("No Internet Connection");
                }
                catch (Exception ex)
                {
                    if (!_inErrorState) EnterErrorState("Error: " + ex.Message);
                }
                Thread.Sleep(PollDelay);
            }
        }

        private void EnterErrorState(string error, bool showDialog = false)
        {
            _inErrorState = true;
            _menuDisplayForm.Enabled = false;
            _tray.Icon = Resources.potatoERR;
            _tray.Text = "TaterNotify (" + error + ")";
            if (showDialog) MessageBox.Show(error, "TaterNotify ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ExitErrorState()
        {
            _inErrorState = false;
            _menuDisplayForm.Enabled = true;
            _tray.Icon = Resources.potato;
            _tray.Text = Resources.CustomApplicationContext_TaterNotify;
        }

        private void GetStandings()
        {
            List<TaterStandingsRow> latestStandings = new List<TaterStandingsRow>();

            const string url = @"http://pools.stats.com/homer/homer_draft.html";
            string fullHtml = WebClient.DownloadString(url);
            Match tab = Regex.Match(fullHtml, @"Total Tater Leaders.*?</table>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match row in Regex.Matches(tab.ToString(), 
            @"<tr\s+bgcolor.*?<font.*?>(.*?)<.*?<font.*?>(?:[^<]*\s)?(.*?)<.*?<font.*?>(\d+)\s+\((\d+).*?(\?id=.*?</tr>)", RegexOptions.IgnoreCase | RegexOptions.Singleline))
            {
                TaterStandingsRow newRow = new TaterStandingsRow(row.Groups[2].ToString().ToUpper(), int.Parse(row.Groups[3].ToString()));
                foreach (Match plyr in Regex.Matches(row.Groups[5].ToString(), @"\?id=(\d+)", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    newRow.Batters.Add(int.Parse(plyr.Groups[1].ToString()));
                }
                latestStandings.Add(newRow);
            }

            _standings = latestStandings;
        }

        private List<Tater> GetLatestTaters()
        {
            List<Tater> latestTaters = new List<Tater>();

            string url = @"http://hosted.stats.com/mlb/scoreboard.asp?day=" + _dateString;
            string fullHtml = WebClient.DownloadString(url);
            foreach (Match row in Regex.Matches(fullHtml, @"<tr\s+class=""shsGameHomeRuns"".*?</tr>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
            {
                foreach (Match m in Regex.Matches(row.ToString(), @"<a\s+.*?id=(\d+)[^>]*>([^<]*)<[^>]*>\s*(\d+)?", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    long playerId = long.Parse(m.Groups[1].ToString());
                    string batter = m.Groups[2].ToString();
                    decimal taters = m.Groups[3].Length > 0 ? decimal.Parse(m.Groups[3].ToString()) : 1;
                    TaterStandingsRow taterStandingsRow = _standings.FirstOrDefault(x => x.Batters.Contains(playerId));
                    string owner = taterStandingsRow != null ? taterStandingsRow.Owner : "";
                    latestTaters.Add(new Tater(playerId, batter, owner, taters));
                }
            }

            return latestTaters;
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            List<Tater> taterDiff = (List<Tater>)e.UserState;

            string message = string.Join(Environment.NewLine, taterDiff);
            _tray.ShowBalloonTip(int.MaxValue, "TATER!", message, ToolTipIcon.Info);
            if (Settings.Default.PlaySounds && 
                (!Settings.Default.SoundsOnlyForMyTeam || taterDiff.Any(x => x.Owner.Equals(Settings.Default.UserOwner))))
            {
                HomeRunSound.Play();
            }
        }

        private void DisposeWorker(object sender, EventArgs e)
        {
            WebClient.Dispose();
            HomeRunSound.Dispose();
        }

        private void OnThreadExit(object sender, EventArgs e)
        {
            _theWorker.Dispose();
            _tray.Visible = false;
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            _theWorker.Dispose();
            Application.Exit();
        }

        private void MenuSettings_Click(object sender, EventArgs e)
        {
            SettingsForm form = new SettingsForm(_standings.Select(x => x.Owner));
            form.ShowDialog();
            form.Dispose();
            Settings.Default.Save();
        }

        private void MenuDisplayForm_Click(object sender, EventArgs e)
        {
            if (_inErrorState) return;
            _tray.ShowBalloonTip(BalloonMillis, "TATERS for " + _dateString + ":", GetTaterSummary(), ToolTipIcon.Info);
        }

        private void Tray_DoubleClick(object sender, EventArgs e)
        {
            if (_inErrorState) return;
            _tray.ShowBalloonTip(BalloonMillis, "TATERS for " + _dateString + ":", GetTaterSummary(), ToolTipIcon.Info);
        }

        private string GetTaterSummary()
        {
            StringBuilder buf = new StringBuilder();
            foreach (TaterStandingsRow row in _standings.OrderByDescending(x => x.Taters + _taters.Where(y => y.Owner.Equals(x.Owner)).Sum(y => y.Taters)).ThenBy(x => x.Owner))
            {
                decimal today = _taters.Where(x => x.Owner.Equals(row.Owner)).Sum(x => x.Taters);
                buf.AppendLine((row.Taters + today) + " (" + today + ") " + row.Owner);
            }
            return buf.ToString();
        }

    }
}
