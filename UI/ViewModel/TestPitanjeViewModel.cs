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
    public class TestPitanjeViewModel:BindableBase
    {
        public static ObservableCollection<Sastoji> SviTestoviIPitanja { get; set; } = new ObservableCollection<Sastoji>();
        public static ObservableCollection<string> IDSvakogPitanja { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogTesta { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogPitanjaSastoji { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogTestaSastoji { get; set; } = new ObservableCollection<string>();
        public MyICommand Kreiraj { get; set; }
        public MyICommand Izbrisi { get; set; }

        Servis.Servisi.TestPitanjeServis Baza = new Servis.Servisi.TestPitanjeServis();
        Servis.Servisi.PitanjeServis BazaPitanja = new Servis.Servisi.PitanjeServis();
        Servis.Servisi.TestServis BazaTestova = new Servis.Servisi.TestServis();

        #region Properties
        private string pitanjeDodavanje;

        public string PitanjeDodavanje
        {
            get { return pitanjeDodavanje; }
            set { pitanjeDodavanje = value; OnPropertyChanged("PitanjeDodavanje"); }
        }

        private string testDodavanje;

        public string TestDodavanje
        {
            get { return testDodavanje; }
            set { testDodavanje = value; OnPropertyChanged("TestDodavanje"); }
        }

        private string pitanjeBrisanje;

        public string PitanjeBrisanje
        {
            get { return pitanjeBrisanje; }
            set { pitanjeBrisanje = value; OnPropertyChanged("PitanjeBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }

        private string testBrisanje;

        public string TestBrisanje
        {
            get { return testBrisanje; }
            set { testBrisanje = value; OnPropertyChanged("TestBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }



        #endregion

        public TestPitanjeViewModel()
        {
            Kreiraj = new MyICommand(OnKreiraj);
            Izbrisi = new MyICommand(OnIzbrisi, CanDelete);

            DobaviPodatkeIzBaze();
        }

        private bool CanDelete() { return PitanjeBrisanje != null && TestBrisanje != null; }

        public void OnKreiraj()
        {

            try
            {
                if (!ValidacijaDodavanje()) return;

                Baza.Dodaj(int.Parse(PitanjeDodavanje), int.Parse(TestDodavanje));
                DobaviPodatkeIzBaze();

                PitanjeDodavanje = null;
                TestDodavanje = null;


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Greska", MessageBoxButton.OK);
            }
        }



        public void OnIzbrisi()
        {
            try
            {

                int idPitanja = int.Parse(PitanjeBrisanje);
                int idTesta = int.Parse(TestBrisanje);
                Baza.Obrisi(idPitanja, idTesta);
                DobaviPodatkeIzBaze();

            }
            catch
            {
                MessageBox.Show("Izaberite ID kursa i teme za brisanje!", "Greska", MessageBoxButton.OK);
            }

        }


        public void DobaviPodatkeIzBaze()
        {
            SviTestoviIPitanja.Clear();
            IDSvakogPitanja.Clear();
            IDSvakogTesta.Clear();
            IDSvakogPitanjaSastoji.Clear();
            IDSvakogTestaSastoji.Clear();

            List<Sastoji> PitanjaTestovi = Baza.UcitajSvaPitanjaITestove();
            foreach (var p in PitanjaTestovi)
            {

                SviTestoviIPitanja.Add(p);
                //da se ne dupliraju u combobox-u
                if (!IDSvakogPitanjaSastoji.Contains(p.PitanjeIdPitanja.ToString()))
                    IDSvakogPitanjaSastoji.Add(p.PitanjeIdPitanja.ToString());

                if (!IDSvakogTestaSastoji.Contains(p.TestIdTesta.ToString()))
                    IDSvakogTestaSastoji.Add(p.TestIdTesta.ToString());

            }

            BazaPitanja.UcitajSvaPitanja().ForEach(x => IDSvakogPitanja.Add(x.IdPitanja.ToString()));
            BazaTestova.UcitajSveTestove().ForEach(x => IDSvakogTesta.Add(x.IdTesta.ToString()));



            OnPropertyChanged("SviTestoviIPitanja");
            OnPropertyChanged("IDSvakogPitanja");
            OnPropertyChanged("IDSvakogTesta");
            OnPropertyChanged("IDSvakogPitanjaSastoji");
            OnPropertyChanged("IDSvakogTestaSastoji");
        }


        public bool ValidacijaDodavanje()
        {
            if (string.IsNullOrEmpty(PitanjeDodavanje) || string.IsNullOrWhiteSpace(PitanjeDodavanje))
            {
                MessageBox.Show("Polje za Pitanje id ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TestDodavanje) || string.IsNullOrWhiteSpace(TestDodavanje))
            {
                MessageBox.Show("Polje za Test id ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {

                int idPitanja = int.Parse(PitanjeDodavanje);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali pitanje id!", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {

                int idTesta = int.Parse(TestDodavanje);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali test id!", "Greska", MessageBoxButton.OK);
                return false;
            }


            return true;

        }
    }
}
