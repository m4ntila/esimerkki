using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WPFsmLiiga2021
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        pelaajaTiedot[] kotiPelaaja = new pelaajaTiedot[50];
        pelaajaTiedot[] vierasPelaaja = new pelaajaTiedot[50];
        joukkueTiedot[] joukkueKuvat = new joukkueTiedot[50];

        /// <summary>
        /// Kokoaa yhden pelaajan tiedot samaan arrayn indeksiin.
        /// </summary>
        struct pelaajaTiedot
        {
            public string nro;
            public string nimi;
            public int maaliLkm;
        }

        /// <summary>
        /// Kokoaa yhden joukkueen tiedot samaan arrayn indeksiin.
        /// </summary>
        struct joukkueTiedot
        {
            public string nimi;
            public string kuva;
        }

        int pelaajaInd = 0;
        int joukkueInd = 0;
        DateTime otteluPvm = DateTime.Now;
        int kotiMaalit = 0;
        int vierasMaalit = 0;

        /// <summary>
        /// Pääohjelma, jonka avulla saadaan osoitettua jääkiekko-ottelun tulostilanne.
        /// Ohjelman avulla voi valita ottelun päivämäärän erillisestä kalenterista,
        /// jonka jälkeen voi valita ottelun joukkueet, sekä maalin tullessa
        /// voidaan valita maalintekijä, jonka jälkeen ohjelma kertoo ottelutilanteen.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            KirjaaPelitiedot();
            XmlReader lukija = XmlReader.Create("SMliiga.xml");
            string joukkue = "";
            lukija.MoveToContent();

            while (lukija.Read())
            {
                if (lukija.NodeType == XmlNodeType.Element
                    && lukija.Name == "Joukkue")
                {
                    if (lukija.HasAttributes)
                    {
                        joukkue = lukija.GetAttribute("nimi");
                        joukkueKuvat[joukkueInd].nimi = joukkue;

                        lstKotijoukkue.Items.Add(joukkue);
                        lstVierasjoukkue.Items.Add(joukkue);
                    }
                }

                if (lukija.NodeType == XmlNodeType.Element
                   && lukija.Name == "Kuva")
                {
                    lukija.Read();
                    joukkueKuvat[joukkueInd].kuva = lukija.Value;
                    joukkueInd++;
                }

                if (lukija.NodeType == XmlNodeType.Element
                   && lukija.Name == "Nimi")
                {
                    lukija.Read();
                    kotiPelaaja[pelaajaInd].nimi = lukija.Value;
                    vierasPelaaja[pelaajaInd].nimi = lukija.Value;
                }

                if (lukija.NodeType == XmlNodeType.Element
                   && lukija.Name == "Pelaajanro")
                {
                    lukija.Read();
                    kotiPelaaja[pelaajaInd].nro = lukija.Value;
                    vierasPelaaja[pelaajaInd].nro = lukija.Value;
                    pelaajaInd++;
                }
            }
        }

        /// <summary>
        /// Kotimaali-napin toiminnot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnKirjaaKotiMaali_Click(object sender, RoutedEventArgs e)
        {
            if (lstKotipelaajat.SelectedIndex < 0)
            {
                MessageBox.Show("Maalintekijä tulee valita.");
                return;
            }

            if (lstKotijoukkue.SelectedIndex == lstVierasjoukkue.SelectedIndex)
            {
                MessageBox.Show("Joukkueet eivät voi olla samat. Valitse uusi joukkue.");
                return;
            }

            string tamaHetki = DateTime.Now.ToShortTimeString();
            kotiMaalit++;
            lblKotiMaalit.Content = kotiMaalit;

            int maalintekijanInd = lstKotipelaajat.SelectedIndex;
            string maalintekijanTiedot = kotiPelaaja[maalintekijanInd].nro + " " + kotiPelaaja[maalintekijanInd].nimi;
            lstKotiMaalit.Items.Add(tamaHetki + " " + maalintekijanTiedot);
            kotiPelaaja[maalintekijanInd].maaliLkm++;

            KirjaaPelitiedot();
            Onnittele(maalintekijanTiedot, kotiPelaaja[maalintekijanInd].maaliLkm);

        }

        /// <summary>
        /// Vierasmaali-napin toiminnot
        /// Sama, kuin kotimaali-nappi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnKirjaaVierasMaali_Click(object sender, RoutedEventArgs e)
        {
            if (lstVieraspelaajat.SelectedIndex < 0)
            {
                MessageBox.Show("Maalintekijä tulee valita.");
                return;
            }

            if (lstVierasjoukkue.SelectedIndex == lstKotijoukkue.SelectedIndex)
            {
                MessageBox.Show("Joukkueet eivät voi olla samat. Valitse uusi joukkue.");
                return;
            }

            string tamaHetki = DateTime.Now.ToShortTimeString();
            vierasMaalit++;
            lblVierasMaalit.Content = vierasMaalit;

            int maalintekijanInd = lstVieraspelaajat.SelectedIndex;
            string maalintekijanTiedot = vierasPelaaja[maalintekijanInd].nro + " " + vierasPelaaja[maalintekijanInd].nimi;
            lstVierasMaalit.Items.Add(tamaHetki + " " + maalintekijanTiedot);
            vierasPelaaja[maalintekijanInd].maaliLkm++;

            KirjaaPelitiedot();
            Onnittele(maalintekijanTiedot, vierasPelaaja[maalintekijanInd].maaliLkm);

        }

        /// <summary>
        /// Kotipelaajaa ei voi valita, jos vastustajaa ei ole valittu tai joukkueet ovat samat.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstKotipelaajat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstVierasjoukkue.SelectedIndex < 0)
            {
                MessageBox.Show("Valitse vastustaja.");
                return;
            }

            if (lstKotijoukkue.SelectedIndex == lstVierasjoukkue.SelectedIndex)
            {
                MessageBox.Show("Joukkueet eivät voi olla samat. Valitse uusi joukkue.");
                return;
            }
        }

        /// <summary>
        /// Vieraspelaajaa ei voi valita, jos vastustajaa ei ole valittu tai joukkueet ovat samat.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstVieraspelaajat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstKotijoukkue.SelectedIndex < 0)
            {
                MessageBox.Show("Valitse vastustaja.");
                return;
            }

            if (lstKotijoukkue.SelectedIndex == lstVierasjoukkue.SelectedIndex)
            {
                MessageBox.Show("Joukkueet eivät voi olla samat. Valitse uusi joukkue.");
                return;
            }
        }

        /// <summary>
        /// Kotijoukkue-listasta valinta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstKotijoukkue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (kotiMaalit > 0 || vierasMaalit > 0)
            {
                if (MessageBox.Show("Haluatko varmasti lopettaa ottelun?", "Uusi ottelu?", MessageBoxButtons.YesNo)
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    UusiOttelu();
                    tyhjennaPelaajaArray();
                    lstVierasjoukkue.SelectedIndex = default;
                    lstVieraspelaajat.Items.Clear();
                }else
                {
                    return;
                }    
            }

            lstKotipelaajat.Items.Clear();
            string haettavaJoukkue = lstKotijoukkue.SelectedItem.ToString();

            lblKotijoukkue.Content = haettavaJoukkue;
            lblKotiMaalit.Content = kotiMaalit;

            int kuvaInd = lstKotijoukkue.SelectedIndex;
            string kuvaPolkuJaNimi = @"C:\mytemp\ohj2\WPFsmLiiga2021\WPFsmLiiga2021\kuvat\" + joukkueKuvat[kuvaInd].kuva;
            ImageSource imageSource = new BitmapImage(new Uri(kuvaPolkuJaNimi));
            imgKoti.Source = imageSource;

            KirjaaPelitiedot();

            XmlReader lukija = XmlReader.Create(@"C:\mytemp\ohj2\WPFsmLiiga2021\WPFsmLiiga2021\SMliiga.xml");
            string joukkue = "";
            int ind = 0;
            lukija.MoveToContent();

            while (lukija.Read())
            {
                //luetaan joukkueet
                if (lukija.NodeType == XmlNodeType.Element
                    && lukija.Name == "Joukkue")
                {
                    if (lukija.HasAttributes)
                    {
                        joukkue = lukija.GetAttribute("nimi");

                        if (joukkue == haettavaJoukkue)
                        {
                            while (lukija.Read())
                            {
                                //silmukka lopetetaan, kun joukkue vaihtuu
                                if (lukija.NodeType == XmlNodeType.Element
                                    && lukija.Name == "Joukkue")
                                {
                                    break;
                                }

                                //luetaan pelaajat
                                if (lukija.NodeType == XmlNodeType.Element
                                    && lukija.Name == "Nimi")
                                {
                                    lukija.Read();
                                    kotiPelaaja[ind].nimi = lukija.Value;
                                }

                                if (lukija.NodeType == XmlNodeType.Element
                                    && lukija.Name == "Pelaajanro")
                                {
                                    lukija.Read();
                                    kotiPelaaja[ind].nro = lukija.Value;
                                    lstKotipelaajat.Items.Add(lukija.Value + " " + kotiPelaaja[ind].nimi);
                                    ind++;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Vierasjoukkue-listasta valinta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstVierasjoukkue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (kotiMaalit > 0 || vierasMaalit > 0)
            {
                if (MessageBox.Show("Haluatko varmasti lopettaa ottelun?", "Uusi ottelu?", MessageBoxButtons.YesNo)
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    UusiOttelu();
                    tyhjennaPelaajaArray();
                    lstKotijoukkue.SelectedIndex = default;
                    lstKotipelaajat.Items.Clear();
                }
                else
                {
                    return;
                }
            }

            lstVieraspelaajat.Items.Clear();
            string haettavaJoukkue = lstVierasjoukkue.SelectedItem.ToString();

            lblVierasjoukkue.Content = haettavaJoukkue;
            lblVierasMaalit.Content = vierasMaalit;

            int kuvaInd = lstVierasjoukkue.SelectedIndex;
            string kuvaPolkuJaNimi = @"C:\mytemp\ohj2\WPFsmLiiga2021\WPFsmLiiga2021\kuvat\" + joukkueKuvat[kuvaInd].kuva;
            ImageSource imageSource = new BitmapImage(new Uri(kuvaPolkuJaNimi));
            imgVieras.Source = imageSource;

            KirjaaPelitiedot(); 

            XmlReader lukija = XmlReader.Create(@"C:\mytemp\ohj2\WPFsmLiiga2021\WPFsmLiiga2021\SMliiga.xml");
            string joukkue = "";
            int ind = 0;
            lukija.MoveToContent();

            while (lukija.Read())
            {
                //luetaan joukkueet
                if (lukija.NodeType == XmlNodeType.Element
                    && lukija.Name == "Joukkue")
                {
                    if (lukija.HasAttributes)
                    {
                        joukkue = lukija.GetAttribute("nimi");

                        if (joukkue == haettavaJoukkue)
                        {
                            while (lukija.Read())
                            {
                                //silmukka lopetetaan, kun joukkue vaihtuu
                                if (lukija.NodeType == XmlNodeType.Element
                                    && lukija.Name == "Joukkue")
                                {
                                    break;
                                }

                                //luetaan pelaajat
                                if (lukija.NodeType == XmlNodeType.Element
                                && lukija.Name == "Nimi")
                                {
                                    lukija.Read();
                                    vierasPelaaja[ind].nimi = lukija.Value;
                                }

                                if (lukija.NodeType == XmlNodeType.Element
                                    && lukija.Name == "Pelaajanro")
                                {
                                    lukija.Read();
                                    vierasPelaaja[ind].nro = lukija.Value;
                                    lstVieraspelaajat.Items.Add(lukija.Value + " " + vierasPelaaja[ind].nimi);
                                    ind++;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tyhjentää pelaajalistat vaihdettaessa joukkuevalintaa.
        /// </summary>
        void tyhjennaPelaajaArray()
        {
            for (int i = 0; i < kotiPelaaja.Length; i++)
            {
                kotiPelaaja[i].nimi = "";
                kotiPelaaja[i].nro = "";
                kotiPelaaja[i].maaliLkm = 0;
            }

            for (int i = 0; i < vierasPelaaja.Length; i++)
            {
                vierasPelaaja[i].nimi = "";
                vierasPelaaja[i].nro = "";
                vierasPelaaja[i].maaliLkm = 0;
            }
            return;
        }

        /// <summary>
        /// Aliohjelma, joka kirjaa ottelun tilanteen kyseisellä hetkellä
        /// ja näyttää sen ikkunan alalaidassa.
        /// </summary>
        void KirjaaPelitiedot()
        {
            string kotiJoukkue = "";
            string vierasJoukkue = "";

            if (lstKotijoukkue.SelectedIndex >= 0 && lstVierasjoukkue.SelectedIndex >= 0)
            {
                kotiJoukkue = lstKotijoukkue.SelectedItem.ToString();
                vierasJoukkue = lstVierasjoukkue.SelectedItem.ToString();
            }
            else
            {
                kotiJoukkue = "kotijoukkue";
                vierasJoukkue = "vierasjoukkue";
            }

            lblPeliTiedot.Content = otteluPvm.ToShortDateString()
                                    + " "
                                    + kotiJoukkue
                                    + " - "
                                    + vierasJoukkue;

            lblTilanne.Content = kotiMaalit
                                 + " - "
                                 + vierasMaalit;
        }

        /// <summary>
        /// Käyttäjä painaa kesken ottelun uutta joukkuetta ja ohjelma kysyy,
        /// aloitetaanko uusi peli. Painaessa "Yes" uusi joukkue tulee valituksi
        /// ja tilanne nollaantuu. Jos käyttäjä valitsee "No", vanha peli jatkuu.
        /// </summary>
        void UusiOttelu()
        {
                LisaaVanhaPeli();

                lstVieraspelaajat.Items.Clear();
                lstKotipelaajat.Items.Clear();

                imgKoti.Source = default;
                imgVieras.Source = default;

                lstKotiMaalit.Items.Clear();
                lstVierasMaalit.Items.Clear();

                lblKotiMaalit.Content = "0";
                lblVierasMaalit.Content = "0";

                lblKotijoukkue.Content = "";
                lblVierasjoukkue.Content = "";

                kotiMaalit = 0;
                vierasMaalit = 0;

                KirjaaPelitiedot();
        }

        /// <summary>
        /// Ohjelma kertoo, mitä tapahtuu, kun vaihdetaan kalenterista päivämäärää.
        /// Ohjelma myös kirjaa uuden ottelupäivän pelitietoihin.
        /// </summary>
        private void Calendar_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            otteluPvm = (DateTime)calOtteluPaiva.SelectedDate;
            KirjaaPelitiedot();
        }

        /// <summary>
        /// Aliohjelma, joka onnittelee, jos pelaaja on saanut kolme
        /// tai enemmän maaleja. Näyttää viestin MessageBoxissa.
        /// </summary>
        /// <param name="pelaajaTiedot"></param>
        /// <param name="maalit"></param>
        void Onnittele(string pelaajaTiedot, int maalit)
        {
            if (maalit > 3)
            {
                MessageBox.Show("ONNEA! "
                                + pelaajaTiedot
                                + " on tehnyt ottelussa "
                                + maalit
                                + " maalia!");
            }else if (maalit == 3)
            {
                MessageBox.Show("Onnea hattutempusta "
                                + pelaajaTiedot + "!");
                return;
            }
        }

        /// <summary>
        /// Lisää vanhan pelin tilanteen listaan, kun
        /// käyttäjä päättää valita uuden pelin.
        /// </summary>
        void LisaaVanhaPeli()
        {
            lstVanhatPelit.Items.Add(lblPeliTiedot.Content.ToString()
                                     +
                                     "   Tulos: "
                                     +
                                     lblTilanne.Content.ToString());
        }

        /// <summary>
        /// Siirrytään TabControllin edelliseen välilehteen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdellinenTab_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = tcLiiga.SelectedIndex - 1;
            if (newIndex < 0)
                newIndex = tcLiiga.Items.Count - 1;
            tcLiiga.SelectedIndex = newIndex;
        }

        /// <summary>
        /// Siirrytään TabControllin seuraavaan välilehteen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSeuraavaTab_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = tcLiiga.SelectedIndex + 1;
            if (newIndex >= tcLiiga.Items.Count)
                newIndex = 0;
            tcLiiga.SelectedIndex = newIndex;
        }

        /// <summary>
        /// Sulkee ohjelmaikkunan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSulje_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
