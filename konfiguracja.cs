using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace LKWokanda
{
    public partial class konfiguracja : Form
    {
        public konfiguracja()
        {
            InitializeComponent();
        }

        private void konfiguracja_Load(object sender, EventArgs e)
        {
            Odczyt_konfiguracji();
        }

        private void Odczyt_konfiguracji()
        {
            textConStrCywilny.Text = Properties.Settings.Default.cywilny;
            textConStrCywilnyII.Text = Properties.Settings.Default.cywilnyII;
            textConStrKarny.Text = Properties.Settings.Default.Karny;
            textConStrKarnyII.Text = Properties.Settings.Default.karnyII;
            textConStrRodzinny.Text = Properties.Settings.Default.Rodzinny;
            textConStrRodzinnyII.Text = Properties.Settings.Default.rodzinnyII;
            textConStrPracy.Text = Properties.Settings.Default.Pracy;
            textConStrPracyII.Text = Properties.Settings.Default.pracyII;
            textConStrPenitencjarny.Text = Properties.Settings.Default.penitencjarny;

            plikXML.Text = Properties.Settings.Default.plikXML;
            checkdoHTML.Checked = Properties.Settings.Default.do_html;
 
            importCywilny.Checked = Properties.Settings.Default.czycywilny;
            importCywilnyII.Checked = Properties.Settings.Default.czy_cywilnyII;
            importKarny.Checked = Properties.Settings.Default.czykarny;
            importKarnyII.Checked = Properties.Settings.Default.czy_karnyII;
            importRodzinny.Checked = Properties.Settings.Default.czyrodzinny;
            importPracy.Checked = Properties.Settings.Default.czypracy;
            importPracyII.Checked = Properties.Settings.Default.czy_pracyII;
            importPenitencjarny.Checked = Properties.Settings.Default.czy_penitencjarny;

            check_ftp.Checked = Properties.Settings.Default.send_ftp;
            checkLog.Checked = Properties.Settings.Default.debug;

            ftp_serwer.Text = Properties.Settings.Default.ftp;
            ftp_katalog.Text = Properties.Settings.Default.ftp_katalog;
            ftp_uzytkownik.Text = Properties.Settings.Default.user;
            ftp_haslo.Text = Properties.Settings.Default.password;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.ConnectionStrings.ConnectionStrings["LKWokanda.Properties.Settings.cywilny"].ConnectionString = textConStrCywilny.Text.Trim();
            conf.ConnectionStrings.ConnectionStrings["LKWokanda.Properties.Settings.cywilnyII"].ConnectionString = textConStrCywilnyII.Text.Trim();
            conf.ConnectionStrings.ConnectionStrings["LKWokanda.Properties.Settings.karny"].ConnectionString = textConStrKarny.Text.Trim();
            conf.ConnectionStrings.ConnectionStrings["LKWokanda.Properties.Settings.karnyII"].ConnectionString = textConStrKarnyII.Text.Trim();
            conf.ConnectionStrings.ConnectionStrings["LKWokanda.Properties.Settings.rodzinny"].ConnectionString = textConStrRodzinny.Text.Trim();
            conf.ConnectionStrings.ConnectionStrings["LKWokanda.Properties.Settings.rodzinnyII"].ConnectionString = textConStrRodzinnyII.Text.Trim();
            conf.ConnectionStrings.ConnectionStrings["LKWokanda.Properties.Settings.pracy"].ConnectionString = textConStrPracy.Text.Trim();
            conf.ConnectionStrings.ConnectionStrings["LKWokanda.Properties.Settings.pracyII"].ConnectionString = textConStrPracyII.Text.Trim();
            conf.ConnectionStrings.ConnectionStrings["LKWokanda.Properties.Settings.penitencjarny"].ConnectionString = textConStrPenitencjarny.Text.Trim();
            conf.Save(ConfigurationSaveMode.Modified);

            Properties.Settings.Default.czycywilny = importCywilny.Checked;

            Properties.Settings.Default.czycywilny = importCywilny.Checked;
            Properties.Settings.Default.czy_cywilnyII = importCywilnyII.Checked;
            Properties.Settings.Default.czykarny = importKarny.Checked;
            Properties.Settings.Default.czy_karnyII = importKarnyII.Checked;
            Properties.Settings.Default.czyrodzinny = importRodzinny.Checked;
            Properties.Settings.Default.czypracy = importPracy.Checked;
            Properties.Settings.Default.czy_pracyII = importPracyII.Checked;
            Properties.Settings.Default.czy_penitencjarny = importPenitencjarny.Checked;

            Properties.Settings.Default.ftp = ftp_serwer.Text;
            Properties.Settings.Default.ftp_katalog = ftp_katalog.Text;
            Properties.Settings.Default.user = ftp_uzytkownik.Text;
            Properties.Settings.Default.password = ftp_haslo.Text;


            Properties.Settings.Default.send_ftp = check_ftp.Checked;
            Properties.Settings.Default.do_html = checkdoHTML.Checked;

            Properties.Settings.Default.debug = checkLog.Checked;
            Properties.Settings.Default.Save();

            } catch (Exception e222)
            {
                Console.WriteLine("Wystąpił masakryczny błąd! " + e222.Message.ToString());
                Application.Exit();
            }
            ConfigurationManager.RefreshSection("ConnectionStrings");
            ConfigurationManager.RefreshSection("appSettings");

          Odczyt_konfiguracji();
          Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

