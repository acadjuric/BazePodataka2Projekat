using Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UI.Helpers;

namespace UI.ViewModel
{
    public class PitanjeViewModel:BindableBase
    {

        
        public static ObservableCollection<Pitanje> SvaPitanja { get; set; } = new ObservableCollection<Pitanje>();
        public static ObservableCollection<string> IDSvakogPitanja { get; set; } = new ObservableCollection<string>();
        public static List<Odgovor> IzabraniOdgovori { get; set; }
        public static List<Test> IzabraniTestovi { get; set; }
        public static List<Odgovor> IzabraniOdgovoriDodavanje { get; set; }
        public static List<Test> IzabraniTestoviDodavanje { get; set; }

        public MyICommand Kreiraj { get; set; }
        public MyICommand Izmeni { get; set; }
        public MyICommand Izbrisi { get; set; }

        public MyICommand DodajOdgovore { get; set; }
        public MyICommand IzmeniOdgovore { get; set; }
        public MyICommand DodajTestove { get; set; }
        public MyICommand IzmeniTestove { get; set; }


        public Servis.Servisi.PitanjeServis Baza = new Servis.Servisi.PitanjeServis();
        public Servis.Servisi.TestServis BazaTestova = new Servis.Servisi.TestServis();
        public Servis.Servisi.OdgovorServis BazaOdgovora = new Servis.Servisi.OdgovorServis();
        public Servis.Servisi.PitanjeOdgovorServis BazaPitanjaIOdgovora = new Servis.Servisi.PitanjeOdgovorServis();
        public Servis.Servisi.TestPitanjeServis BazaTestovaIPitanja = new Servis.Servisi.TestPitanjeServis();

        #region Properties
        private string tbTekst;

        public string TBTekst
        {
            get { return tbTekst; }
            set { tbTekst = value; OnPropertyChanged("TBTekst"); }
        }

        private string tbNivo;

        public string TBNivo
        {
            get { return tbNivo; }
            set { tbNivo = value; OnPropertyChanged("TBNivo"); }
        }

        private string tbBodovi;

        public string TBBodovi
        {
            get { return tbBodovi; }
            set { tbBodovi = value; OnPropertyChanged("TBBodovi"); }
        }


        private string tbTekstIzmena;

        public string TBTekstIzmena
        {
            get { return tbTekstIzmena; }
            set { tbTekstIzmena = value; OnPropertyChanged("TBTekstIzmena"); }
        }

        private string tbNivoIzmena;

        public string TBNivoIzmena
        {
            get { return tbNivoIzmena; }
            set { tbNivoIzmena = value; OnPropertyChanged("TBNivoIzmena"); }
        }

        private string tbBodoviIzmena;

        public string TBBodoviIzmena
        {
            get { return tbBodoviIzmena; }
            set { tbBodoviIzmena = value; OnPropertyChanged("TBBodoviIzmena"); }
        }

        private string idIzmena;

        public string IdIzmena
        {
            get { return idIzmena; }
            set { idIzmena = value; OnPropertyChanged("IdIzmena");
                IzmeniTestove.RaiseCanExecuteChanged() ; Izmeni.RaiseCanExecuteChanged(); IzmeniOdgovore.RaiseCanExecuteChanged();
                if(IdIzmena != null)
                {
                    DobaviPitanjeISveNjegoveAtribute(int.Parse(IdIzmena));
                }
            }
        }

        private string idBrisanje;

