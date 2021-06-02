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
    public class TestViewModel : BindableBase
    {

        public static ObservableCollection<Test> SviTestovi { get; set; } = new ObservableCollection<Test>();
        public static ObservableCollection<string> IDSvakogTesta { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IdSvihKurseva { get; set; } = new ObservableCollection<string>();
        public static List<Pitanje> IzabranaPitanjaZaTest { get; set; }
        public static List<Pitanje> IzabranaPitanjaZaTestDodavanje { get; set; }

        public MyICommand Kreiraj { get; set; }
        public MyICommand Izmeni { get; set; }
        public MyICommand Izbrisi { get; set; }
        public MyICommand DodajPitanja { get; set; }
        public MyICommand IzmeniPitanja { get; set; }

        Servis.Servisi.TestServis Baza = new Servis.Servisi.TestServis();
        Servis.Servisi.KursServis BazaKurseva = new Servis.Servisi.KursServis();
        Servis.Servisi.TestPitanjeServis BazaTestovaIPitanja = new Servis.Servisi.TestPitanjeServis();
        Servis.Servisi.PitanjeServis BazaPitanja = new Servis.Servisi.PitanjeServis();


        #region Properties
        private string tbNaziv;

        public string TBNaziv
        {
            get { return tbNaziv; }
            set { tbNaziv = value; OnPropertyChanged("TBNaziv"); }
        }

        private DateTime? datum;

        public DateTime? Datum
        {
            get { return datum; }
            set { datum = value; OnPropertyChanged("Datum"); }
        }

        private string tbBodovi;

        public string TBBodovi
        {
            get { return tbBodovi; }
            set { tbBodovi = value; OnPropertyChanged("TBBodovi"); }
        }


        private string tbNazivIzmena;

        public string TBNazivIzmena
        {
            get { return tbNazivIzmena; }
            set { tbNazivIzmena = value; OnPropertyChanged("TBNazivIzmena"); }
        }

        private DateTime? datumIzmena;

        public DateTime? DatumIzmena
        {
            get { return datumIzmena; }
            set { datumIzmena = value; OnPropertyChanged("DatumIzmena"); }
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
            set
            {
                idIzmena = value; OnPropertyChanged("IdIzmena");
                Izmeni.RaiseCanExecuteChanged(); IzmeniPitanja.RaiseCanExecuteChanged();

                if (idIzmena != null)
                {
                    DobaviTestISveNjegoveAtributeIzBaze(int.Parse(IdIzmena), true);
                }
            }
        }

        private string idBrisanje;

        public string IdBrisanje
        {
            get { return idBrisanje; }
            set { idBrisanje = value; OnPropertyChanged("IdBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }

        private string izabraniKurs;

        public string IzabraniKurs
        {
            get { return izabraniKurs; }
            set { izabraniKurs = value; OnPropertyChanged("IzabraniKurs"); }
        }


        private string izabraniKursIzmena;

        public string IzabraniKursIzmena
        {
            get { return izabraniKursIzmena; }
            set { izabraniKursIzmena = value; OnPropertyChanged("IzabraniKursIzmena"); }
        }

        #endregion

        int poslednjiIdTestaZaKojiJeKliknutoIzmeniPitanje = -1;

        public TestViewModel()
        {
            IzabranaPitanjaZaTestDodavanje = new List<Pitanje>();
            IzabranaPitanjaZaTest = new List<Pitanje>();

            Kreiraj = new MyICommand(OnKreiraj);
            Izmeni = new MyICommand(OnIzmeni, CanEdit);
            Izbrisi = new MyICommand(OnIzbrisi, CanDelete);
            DodajPitanja = new MyICommand(OnDodajPitanja);
            IzmeniPitanja = new MyICommand(OnIzmeniPitanja, CanEdit);

            poslednjiIdTestaZaKojiJeKliknutoIzmeniPitanje = -1;

            DobaviPodatkeIzBaze();
        }

        private void OnDodajPitanja()
        {
            Views.TestPitanjeWindow prozor = new Views.TestPitanjeWindow(false);
            prozor.ShowDialog();

        }

        private void OnIzmeniPitanja()
        {

            int id = int.Parse(IdIzmena);

            if (poslednjiIdTestaZaKojiJeKliknutoIzmeniPitanje != id)
            {
                //IzabranaPitanjaZaTest.Clear(); ubaceno u metodu
                DobaviTestISveNjegoveAtributeIzBaze(id,false);
                //Test t = Baza.VratiTest(id);

                //if (t == null) return;

                //List<Sastoji> SvaPitanjaZaTest = BazaTestovaIPitanja.VratiPitanjaZaTest(id);
                //if(SvaPitanjaZaTest != null)
                //{
                //    foreach(var sastoji in SvaPitanjaZaTest)
                //    {
                //        Pitanje p = null;
                //        p = BazaPitanja.VratiPitanje(sastoji.PitanjeIdPitanja);
                //        if (p != null)
                //            IzabranaPitanjaZaTest.Add(p);
                //    }
                //}

            }

            poslednjiIdTestaZaKojiJeKliknutoIzmeniPitanje = id;
            Views.TestPitanjeWindow prozor = new Views.TestPitanjeWindow(true);
            prozor.ShowDialog();
        }

        private void DobaviTestISveNjegoveAtributeIzBaze(int id, bool IndikatorZaPopunjavanjePoljaZaIzmene)
        {
            Test t = Baza.VratiTest(id);
            if (t == null) return;

            IzabranaPitanjaZaTest.Clear();

            if (IndikatorZaPopunjavanjePoljaZaIzmene)
            {
                TBNazivIzmena = t.Naziv;
                TBBodoviIzmena = t.Bodovi.ToString();
                DatumIzmena = t.Datum;
                IzabraniKursIzmena = t.KursIdKursa.ToString();
            }

            List<Sastoji> SvaPitanjaZaTest = BazaTestovaIPitanja.VratiPitanjaZaTest(id);
            if (SvaPitanjaZaTest != null)
            {
                foreach (var sastoji in SvaPitanjaZaTest)
                {
                    Pitanje p = null;
                    p = BazaPitanja.VratiPitanje(sastoji.PitanjeIdPitanja);
                    if (p != null)
                        IzabranaPitanjaZaTest.Add(p);
                }
            }
        }

        private bool CanEdit() { return IdIzmena != null; }
        private bool CanDelete() { return IdBrisanje != null; }

        private void OnKreiraj()
        {
            try
            {
                if (!ValidacijaDodavanje()) return;

                Baza.DodajTest(TBNaziv, (DateTime)Datum, int.Parse(TBBodovi), int.Parse(IzabraniKurs), IzabranaPitanjaZaTestDodavanje);
                DobaviPodatkeIzBaze();
                TBNaziv = string.Empty;
                TBBodovi = string.Empty;
                Datum = null;
                IzabranaPitanjaZaTestDodavanje.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show("Nesto nije u redu kod dodavanja", "Greska", MessageBoxButton.OK);
            }

        }

        private void OnIzmeni()
        {
            try
            {
                if (!ValidacijaIzmena()) return;

                int id = int.Parse(IdIzmena);

                int bodovi = int.Parse(TBBodoviIzmena);

                Baza.IzmeniTest(id, TBNazivIzmena, (DateTime)DatumIzmena, bodovi, int.Parse(IzabraniKursIzmena), IzabranaPitanjaZaTest);
                DobaviPodatkeIzBaze();

                TBBodoviIzmena = string.Empty;
                DatumIzmena = null;
                TBNazivIzmena = string.Empty;
                IdIzmena = null;
                IzabranaPitanjaZaTest.Clear();
                poslednjiIdTestaZaKojiJeKliknutoIzmeniPitanje = -1;
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

                Baza.ObrisiTest(id);
                DobaviPodatkeIzBaze();
            }
            catch
            {
                MessageBox.Show("Izaberite ID za brisanje!", "Greska", MessageBoxButton.OK);
            }

        }

        public void DobaviPodatkeIzBaze()
        {
            SviTestovi.Clear();
            IDSvakogTesta.Clear();
            IdSvihKurseva.Clear();

            List<Test> Testovi = Baza.UcitajSveTestove();
            foreach (var t in Testovi)
            {
                SviTestovi.Add(t);
                IDSvakogTesta.Add(t.IdTesta.ToString());
            }

            BazaKurseva.UcitajSveKurseve().ForEach(x => IdSvihKurseva.Add(x.IdKursa.ToString()));

            OnPropertyChanged("IDSvakogTesta");
            OnPropertyChanged("SviTestovi");
            OnPropertyChanged("IdSvihKurseva");

        }

        public bool ValidacijaDodavanje()
        {
            if (string.IsNullOrEmpty(TBNaziv) || string.IsNullOrWhiteSpace(TBNaziv))
            {
                MessageBox.Show("Polje za Naziv ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBBodovi) || string.IsNullOrWhiteSpace(TBBodovi))
            {
                MessageBox.Show("Polje za Bodove ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (Datum == null)
            {
                MessageBox.Show("Polje za DATUM ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (Datum.Value == DateTime.MinValue)
            {
                MessageBox.Show("Polje za DATUM nije validno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(IzabraniKurs) || string.IsNullOrWhiteSpace(IzabraniKurs))
            {
                MessageBox.Show("Polje za KURS ID ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBNaziv.Length < 3)
            {
                MessageBox.Show("Naziv mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
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
                int IdIzabranogKursa = int.Parse(IzabraniKurs);
            }
            catch
            {
                MessageBox.Show("Niste dobro izbarali kurs!", "Greska", MessageBoxButton.OK);
                return false;
            }

            if (IzabranaPitanjaZaTestDodavanje.Count == 0)
            {
                MessageBox.Show("Morate izabrati BAR jedno Pitanje za ovaj Test", "Greska", MessageBoxButton.OK);
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
            if (string.IsNullOrEmpty(TBBodoviIzmena) || string.IsNullOrWhiteSpace(TBBodoviIzmena))
            {
                MessageBox.Show("Polje za Nove Bodove ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (DatumIzmena == null)
            {
                MessageBox.Show("Polje za Novi DATUM ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (DatumIzmena.Value == DateTime.MinValue)
            {
                MessageBox.Show("Polje za Novi DATUM nije validno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(IzabraniKursIzmena) || string.IsNullOrWhiteSpace(IzabraniKursIzmena))
            {
                MessageBox.Show("Polje za Novi KURS ID ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBNazivIzmena.Length < 3)
            {
                MessageBox.Show("Novi Naziv mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                int bodovi = int.Parse(TBBodoviIzmena);
            }
            catch
            {
                MessageBox.Show("Niste dobro uneli NOVE bodove!", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                int IdIzabranogKursa = int.Parse(IzabraniKursIzmena);
            }
            catch
            {
                MessageBox.Show("Niste dobro izbarali NOVI kurs!", "Greska", MessageBoxButton.OK);
                return false;
            }

            if (IzabranaPitanjaZaTest.Count == 0)
            {
                MessageBox.Show("Morate izabrati BAR jedno Pitanje za ovaj Test", "Greska", MessageBoxButton.OK);
                return false;
            }

            return true;
        }
    }
}
