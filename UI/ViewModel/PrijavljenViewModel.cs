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
    public class PrijavljenViewModel:BindableBase
    {

        public static ObservableCollection<Prijavljen> SviPrijavljeni { get; set; } = new ObservableCollection<Prijavljen>();
        public static ObservableCollection<string> IDSvakogKursa { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogUcenika { get; set; } = new ObservableCollection<string>();
        

        public static ObservableCollection<string> PrijavljeniIDKurseva { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> PrijavljeniIDUcenika { get; set; } = new ObservableCollection<string>();


        public MyICommand Kreiraj { get; set; }
        public MyICommand Izbrisi { get; set; }


        //bool UcenikIzmena = false, KursIzmena = false;


        Servis.Servisi.PrijavljenServis Baza = new Servis.Servisi.PrijavljenServis();
        Servis.Servisi.UcenikServis BazaUcenika = new Servis.Servisi.UcenikServis();
        Servis.Servisi.KursServis BazaKurseva = new Servis.Servisi.KursServis();

        #region Properties
        private string kursDodavanje;

        public string KursDodavanje
        {
            get { return kursDodavanje; }
            set { kursDodavanje = value; OnPropertyChanged("KursDodavanje"); }
        }

        private string ucenikDodavanje;

        public string UcenikDodavanje
        {
            get { return ucenikDodavanje; }
            set { ucenikDodavanje = value; OnPropertyChanged("UcenikDodavanje"); }
        }

        private string kursBrisanje;

        public string KursBrisanje
        {
            get { return kursBrisanje; }
            set { kursBrisanje = value; OnPropertyChanged("KursBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }

        private string ucenikBrisanje;

        public string UcenikBrisanje
        {
            get { return ucenikBrisanje; }
            set { ucenikBrisanje = value; OnPropertyChanged("UcenikBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }



        #endregion


        public PrijavljenViewModel()
        {
            Kreiraj = new MyICommand(OnKreiraj);
            Izbrisi = new MyICommand(OnIzbrisi, CanDelete);


            DobaviPodatkeIzBaze();

        }

        private bool CanDelete() { return UcenikBrisanje != null && KursBrisanje != null; }

        public void OnKreiraj()
        {

            try
            {
                if (!ValidacijaDodavanje()) return;

                Baza.DodajPrijavljen(int.Parse(KursDodavanje), int.Parse(UcenikDodavanje));
                DobaviPodatkeIzBaze();

                KursDodavanje = null;
                UcenikDodavanje = null;
                
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

                int idUcenika = int.Parse(UcenikBrisanje);
                int idKursa = int.Parse(KursBrisanje);
                Baza.ObrisiPrijavljen(idKursa, idUcenika);
                DobaviPodatkeIzBaze();

            }
            catch
            {
                MessageBox.Show("Izaberite ID kursa i ucenika za brisanje!", "Greska", MessageBoxButton.OK);
            }

        }


        public void DobaviPodatkeIzBaze()
        {
            SviPrijavljeni.Clear();
            IDSvakogKursa.Clear();
            IDSvakogUcenika.Clear();
            PrijavljeniIDUcenika.Clear();
            PrijavljeniIDKurseva.Clear();

            List<Prijavljen> Prijavljeni = Baza.UcitajSvePrijavljene();
            foreach (var t in Prijavljeni)
            {
                SviPrijavljeni.Add(t);
                // da ne duplira podatke u comboboxu, 1 ucenik na 2 kursa, bez ovoga bi se id ucenika
                //ponovio u combobox-u za brisanje...
                if( !PrijavljeniIDUcenika.Contains(t.UcenikIdKorisnika.ToString()) )
                    PrijavljeniIDUcenika.Add(t.UcenikIdKorisnika.ToString());

                if( !PrijavljeniIDKurseva.Contains(t.KursIdKursa.ToString()))
                    PrijavljeniIDKurseva.Add(t.KursIdKursa.ToString());
            }

            BazaUcenika.UcitajSveUcenike().ForEach(x => IDSvakogUcenika.Add(x.IdKorisnika.ToString()));
            BazaKurseva.UcitajSveKurseve().ForEach(x => IDSvakogKursa.Add(x.IdKursa.ToString()));

            OnPropertyChanged("SviPrijavljeni");
            OnPropertyChanged("IDSvakogKursa");
            OnPropertyChanged("IDSvakogUcenika");
            OnPropertyChanged("PrijavljeniIDUcenika");
            OnPropertyChanged("PrijavljeniIDKurseva");
        }



        public bool ValidacijaDodavanje()
        {
            if (string.IsNullOrEmpty(KursDodavanje) || string.IsNullOrWhiteSpace(KursDodavanje))
            {
                MessageBox.Show("Polje za Kurs id ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(UcenikDodavanje) || string.IsNullOrWhiteSpace(UcenikDodavanje))
            {
                MessageBox.Show("Polje za Ucenik id ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            try
            {
                int idKursa = int.Parse(KursDodavanje);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali Kurs id!", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                int idKorisnika = int.Parse(UcenikDodavanje);
            }
            catch
            {
                MessageBox.Show("Niste dobro izbarali Ucenik id!", "Greska", MessageBoxButton.OK);
                return false;
            }
            return true;

        }

    }
}
