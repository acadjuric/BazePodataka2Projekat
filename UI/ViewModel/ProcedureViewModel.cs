using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Helpers;
using Servis.Pomocne_Klase;
using System.Windows;

namespace UI.ViewModel
{
    public class ProcedureViewModel:BindableBase
    {

        public static ObservableCollection<ProcUcenikVidiPitanjaZaTest> DobijenaPitanjaZaKursITest { get; set; } = new ObservableCollection<ProcUcenikVidiPitanjaZaTest>();
        public static ObservableCollection<ProcNastavnikoveNastavneTeme> DobijeneNastavnikoveTeme { get; set; } = new ObservableCollection<ProcNastavnikoveNastavneTeme>();
        public static ObservableCollection<ProcImePrezimeZvanjeNastavnikaZaKurs> DobijenNastavnik { get; set; } = new ObservableCollection<ProcImePrezimeZvanjeNastavnikaZaKurs>();

        public static ObservableCollection<string> SviPrijavljeniKursevi { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IdSvihNastavnika { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogKursa { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogTesta { get; set; } = new ObservableCollection<string>();

        public MyICommand NastavnikZaKurs { get; set; } //izabere se kurs trazi se nastavnik
        public MyICommand NastavnikoveTeme { get; set; } //izabere nastavnik i traze se teme njegove
        public MyICommand PitanjaZaTest { get; set; } //izabere se kurs i test trazi se pitanja
                                                      //public static ObservableCollection<string> IDSvakogKursa { get; set; } = new ObservableCollection<string>();
                                                      //public static ObservableCollection<string> IdSvihKorisnika { get; set; } = new ObservableCollection<string>();

        //public static ObservableCollection<string> IDSvakogKursa { get; set; } = new ObservableCollection<string>();
        //public static ObservableCollection<string> IdSvihKorisnika { get; set; } = new ObservableCollection<string>();

        //public static ObservableCollection<string> IDSvakogKursa { get; set; } = new ObservableCollection<string>();
        //public static ObservableCollection<string> IdSvihKorisnika { get; set; } = new ObservableCollection<string>();
        Servis.Servisi.PrijavljenServis bazaPrijvaljenih = new Servis.Servisi.PrijavljenServis();
        Servis.Servisi.NastavnikServis BazaNastavnika = new Servis.Servisi.NastavnikServis();
        Servis.Servisi.TestServis BazaTestova = new Servis.Servisi.TestServis();
        Servis.Servisi.KursServis BazaKurseva = new Servis.Servisi.KursServis();
        Servis.Servisi.ProcedureServis procedureServis = new Servis.Servisi.ProcedureServis();

        #region Properties
        private string izabraniKurs1;

        public string IzabraniKurs1
        {
            get { return izabraniKurs1; }
            set { izabraniKurs1 = value; OnPropertyChanged("IzabraniKurs1"); }
        }

        private string izabraniNastavnik;

        public string IzabraniNastavnik
        {
            get { return izabraniNastavnik; }
            set { izabraniNastavnik = value; OnPropertyChanged("IzabraniNastavnik"); }
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


        public ProcedureViewModel()
        {
            NastavnikZaKurs = new MyICommand(OnNastavnikZaKurs);
            NastavnikoveTeme = new MyICommand(OnNastavnikoveTeme);
            PitanjaZaTest = new MyICommand(OnPitanjaZaTest);
            DobaviPodatkeIzBaze();
        }

        private void OnNastavnikZaKurs()
        {
            if(string.IsNullOrEmpty(IzabraniKurs1) || string.IsNullOrWhiteSpace(IzabraniKurs1))
            {
                MessageBox.Show("Morate izabrati id kursa", "Greska", MessageBoxButton.OK);
                return;
            }
            DobijenNastavnik.Clear();
            var lista= procedureServis.ProcImePrezimeNastavnikaZaKurs(int.Parse(IzabraniKurs1));
            lista.ForEach(x => DobijenNastavnik.Add(x));
            OnPropertyChanged("DobijenNastavnik");
        }

        private void OnNastavnikoveTeme()
        {
            if (string.IsNullOrEmpty(IzabraniNastavnik) || string.IsNullOrWhiteSpace(IzabraniNastavnik))
            {
                MessageBox.Show("Morate izabrati id nastavnika", "Greska", MessageBoxButton.OK);
                return;
            }
            DobijeneNastavnikoveTeme.Clear();
            var lista = procedureServis.ProcNastavnikoveNastavneTeme(int.Parse(IzabraniNastavnik));
            lista.ForEach(x => DobijeneNastavnikoveTeme.Add(x));
            OnPropertyChanged("DobijeneNastavnikoveTeme");

        }

        private void OnPitanjaZaTest()
        {
            if (string.IsNullOrEmpty(IzabraniKurs2) || string.IsNullOrWhiteSpace(IzabraniKurs2))
            {
                MessageBox.Show("Morate izabrati id kursa", "Greska", MessageBoxButton.OK);
                return;
            }
            if (string.IsNullOrEmpty(IzabraniTest2) || string.IsNullOrWhiteSpace(IzabraniTest2))
            {
                MessageBox.Show("Morate izabrati id test", "Greska", MessageBoxButton.OK);
                return;
            }
            DobijenaPitanjaZaKursITest.Clear();
            var lista = procedureServis.ProcUcenikVidiPitanjaZaTest(int.Parse(IzabraniKurs2),int.Parse(IzabraniTest2));
            lista.ForEach(x => DobijenaPitanjaZaKursITest.Add(x));
           OnPropertyChanged("DobijenaPitanjaZaKursITest");
        }

        private void DobaviPodatkeIzBaze()
        {
            SviPrijavljeniKursevi.Clear();
            IdSvihNastavnika.Clear();
            IDSvakogTesta.Clear();
            IDSvakogKursa.Clear();

            var listaPrijavljenih = bazaPrijvaljenih.UcitajSvePrijavljene();
            foreach(var item in listaPrijavljenih)
            {
                if (!SviPrijavljeniKursevi.Contains(item.KursIdKursa.ToString()))
                {
                    SviPrijavljeniKursevi.Add(item.KursIdKursa.ToString());
                }
            }

            var listaNastavnika = BazaNastavnika.UcitajSveNastavnike();
            listaNastavnika.ForEach(x => IdSvihNastavnika.Add(x.IdKorisnika.ToString()));

            var listaKurseva = BazaKurseva.UcitajSveKurseve();
            listaKurseva.ForEach(x => IDSvakogKursa.Add(x.IdKursa.ToString()));

            var listaTestova = BazaTestova.UcitajSveTestove();
            listaTestova.ForEach(x => IDSvakogTesta.Add(x.IdTesta.ToString()));

            OnPropertyChanged("SviPrijavljeniKursevi");
            OnPropertyChanged("IdSvihNastavnika");
            OnPropertyChanged("IDSvakogKursa");
            OnPropertyChanged("IDSvakogTesta");
        }


    }
}
