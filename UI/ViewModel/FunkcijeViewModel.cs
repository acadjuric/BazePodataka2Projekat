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
    public class FunkcijeViewModel:BindableBase
    {

        

        public static ObservableCollection<string> SviKursevi { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IdSvihKorisnika { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogKursa { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogTesta { get; set; } = new ObservableCollection<string>();

        public MyICommand TestoviZaKurs { get; set; } 
        public MyICommand ObavestenjaZaOdKorisnika { get; set; } 
        public MyICommand BrojUcenikaPolaganje { get; set; } 


        Servis.Servisi.PrijavljenServis bazaPrijvaljenih = new Servis.Servisi.PrijavljenServis();
        Servis.Servisi.NastavnikServis BazaNastavnika = new Servis.Servisi.NastavnikServis();
        Servis.Servisi.UcenikServis BazaUcenika = new Servis.Servisi.UcenikServis();
        Servis.Servisi.TestServis BazaTestova = new Servis.Servisi.TestServis();
        Servis.Servisi.KursServis BazaKurseva = new Servis.Servisi.KursServis();

        Servis.Servisi.FunkcijeServis funkcijeServis = new Servis.Servisi.FunkcijeServis();

        #region Properties
        private string izabraniKurs1;

        public string IzabraniKurs1
        {
            get { return izabraniKurs1; }
            set { izabraniKurs1 = value; OnPropertyChanged("IzabraniKurs1"); }
        }

        private string rezultat1;

        public string Rezultat1
        {
            get { return rezultat1; }
            set { rezultat1 = value; OnPropertyChanged("Rezultat1"); }
        }

        private string rezultat2;

        public string Rezultat2
        {
            get { return rezultat2; }
            set { rezultat2 = value; OnPropertyChanged("Rezultat2"); }
        }

        private string rezultat3;

        public string Rezultat3
        {
            get { return rezultat3; }
            set { rezultat3 = value; OnPropertyChanged("Rezultat3"); }
        }

        private string izabraniKorisnik;

        public string IzabraniKorisnik
        {
            get { return izabraniKorisnik; }
            set { izabraniKorisnik = value; OnPropertyChanged("IzabraniKorisnik"); }
        }

        private string izabraniKurs2;

        public string IzabraniKurs2
        {
            get { return izabraniKurs2; }
            set { izabraniKurs2 = value; OnPropertyChanged("IzabraniKurs2"); }
        }

        private string izabraniTest2;

        public string IzabraniTest2
        {
            get { return izabraniTest2; }
            set { izabraniTest2 = value; OnPropertyChanged("IzabraniTest2"); }
        }
        #endregion


        public FunkcijeViewModel()
        {
            TestoviZaKurs = new MyICommand(OnTestoviZaKurs);
            ObavestenjaZaOdKorisnika = new MyICommand(OnObavestenjaZaOdKorisnika);
            BrojUcenikaPolaganje = new MyICommand(OnBrojUcenikaPolaganje);
            DobaviPodatkeIzBaze();
        }
        public void OnTestoviZaKurs()
        {
            if(string.IsNullOrEmpty(IzabraniKurs1) || string.IsNullOrWhiteSpace(IzabraniKurs1))
            {
                MessageBox.Show("Morate izabrati id kursa", "Greska", MessageBoxButton.OK);
                return;
            }

            Rezultat1 = funkcijeServis.FuncBrojTestovaZaKurs(int.Parse(IzabraniKurs1)).ToString();

        }

        public void OnObavestenjaZaOdKorisnika()
        {
            if (string.IsNullOrEmpty(IzabraniKorisnik) || string.IsNullOrWhiteSpace(IzabraniKorisnik))
            {
                MessageBox.Show("Morate izabrati id korisnika", "Greska", MessageBoxButton.OK);
                return;
            }

            Rezultat2 = funkcijeServis.FuncBrojSvihObavestenjaZaKorisnika(int.Parse(IzabraniKorisnik)).ToString();

        }

        public void OnBrojUcenikaPolaganje()
        {
            if (string.IsNullOrEmpty(IzabraniKurs2) || string.IsNullOrWhiteSpace(IzabraniKurs2))
            {
                MessageBox.Show("Morate izabrati id kursa", "Greska", MessageBoxButton.OK);
                return;
            }
            if (string.IsNullOrEmpty(IzabraniTest2) || string.IsNullOrWhiteSpace(IzabraniTest2))
            {
                MessageBox.Show("Morate izabrati id testa", "Greska", MessageBoxButton.OK);
                return;
            }

            Rezultat3 = funkcijeServis.FuncBrojUcenikaKojiPolazuIzabraniKursITest(int.Parse(IzabraniKurs2), int.Parse(IzabraniTest2)).ToString();
        }


        private void DobaviPodatkeIzBaze()
        {
            SviKursevi.Clear();
            IdSvihKorisnika.Clear();
            IDSvakogTesta.Clear();
            IDSvakogKursa.Clear();

            //var listaPrijavljenih = bazaPrijvaljenih.UcitajSvePrijavljene();
            //foreach (var item in listaPrijavljenih)
            //{
            //    if (!SviPrijavljeniKursevi.Contains(item.KursIdKursa.ToString()))
            //    {
            //        SviPrijavljeniKursevi.Add(item.KursIdKursa.ToString());
            //    }
            //}

            

            var listaNastavnika = BazaNastavnika.UcitajSveNastavnike();
            listaNastavnika.ForEach(x => IdSvihKorisnika.Add(x.IdKorisnika.ToString()));

            var listaUcenika = BazaUcenika.UcitajSveUcenike();
            listaUcenika.ForEach(x => IdSvihKorisnika.Add(x.IdKorisnika.ToString()));

            var listaKurseva = BazaKurseva.UcitajSveKurseve();
            listaKurseva.ForEach(x => IDSvakogKursa.Add(x.IdKursa.ToString()));

            listaKurseva.ForEach(x => SviKursevi.Add(x.IdKursa.ToString()));

            var listaTestova = BazaTestova.UcitajSveTestove();
            listaTestova.ForEach(x => IDSvakogTesta.Add(x.IdTesta.ToString()));

            OnPropertyChanged("SviKursevi");
            OnPropertyChanged("IdSvihKorisnika");
            OnPropertyChanged("IDSvakogKursa");
            OnPropertyChanged("IDSvakogTesta");
        }
    }
}
