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
    public class PrijavljenTestViewModel:BindableBase
    {

        public static ObservableCollection<Polaze> SviPrijavljeniITestovi { get; set; } = new ObservableCollection<Polaze>();
        public static ObservableCollection<string> IDSvakogPrijavljenog { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogTesta { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogPrijavljenogPolaze { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogTestaPolaze { get; set; } = new ObservableCollection<string>();
        public MyICommand Kreiraj { get; set; }
        public MyICommand Izbrisi { get; set; }

        Servis.Servisi.PrijavljenTestServis Baza = new Servis.Servisi.PrijavljenTestServis();
        Servis.Servisi.PrijavljenServis BazaPrijavljenih = new Servis.Servisi.PrijavljenServis();
        Servis.Servisi.TestServis BazaTestova = new Servis.Servisi.TestServis();

        #region Properties
        private string prijavljenDodavanje;

        public string PrijavljenDodavanje
        {
            get { return prijavljenDodavanje; }
            set { prijavljenDodavanje = value;
                OnPropertyChanged("PrijavljenDodavanje");
                if (PrijavljenDodavanje != null)
                {
                    var SviTestovi = BazaTestova.UcitajSveTestove();
                    int kursId = int.Parse(PrijavljenDodavanje.Split(',')[0]);
                    SviTestovi.RemoveAll(x => x.KursIdKursa != kursId);
                    IDSvakogTesta.Clear();
                    SviTestovi.ForEach(x => IDSvakogTesta.Add(x.IdTesta.ToString()));
                }
            }
        }

        private string testDodavanje;

        public string TestDodavanje
        {
            get { return testDodavanje; }
            set { testDodavanje = value; OnPropertyChanged("TestDodavanje"); }
        }

        private string prijavljenBrisanje;

        public string PrijavljenBrisanje
        {
            get { return prijavljenBrisanje; }
            set { prijavljenBrisanje = value; OnPropertyChanged("PrijavljenBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }

        private string testBrisanje;

        public string TestBrisanje
        {
            get { return testBrisanje; }
            set { testBrisanje = value; OnPropertyChanged("TestBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }



        #endregion

        public PrijavljenTestViewModel()
        {
            Kreiraj = new MyICommand(OnKreiraj);
            Izbrisi = new MyICommand(OnIzbrisi, CanDelete);

            DobaviPodatkeIzBaze();
        }

        private bool CanDelete() { return PrijavljenBrisanje != null && TestBrisanje != null; }

        public void OnKreiraj()
        {

            try
            {
                if (!ValidacijaDodavanje()) return;

                string[] delovi = PrijavljenDodavanje.Split(',');

                Baza.Dodaj(int.Parse(delovi[0]),int.Parse(delovi[1]), int.Parse(TestDodavanje));
                DobaviPodatkeIzBaze();

                PrijavljenDodavanje = null;
                TestDodavanje = null;


            }
            catch (Exception e)
            {

            }
        }



        public void OnIzbrisi()
        {
            try
            {

                string[] delovi = PrijavljenBrisanje.Split(',');
                int idTesta = int.Parse(TestBrisanje);
                
                Baza.Obrisi(int.Parse(delovi[0]),int.Parse(delovi[1]), idTesta);
                DobaviPodatkeIzBaze();

            }
            catch
            {
                MessageBox.Show("Izaberite ID kursa i teme za brisanje!", "Greska", MessageBoxButton.OK);
            }

        }


        public void DobaviPodatkeIzBaze()
        {
            SviPrijavljeniITestovi.Clear();
            IDSvakogPrijavljenog.Clear();
            IDSvakogTesta.Clear();
            IDSvakogPrijavljenogPolaze.Clear();
            IDSvakogTestaPolaze.Clear();

            List<Polaze> PrijavljeniTestovi = Baza.UcitajSvePrijavljeneITestove();
            foreach (var p in PrijavljeniTestovi)
            {

                SviPrijavljeniITestovi.Add(p);
                //da se ne dupliraju u combobox-u
                string kljuc = p.PrijavljenKursIdKursa + "," + p.PrijavljenUcenikIdKorisnika;
                if (!IDSvakogPrijavljenogPolaze.Contains(kljuc))
                    IDSvakogPrijavljenogPolaze.Add(kljuc);

                if (!IDSvakogTestaPolaze.Contains(p.TestIdTesta.ToString()))
                    IDSvakogTestaPolaze.Add(p.TestIdTesta.ToString());

            }
            var sviPrijavljeni = BazaPrijavljenih.UcitajSvePrijavljene();
            foreach(var item in sviPrijavljeni)
            {
                IDSvakogPrijavljenog.Add(item.KursIdKursa + "," + item.UcenikIdKorisnika);
            }

            //nema na pocetku, prilikom odabria prijavljenog ID-a
            // uzima se kurs id i nalaze se testovi koji pripadaju samo tom kursu i oni se prikazuju u
            // listi IDSvakogTesta
            // pogledaj PROPFULL za PrijavljenDodavanje tj. njegov SETTER
            //BazaTestova.UcitajSveTestove().ForEach(x => IDSvakogTesta.Add(x.IdTesta.ToString()));



            OnPropertyChanged("SviPrijavljeniITestovi");
            OnPropertyChanged("IDSvakogPrijavljenog");
            OnPropertyChanged("IDSvakogTesta");
            OnPropertyChanged("IDSvakogPrijavljenogPolaze");
            OnPropertyChanged("IDSvakogTestaPolaze");
        }

        public bool ValidacijaDodavanje()
        {
            if (string.IsNullOrEmpty(PrijavljenDodavanje) || string.IsNullOrWhiteSpace(PrijavljenDodavanje))
            {
                MessageBox.Show("Polje za Prijvaljen id ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TestDodavanje) || string.IsNullOrWhiteSpace(TestDodavanje))
            {
                MessageBox.Show("Polje za Test id ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {

                string[] delovi = PrijavljenDodavanje.Split(',');
                int kursId = int.Parse(delovi[0]);
                int ucenikID = int.Parse(delovi[1]);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali prijavljen id!", "Greska", MessageBoxButton.OK);
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
