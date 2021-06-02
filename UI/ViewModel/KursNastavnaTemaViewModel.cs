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
    public class KursNastavnaTemaViewModel:BindableBase
    {
        public static ObservableCollection<Sadrzi> SviKurseviITeme { get; set; } = new ObservableCollection<Sadrzi>();
        public static ObservableCollection<string> IDSvakogKursa { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakeTeme { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogKursaSadrzi { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakeTemeSadrzi { get; set; } = new ObservableCollection<string>();
        public MyICommand Kreiraj { get; set; }
        public MyICommand Izbrisi { get; set; }

        Servis.Servisi.KursNastavnaTemaServis Baza = new Servis.Servisi.KursNastavnaTemaServis();
        Servis.Servisi.KursServis BazaKurseva = new Servis.Servisi.KursServis();
        Servis.Servisi.NastavnaTemaServis BazaTema = new Servis.Servisi.NastavnaTemaServis();

        #region Properties
        private string kursDodavanje;

        public string KursDodavanje
        {
            get { return kursDodavanje; }
            set { kursDodavanje = value; OnPropertyChanged("KursDodavanje"); }
        }

        private string temaDodavanje;

        public string TemaDodavanje
        {
            get { return temaDodavanje; }
            set { temaDodavanje = value; OnPropertyChanged("TemaDodavanje"); }
        }

        private string kursBrisanje;

        public string KursBrisanje
        {
            get { return kursBrisanje; }
            set { kursBrisanje = value; OnPropertyChanged("KursBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }

        private string temaBrisanje;

        public string TemaBrisanje
        {
            get { return temaBrisanje; }
            set { temaBrisanje = value; OnPropertyChanged("TemaBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }



        #endregion

        public KursNastavnaTemaViewModel()
        {
            Kreiraj = new MyICommand(OnKreiraj);
            Izbrisi = new MyICommand(OnIzbrisi, CanDelete);

            DobaviPodatkeIzBaze();
        }


        
        private bool CanDelete() { return KursBrisanje != null && TemaBrisanje != null; }

        public void OnKreiraj()
        {

            try
            {
                if (!ValidacijaDodavanje()) return;

                Baza.Dodaj(int.Parse(KursDodavanje), int.Parse(TemaDodavanje));
                DobaviPodatkeIzBaze();

                KursDodavanje = null;
                TemaDodavanje = null;

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

                int idKursa = int.Parse(KursBrisanje);
                int idTema = int.Parse(TemaBrisanje);
                Baza.Obrisi(idKursa, idTema);
                DobaviPodatkeIzBaze();

            }
            catch
            {
                MessageBox.Show("Izaberite ID kursa i teme za brisanje!", "Greska", MessageBoxButton.OK);
            }

        }


        public void DobaviPodatkeIzBaze()
        {
            SviKurseviITeme.Clear();
            IDSvakogKursa.Clear();
            IDSvakeTeme.Clear();
            IDSvakogKursaSadrzi.Clear();
            IDSvakeTemeSadrzi.Clear();

            List<Sadrzi> KurseviTeme = Baza.UcitajSveKurseveINastavneTeme();
            foreach (var p in KurseviTeme)
            {
                
                SviKurseviITeme.Add(p);
                //da se ne dupliraju u combobox-u
                if(!IDSvakogKursaSadrzi.Contains(p.KursIdKursa.ToString()))
                    IDSvakogKursaSadrzi.Add(p.KursIdKursa.ToString());

                if(!IDSvakeTemeSadrzi.Contains(p.NastavnaTemaIdTeme.ToString()))
                    IDSvakeTemeSadrzi.Add(p.NastavnaTemaIdTeme.ToString());
                
            }

            BazaKurseva.UcitajSveKurseve().ForEach(x => IDSvakogKursa.Add(x.IdKursa.ToString()));
            BazaTema.UcitajSveNastavneTeme().ForEach(x => IDSvakeTeme.Add(x.IdTeme.ToString()));

            

            OnPropertyChanged("SviKurseviITeme");
            OnPropertyChanged("IDSvakogKursa");
            OnPropertyChanged("IDSvakeTeme");
            OnPropertyChanged("IDSvakogKursaSadrzi");
            OnPropertyChanged("IDSvakeTemeSadrzi");
        }

        public bool ValidacijaDodavanje()
        {
            if (string.IsNullOrEmpty(KursDodavanje) || string.IsNullOrWhiteSpace(KursDodavanje))
            {
                MessageBox.Show("Polje za Kurs id ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TemaDodavanje) || string.IsNullOrWhiteSpace(TemaDodavanje))
            {
                MessageBox.Show("Polje za Tema id ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {

                int kursID = int.Parse(KursDodavanje);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali kurs id!", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {

                int temaID = int.Parse(TemaDodavanje);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali tema id!", "Greska", MessageBoxButton.OK);
                return false;
            }


            return true;

        }
    }
}
