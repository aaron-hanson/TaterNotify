using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TaterNotify.Properties;

namespace TaterNotify
{
    public partial class SettingsForm : Form
    {
        public SettingsForm(IEnumerable<object> owners)
        {
            InitializeComponent();

            Icon = Resources.potato;

            cboUserOwner.Items.Add("(none)");
            cboUserOwner.Items.AddRange(owners.ToArray());
        }

        private void SettingsForm_Load(object sender, System.EventArgs e)
        {
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            Location = new Point(workingArea.Width - Width, workingArea.Height - Height);

            cboUserOwner.SelectedItem = Settings.Default.UserOwner;
            cbPlaySounds.Checked = Settings.Default.PlaySounds;
            cbSoundsOnlyForTeam.Checked = Settings.Default.SoundsOnlyForMyTeam;
            cbSoundsOnlyForTeam.Enabled = cbPlaySounds.Checked;
        }

        private void cboUserOwner_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Settings.Default.UserOwner = cboUserOwner.SelectedItem.ToString();
        }

        private void cbPlaySounds_CheckedChanged(object sender, System.EventArgs e)
        {
            Settings.Default.PlaySounds = cbPlaySounds.Checked;
            cbSoundsOnlyForTeam.Enabled = cbPlaySounds.Checked;
        }

        private void cbSoundsOnlyForTeam_CheckedChanged(object sender, System.EventArgs e)
        {
            Settings.Default.SoundsOnlyForMyTeam = cbSoundsOnlyForTeam.Checked;
        }

    }
}
