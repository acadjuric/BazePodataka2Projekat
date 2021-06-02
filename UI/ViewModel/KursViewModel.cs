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
    public class KursViewModel : BindableBase
    {

        public static ObservableCollection<Kurs> SviKursevi { get; set; } = new ObservableCollection<Kurs>();
        public static ObservableCollection<string> IDSvakogKursa { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IdSvihKorisnika { get; set; } = new ObservableCollection<string>();
        public static List<NastavnaTema> IzabraneTemeZaKurs { get; set; }
        public static List<NastavnaTema> IzabraneTemeZaKursDodavanje { get; set; }

        public MyICommand Kreiraj { get; set; }
        public MyICommand Izmeni { get; set; }
        public MyICommand Izbrisi { get; set; }
        public MyICommand DodajTeme { get; set; }
        public MyICommand IzmeniTeme { get; set; }

        Servis.Servisi.KursServis Baza = new Servis.Servisi.KursServis();
        Servis.Servisi.NastavnikServis BazaNastavnika = new Servis.Servisi.NastavnikServis();
        Servis.Servisi.KursNastavnaTemaServis BazaKursNastavnaTema = new Servis.Servisi.KursNastavnaTemaServis();
        Servis.Servisi.NastavnaTemaServis BazaNastavnihTema = new Servis.Servisi.NastavnaTemaServis();

        #region Properties
        private string tbNaziv;

        public string TBNaziv
        {
            get { return tbNaziv; }
            set { tbNaziv = value; OnPropertyChanged("TBNaziv"); }
        }

        private string tbPoeni;

        public string TBPoeni
        {
            get { return tbPoeni; }
            set { tbPoeni = value; OnPropertyChanged("TBPoeni"); }
        }


        private string tbNazivIzmena;

        public string TBNazivIzmena
        {
            get { return tbNazivIzmena; }
            set { tbNazivIzmena = value; OnPropertyChanged("TBNazivIzmena"); }
        }



        private string tbPoeniIzmena;

        public string TBPoeniIzmena
        {
            get { return tbPoeniIzmena; }
            set { tbPoeniIzmena = value; OnPropertyChanged("TBPoeniIzmena"); }
        }

        private string idIzmena;

        public string IdIzmena
        {
            get { return idIzmena; }
            set { idIzmena = value; OnPropertyChanged("IdIzmena");
                Izmeni.RaiseCanExecuteChanged(); IzmeniTeme.RaiseCanExecuteChanged();
                if(IdIzmena != null)
                {
                    DobaviKursISveNjegoveAtribute(int.Parse(IdIzmena), true);
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

        int poslednjiIdKursaZaKojiJeKliknutoIzmeniTeme = -1;

        public KursViewModel()
        {
            IzabraneTemeZaKurs = new List<NastavnaTema>();
            IzabraneTemeZaKursDodavanje = new List<NastavnaTema>();
            Kreiraj = new MyICommand(OnKreiraj);
            Izmeni = new MyICommand(OnIzmeni, CanEdit);
            Izbrisi = new MyICommand(OnIzbrisi, CanDelete);
            DodajTeme = new MyICommand(OnDodajTeme);
            IzmeniTeme = new MyICommand(OnIzmeniTeme, CanEdit);//omoguceno je samo ako je izabran id kursa za koji zeli da izmeni teme

            poslednjiIdKursaZaKojiJeKliknutoIzmeniTeme = -1;

            DobaviPodatkeIzBaze();


        }

        public void OnIzmeniTeme()
        {
            
            int id = int.Parse(IdIzmena);

            if (poslednjiIdKursaZaKojiJeKliknutoIzmeniTeme != id)
            {
                DobaviKursISveNjegoveAtribute(id, false);
                //IzabraneTemeZaKurs.Clear(); ubaceno u metodu

                //Kurs k = Baza.VratiKurs(id);
                //if (k == null) return;

                //List<Sadrzi> SveTemeZaKursId = BazaKursNastavnaTema.VratiTemeZaKurs(id);
                //if (SveTemeZaKursId != null)
                //{
                //    foreach (var sadrzi in SveTemeZaKursId)
                //    {
                //        NastavnaTema item = null;
                //        item = BazaNastavnihTema.VratiNastavnuTemu(sadrzi.NastavnaTemaIdTeme);
                //        if (item != null)
                //            IzabraneTemeZaKurs.Add(item);
                //    }
                //}
            }
            poslednjiIdKursaZaKojiJeKliknutoIzmeniTeme = id;
            Views.KursNastavaWindow prozor = new Views.KursNastavaWindow(true);
            prozor.ShowDialog();
        }

        public void OnDodajTeme()
        {
            Views.KursNastavaWindow prozor = new Views.KursNastavaWindow(false);
            prozor.ShowDialog();

        }

        private void DobaviKursISveNjegoveAtribute(int id, bool IndikatorZaPopunjavanjePoljaZaIzmenu)
        {
            
            Kurs k = Baza.VratiKurs(id);
            if (k == null) return;

            IzabraneTemeZaKurs.Clear();

            if (IndikatorZaPopunjavanjePoljaZaIzmenu)
            {
                TBNazivIzmena = k.Naziv;
                TBPoeniIzmena = k.Poeni.ToString();
                IzabraniKorisnikIzmena = k.NastavnikIdKorisnika.ToString();
            }

            List<Sadrzi> SveTemeZaKursId = BazaKursNastavnaTema.VratiTemeZaKurs(id);
            if (SveTemeZaKursId != null)
            {
                foreach (var sadrzi in SveTemeZaKursId)
                {
                    NastavnaTema item = null;
                    item = BazaNastavnihTema.VratiNastavnuTemu(sadrzi.NastavnaTemaIdTeme);
                    if (item != null)
                        IzabraneTemeZaKurs.Add(item);
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

                Baza.DodajKurs(TBNaziv, int.Parse(TBPoeni), int.Parse(IzabraniKorisnik), IzabraneTemeZaKursDodavanje);
                DobaviPodatkeIzBaze();
                TBNaziv = string.Empty;
                TBPoeni = string.Empty;
                IzabraneTemeZaKursDodavanje.Clear();
            }
            catch (Exception e)
            {

            }
        }

        public void OnIzmeni()
        {
            try
            {
                if (!ValidacijaIzmena()) return;

                int id = int.Parse(IdIzmena);

                int bodovi = int.Parse(TBPoeniIzmena);

                Baza.IzmeniKurs(id, TBNazivIzmena, bodovi, int.Parse(IzabraniKorisnikIzmena), IzabraneTemeZaKurs);
                DobaviPodatkeIzBaze();

                TBPoeniIzmena = string.Empty;
                TBNazivIzmena = string.Empty;
                IdIzmena = null;
                IzabraneTemeZaKurs.Clear();
                poslednjiIdKursaZaKojiJeKliknutoIzmeniTeme = -1;
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
                Baza.ObrisiKurs(id);
                DobaviPodatkeIzBaze();

            }
            catch
            {
                MessageBox.Show("Izaberite ID za brisanje!", "Greska", MessageBoxButton.OK);
            }

        }


        public void DobaviPodatkeIzBaze()
        {
            SviKursevi.Clear();
            IDSvakogKursa.Clear();
            IdSvihKorisnika.Clear();

            List<Kurs> Kursevi = Baza.UcitajSveKurseve();
            foreach (var p in Kursevi)
            {
                SviKursevi.Add(p);
                IDSvakogKursa.Add(p.IdKursa.ToString());
            }

            BazaNastavnika.UcitajSveNastavnike().ForEach(x => IdSvihKorisnika.Add(x.IdKorisnika.ToString()));

            OnPropertyChanged("IdSvihKorisnika");
            OnPropertyChanged("IDSvakogKursa");
            OnPropertyChanged("SviKursevi");

        }


        public bool ValidacijaDodavanje()
        {
            if (string.IsNullOrEmpty(TBNaziv) || string.IsNullOrWhiteSpace(TBNaziv))
            {
                MessageBox.Show("Polje za Naziv ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBPoeni) || string.IsNullOrWhiteSpace(TBPoeni))
            {
                MessageBox.Show("Polje za Poene ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(IzabraniKorisnik) || string.IsNullOrWhiteSpace(IzabraniKorisnik))
            {
                MessageBox.Show("Polje za Nastavnik Id ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBNaziv.Length < 3)
            {

                MessageBox.Show("Naziv mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {

                int poeni = int.Parse(TBPoeni);
            }
            catch
            {
                MessageBox.Show("Niste dobro uneli poene!", "Greska", MessageBoxButton.OK);
                return false;
            }

            if (IzabraneTemeZaKursDodavanje.Count == 0)
            {
                MessageBox.Show("Morate izabrati BAR jednu Nastavnu Temu za ovaj Kurs", "Greska", MessageBoxButton.OK);
                return false;
            }


            return true;

        }

        public bool ValidacijaIzmena()
        {
            if (string.IsNullOrEmpty(TBNazivIzmena) || string.IsNullOrWhiteSpace(TBNazivIzmena))
            {
                MessageBox.Show("Polje za Novi Naziv ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBPoeniIzmena) || string.IsNullOrWhiteSpace(TBPoeniIzmena))
            {
                MessageBox.Show("Polje za Nove Poene ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(IzabraniKorisnikIzmena) || string.IsNullOrWhiteSpace(IzabraniKorisnikIzmena))
            {
                MessageBox.Show("Polje za Novi Nastavnik Id ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBNazivIzmena.Length < 3)
            {

                MessageBox.Show("Novi Naziv mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {

                int poeni = int.Parse(TBPoeniIzmena);
            }
            catch
            {
                MessageBox.Show("Niste dobro uneli nove poene!", "Greska", MessageBoxButton.OK);
                return false;
            }

            if (IzabraneTemeZaKurs.Count == 0)
            {
                MessageBox.Show("Morate izabrati BAR jednu Nastavnu Temu za ovaj Kurs", "Greska", MessageBoxButton.OK);
                return false;
            }

            return true;
        }


    }
}
