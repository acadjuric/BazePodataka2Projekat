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
    public class ObavestenjaViewModel:BindableBase
    {

        
        public static ObservableCollection<Obavestenje> SvaObavestenja { get; set; } = new ObservableCollection<Obavestenje>();
        public static ObservableCollection<string> IDSvakogObavestenja { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IdSvihKorisnika { get; set; } = new ObservableCollection<string>();
        public MyICommand Kreiraj { get; set; }
        public MyICommand Izmeni { get; set; }
        public MyICommand Izbrisi { get; set; }

        Servis.Servisi.ObavestenjeServis Baza = new Servis.Servisi.ObavestenjeServis();
        Servis.Servisi.NastavnikServis BazaNastavnika = new Servis.Servisi.NastavnikServis();
        Servis.Servisi.UcenikServis BazaUcenika = new Servis.Servisi.UcenikServis();


        #region Properties
        private string tbNaslov;

        public string TBNaslov
        {
            get { return tbNaslov; }
            set { tbNaslov = value; OnPropertyChanged("TBNaslov"); }
        }

        private DateTime? datumSlanja;

        public DateTime? DatumSlanja
        {
            get { return datumSlanja; }
            set { datumSlanja = value; OnPropertyChanged("DatumSlanja"); }
        }

        private string tbTekst;

        public string TBTekst
        {
            get { return tbTekst; }
            set { tbTekst = value; OnPropertyChanged("TBTekst"); }
        }


        private string tbNaslovIzmena;

        public string TBNaslovIzmena
        {
            get { return tbNaslovIzmena; }
            set { tbNaslovIzmena = value; OnPropertyChanged("TBNaslovIzmena"); }
        }

        private DateTime? datumSlanjaIzmena;

        public DateTime? DatumSlanjaIzmena
        {
            get { return datumSlanjaIzmena; }
            set { datumSlanjaIzmena = value; OnPropertyChanged("DatumSlanjaIzmena"); }
        }

        private string tbTekstIzmena;

        public string TBTekstIzmena
        {
            get { return tbTekstIzmena; }
            set { tbTekstIzmena = value; OnPropertyChanged("TBTekstIzmena"); }
        }

        private string idIzmena;

        public string IdIzmena
        {
            get { return idIzmena; }
            set { idIzmena = value; OnPropertyChanged("IdIzmena"); Izmeni.RaiseCanExecuteChanged();
                if(IdIzmena != null)
                {
                    Obavestenje o = null;
                    o = Baza.VratiObavestenje(int.Parse(IdIzmena));
                    if(o != null)
                    {
                        TBNaslovIzmena = o.Naslov;
                        TBTekstIzmena = o.Tekst;
                        DatumSlanjaIzmena = o.DatumSlanja;
                        IzabraniKorisnikIzmena = o.KorisnikIdKorisnika.ToString();
                    }
                }
            }
        }

        private string idBrisanje;

        public string IdBrisanje
        {
            get { return idBrisanje; }
            set { idBrisanje = value; OnPropertyChanged("IdBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }


        private string izabraniKorisnik;

        public string IzabraniKorisnik
        {
            get { return izabraniKorisnik; }
            set { izabraniKorisnik = value; OnPropertyChanged("IzabraniKorisnik"); }
        }


        private string izabraniKorisnikIzmena;

        public string IzabraniKorisnikIzmena
        {
            get { return izabraniKorisnikIzmena; }
            set { izabraniKorisnikIzmena = value; OnPropertyChanged("IzabraniKorisnikIzmena"); }
        }



        #endregion

        public ObavestenjaViewModel()
        {
            Kreiraj = new MyICommand(OnKreiraj);
            Izmeni = new MyICommand(OnIzmeni, CanEdit);
            Izbrisi = new MyICommand(OnIzbrisi, CanDelete);

            DobaviPodatkeIzBaze();

        }

        private bool CanEdit() { return IdIzmena != null; }
        private bool CanDelete() { return IdBrisanje != null; }

        private void OnKreiraj()
        {
          
            try
            {
                if (!ValidacijaDodavanje()) return;

                Baza.DodajObavestenje(TBNaslov, TBTekst, (DateTime)DatumSlanja, int.Parse(IzabraniKorisnik));
                DobaviPodatkeIzBaze();
                TBNaslov = string.Empty;
                TBTekst = string.Empty;
                DatumSlanja = null;
            }
            catch(Exception e)
            {

            }

        }

        private void OnIzmeni()
        {
            try
            {
                if (!ValidacijaIzmena()) return;

                int id = int.Parse(IdIzmena);

                Baza.IzmeniObavestenje(id, TBNaslovIzmena, TBTekstIzmena, (DateTime)DatumSlanjaIzmena,int.Parse(IzabraniKorisnikIzmena));
                DobaviPodatkeIzBaze();

                TBNaslovIzmena = string.Empty;
                DatumSlanjaIzmena = null;
                TBTekstIzmena = string.Empty;
                IdIzmena = null;
            }
            catch
            {
                MessageBox.Show("Izaberite ID za izmenu!", "Greska", MessageBoxButton.OK);
            }
        }

        private void OnIzbrisi()
        {

            try
            {

                int id = int.Parse(IdBrisanje);
                Baza.ObrisiObavestenje(id);
                DobaviPodatkeIzBaze();

            }
            catch
            {
                MessageBox.Show("Izaberite ID za brisanje!", "Greska", MessageBoxButton.OK);
            }

        }



        public void DobaviPodatkeIzBaze()
        {
            SvaObavestenja.Clear();
            IDSvakogObavestenja.Clear();
            IdSvihKorisnika.Clear();

            List<Obavestenje> Obavestenja = Baza.UcitajSvaObavestenja();
            foreach (var p in Obavestenja)
            {
                SvaObavestenja.Add(p);
                IDSvakogObavestenja.Add(p.IdObavestenja.ToString());
            }

            BazaNastavnika.UcitajSveNastavnike().ForEach(x => IdSvihKorisnika.Add(x.IdKorisnika.ToString()));
            BazaUcenika.UcitajSveUcenike().ForEach(x => IdSvihKorisnika.Add(x.IdKorisnika.ToString()));

            OnPropertyChanged("IDSvakogObavestenja");
            OnPropertyChanged("SvaObavestenja");
            OnPropertyChanged("IdSvihKorisnika");

        }


        public bool ValidacijaDodavanje()
        {
            if (string.IsNullOrEmpty(TBNaslov) || string.IsNullOrWhiteSpace(TBNaslov))
            {
                MessageBox.Show("Polje za Naslov ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBTekst) || string.IsNullOrWhiteSpace(TBTekst))
            {
                MessageBox.Show("Polje za Tekst ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if(DatumSlanja == null)
            {
                MessageBox.Show("Polje za DATUM SLANJA ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (DatumSlanja.Value == DateTime.MinValue)
            {
                MessageBox.Show("Polje za DATUM SLANJA nije validno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(IzabraniKorisnik) || string.IsNullOrWhiteSpace(IzabraniKorisnik))
            {
                MessageBox.Show("Polje za Korisnik ID ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBNaslov.Length < 3)
            {
                MessageBox.Show("Naziv mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }
            //necemo ogranicenje na tekst jer korisnik mozda zeli da posalje poruku sa 1 karakterom.

            try
            {
                int korisnikID = int.Parse(IzabraniKorisnik);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali korisnikov id!", "Greska", MessageBoxButton.OK);
                return false;
            }

            return true;

        }

        public bool ValidacijaIzmena()
        {
            if (string.IsNullOrEmpty(TBNaslovIzmena) || string.IsNullOrWhiteSpace(TBNaslovIzmena))
            {
                MessageBox.Show("Polje za Novi Naslov ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBTekstIzmena) || string.IsNullOrWhiteSpace(TBTekstIzmena))
            {
                MessageBox.Show("Polje za Novi Tekst ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if(DatumSlanjaIzmena == null)
            {
                MessageBox.Show("Polje za Novi DATUM SLANJA ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (DatumSlanjaIzmena.Value == DateTime.MinValue)
            {
                MessageBox.Show("Polje za Novi DATUM SLANJA nije validno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(IzabraniKorisnikIzmena) || string.IsNullOrWhiteSpace(IzabraniKorisnikIzmena))
            {
                MessageBox.Show("Polje za Novi Korisnik ID ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBNaslovIzmena.Length < 3)
            {
                MessageBox.Show("Novi Naziv mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }
            //necemo ogranicenje na tekst jer korisnik mozda zeli da posalje poruku sa 1 karakterom.

            try
            {
                int korisnikID = int.Parse(IzabraniKorisnikIzmena);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali novi korisnikov id!", "Greska", MessageBoxButton.OK);
                return false;
            }

            return true;
        }
    }
}