        public string IdBrisanje
        {
            get { return idBrisanje; }
            set { idBrisanje = value; OnPropertyChanged("IdBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }

        #endregion

        int poslednjiIdPitanjaZaKojiJeKliknutoIzmeniTestove = -1;
        int poslednjiIdPitanjaZaKojiJeKliknutoIzmeniOdgovore = -1;

        public PitanjeViewModel ()
        {
            IzabraniTestovi = new List<Test>();
            IzabraniOdgovori = new List<Odgovor>();
            IzabraniOdgovoriDodavanje = new List<Odgovor>();
            IzabraniTestoviDodavanje = new List<Test>();

            Kreiraj = new MyICommand(OnKreiraj);
            Izmeni = new MyICommand(OnIzmeni,CanEdit);
            Izbrisi = new MyICommand(OnIzbrisi, CanDelete);
            DodajOdgovore = new MyICommand(OnDodajOdgovore);
            IzmeniOdgovore = new MyICommand(OnIzmeniOdgovore, CanEdit);
            DodajTestove = new MyICommand(OnDodajTestove);
            IzmeniTestove = new MyICommand(OnIzmeniTestove,CanEdit);

            poslednjiIdPitanjaZaKojiJeKliknutoIzmeniTestove = -1;
            poslednjiIdPitanjaZaKojiJeKliknutoIzmeniOdgovore = -1;

            DobaviPodatkeIzBaze();
            
        }

        public void OnDodajTestove()
        {
            Views.PitanjeTestWindow prozor = new Views.PitanjeTestWindow(false);
            prozor.ShowDialog();
        }

        public void OnIzmeniTestove()
        {
            int id = int.Parse(IdIzmena);

            if (poslednjiIdPitanjaZaKojiJeKliknutoIzmeniTestove != id)
            {
                IzabraniTestovi.Clear();

                Pitanje p = Baza.VratiPitanje(id);
                if (p == null) return;

                List<Sastoji> SviTestoviZaPitanje = BazaTestovaIPitanja.VratiTestoveZaPitanje(id);
                if(SviTestoviZaPitanje != null)
                {
                    foreach(var sastoji in SviTestoviZaPitanje)
                    {
                        Test t = null;
                        t = BazaTestova.VratiTest(sastoji.TestIdTesta);
                        if (t != null)
                            IzabraniTestovi.Add(t);
                    }
                }
            }


            poslednjiIdPitanjaZaKojiJeKliknutoIzmeniTestove = id;
            Views.PitanjeTestWindow prozor = new Views.PitanjeTestWindow(true);
            prozor.ShowDialog();
        }

        public void OnDodajOdgovore()
        {
            Views.PitanjeOdgovorWindow prozor = new Views.PitanjeOdgovorWindow(false);
            prozor.ShowDialog();
        }

        public void OnIzmeniOdgovore()
        {
            int id = int.Parse(IdIzmena);

            if (poslednjiIdPitanjaZaKojiJeKliknutoIzmeniOdgovore != id)
            {
                IzabraniOdgovori.Clear();

                Pitanje p = Baza.VratiPitanje(id);
                if (p == null) return;

                List<Poseduje> SviOdgovoriZaPitanje = BazaPitanjaIOdgovora.VratiOdgovoreZaPitanje(id);
                if (SviOdgovoriZaPitanje != null)
                {
                    foreach(var poseduje in SviOdgovoriZaPitanje)
                    {
                        Odgovor o = null;
                        o = BazaOdgovora.VratiOdgovor(poseduje.OdgovorIdOdgovora);
                        if (o != null)
                            IzabraniOdgovori.Add(o);
                    }
                }
            }
            poslednjiIdPitanjaZaKojiJeKliknutoIzmeniOdgovore = id;
            Views.PitanjeOdgovorWindow prozor = new Views.PitanjeOdgovorWindow(true);
            prozor.ShowDialog();
        }

        private void DobaviPitanjeISveNjegoveAtribute(int id)
        {
            

            Pitanje p = Baza.VratiPitanje(id);
            if (p == null) return;

            IzabraniOdgovori.Clear();
            IzabraniTestovi.Clear();

            TBTekstIzmena = p.Tekst;
            TBBodoviIzmena = p.Bodovi.ToString();
            TBNivoIzmena = p.Nivo.ToString();


            List<Poseduje> SviOdgovoriZaPitanje = BazaPitanjaIOdgovora.VratiOdgovoreZaPitanje(id);
            if (SviOdgovoriZaPitanje != null)
            {
                foreach (var poseduje in SviOdgovoriZaPitanje)
                {
                    Odgovor o = null;
                    o = BazaOdgovora.VratiOdgovor(poseduje.OdgovorIdOdgovora);
                    if (o != null)
                        IzabraniOdgovori.Add(o);
                }
            }

            List<Sastoji> SviTestoviZaPitanje = BazaTestovaIPitanja.VratiTestoveZaPitanje(id);
            if (SviTestoviZaPitanje != null)
            {
                foreach (var sastoji in SviTestoviZaPitanje)
                {
                    Test t = null;
                    t = BazaTestova.VratiTest(sastoji.TestIdTesta);
                    if (t != null)
                        IzabraniTestovi.Add(t);
                }
            }

        }

        private bool CanEdit() { return IdIzmena != null; }
        private bool CanDelete() { return IdBrisanje != null; }

        public void OnKreiraj()
        {
           
            try
            {
                if (!ValidacijaDodavanje()) return;

                Baza.DodajPitanje(TBTekst, int.Parse(TBNivo), int.Parse(TBBodovi),IzabraniOdgovoriDodavanje, IzabraniTestoviDodavanje);
                DobaviPodatkeIzBaze();

                TBTekst = string.Empty;
                TBBodovi = string.Empty;
                TBNivo = string.Empty;
                IzabraniTestoviDodavanje.Clear();
                IzabraniOdgovoriDodavanje.Clear();
            }
            catch
            {
                MessageBox.Show("Nesto nije u redu kod dodavanja", "Greska", MessageBoxButton.OK);
            }

        }

        public void OnIzmeni()
        {
            try
            {
                if (!ValidacijaIzmena()) return;

                int id = int.Parse(IdIzmena);
                int nivo = int.Parse(TBNivoIzmena);
                int bodovi = int.Parse(TBBodoviIzmena);
                

                Baza.IzmeniPitanje(id, TBTekstIzmena, nivo, bodovi,IzabraniOdgovori,IzabraniTestovi);
                DobaviPodatkeIzBaze();

                TBBodoviIzmena = string.Empty;
                TBNivoIzmena = string.Empty;
                TBTekstIzmena = string.Empty;
                IdIzmena = null;
                IzabraniOdgovori.Clear();
                IzabraniTestovi.Clear();
                poslednjiIdPitanjaZaKojiJeKliknutoIzmeniTestove = -1;
                poslednjiIdPitanjaZaKojiJeKliknutoIzmeniOdgovore = -1;
            }
            catch
            {
                MessageBox.Show("Izaberite ID za izmenu!", "Greska", MessageBoxButton.OK);
            }
        }

        public void OnIzbrisi()
        {
            try
            {
                
                int id = int.Parse(IdBrisanje);
                
                Baza.ObrisiPitanje(id);
                DobaviPodatkeIzBaze();
            }
            catch(Exception e)
            {
                MessageBox.Show("Izaberite ID za brisanje!", "Greska", MessageBoxButton.OK);
            }
        }


        public void DobaviPodatkeIzBaze()
        {
            SvaPitanja.Clear();
            IDSvakogPitanja.Clear();

            List<Pitanje> Pitanja = Baza.UcitajSvaPitanja();
            foreach (var p in Pitanja)
            {
                SvaPitanja.Add(p);
                IDSvakogPitanja.Add(p.IdPitanja.ToString());
            }

            OnPropertyChanged("IDSvakogPitanja");
            OnPropertyChanged("SvaPitanja");

        }

        public bool ValidacijaDodavanje()
        {
            if (string.IsNullOrEmpty(TBTekst) || string.IsNullOrWhiteSpace(TBTekst))
            {
                MessageBox.Show("Polje za Tekst ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBBodovi) || string.IsNullOrWhiteSpace(TBBodovi))
            {
                MessageBox.Show("Polje za Bodove ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBNivo) || string.IsNullOrWhiteSpace(TBNivo))
            {
                MessageBox.Show("Polje za Nivo ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBTekst.Length < 10)
            {
                MessageBox.Show("Tekst mora imati minimum 10 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                int bodovi = int.Parse(TBBodovi);
            }
            catch
            {
                MessageBox.Show("Niste dobro uneli bodove!", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                int nivo = int.Parse(TBNivo);
            }
            catch
            {
                MessageBox.Show("Niste dobro uneli nivo pitanja!", "Greska", MessageBoxButton.OK);
                return false;
            }

            if (IzabraniOdgovoriDodavanje.Count == 0)
            {
                MessageBox.Show("Morate izabrati BAR jedan Odgovor za ovo Pitanje", "Greska", MessageBoxButton.OK);
                return false;
            }

            return true;

        }

        public bool ValidacijaIzmena()
        {
            if (string.IsNullOrEmpty(TBTekstIzmena) || string.IsNullOrWhiteSpace(TBTekstIzmena))
            {
                MessageBox.Show("Polje za Novi Tekst ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBBodoviIzmena) || string.IsNullOrWhiteSpace(TBBodoviIzmena))
            {
                MessageBox.Show("Polje za Nove Bodove ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBNivoIzmena) || string.IsNullOrWhiteSpace(TBNivoIzmena))
            {
                MessageBox.Show("Polje za Novi Nivo  ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBTekstIzmena.Length < 10)
            {
                MessageBox.Show("Novi Tekst mora imati minimum 10 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                int bodovi = int.Parse(TBBodoviIzmena);
            }
            catch
            {
                MessageBox.Show("Niste dobro uneli nove bodove!", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                int nivo = int.Parse(TBNivoIzmena);
            }
            catch
            {
                MessageBox.Show("Niste dobro uneli novi nivo pitanja!", "Greska", MessageBoxButton.OK);
                return false;
            }

            if (IzabraniOdgovori.Count == 0)
            {
                MessageBox.Show("Morate izabrati BAR jedan Odgovor za ovo Pitanje", "Greska", MessageBoxButton.OK);
                return false;
            }

            return true;
        }

    }
}
