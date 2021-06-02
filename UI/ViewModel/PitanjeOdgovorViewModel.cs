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
    public class PitanjeOdgovorViewModel:BindableBase
    {
        public static ObservableCollection<Poseduje> SvaPitanjaIOdgovori { get; set; } = new ObservableCollection<Poseduje>();
        public static ObservableCollection<string> IDSvakogPitanja { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogOdgovora { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogPitanjaPoseduje { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IDSvakogOdgovoraPoseduje { get; set; } = new ObservableCollection<string>();
        public MyICommand Kreiraj { get; set; }
        public MyICommand Izbrisi { get; set; }

        Servis.Servisi.PitanjeOdgovorServis Baza = new Servis.Servisi.PitanjeOdgovorServis();
        Servis.Servisi.PitanjeServis BazaPitanja = new Servis.Servisi.PitanjeServis();
        Servis.Servisi.OdgovorServis BazaOdgovora = new Servis.Servisi.OdgovorServis();

        #region Properties
        private string pitanjeDodavanje;

        public string PitanjeDodavanje
        {
            get { return pitanjeDodavanje; }
            set { pitanjeDodavanje = value; OnPropertyChanged("PitanjeDodavanje"); }
        }

        private string odgovorDodavanje;

        public string OdgovorDodavanje
        {
            get { return odgovorDodavanje; }
            set { odgovorDodavanje = value; OnPropertyChanged("OdgovorDodavanje"); }
        }

        private string pitanjeBrisanje;

        public string PitanjeBrisanje
        {
            get { return pitanjeBrisanje; }
            set { pitanjeBrisanje = value; OnPropertyChanged("PitanjeBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }

        private string odgovorBrisanje;

        public string OdgovorBrisanje
        {
            get { return odgovorBrisanje; }
            set { odgovorBrisanje = value; OnPropertyChanged("OdgovorBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }



        #endregion

        public PitanjeOdgovorViewModel()
        {
            Kreiraj = new MyICommand(OnKreiraj);
            Izbrisi = new MyICommand(OnIzbrisi, CanDelete);

            DobaviPodatkeIzBaze();
        }

        private bool CanDelete() { return PitanjeBrisanje != null && OdgovorBrisanje != null; }

        public void OnKreiraj()
        {

            try
            {
                if (!ValidacijaDodavanje()) return;

                Baza.Dodaj(int.Parse(PitanjeDodavanje), int.Parse(OdgovorDodavanje));
                DobaviPodatkeIzBaze();

                PitanjeDodavanje = null;
                OdgovorDodavanje = null;
               

            }
            catch (Exception e)
            {

            }
        }



        public void OnIzbrisi()
        {
            try
            {

                int idPitanja = int.Parse(PitanjeBrisanje);
                int idOdgovora = int.Parse(OdgovorBrisanje);
                Baza.Obrisi(idPitanja, idOdgovora);
                DobaviPodatkeIzBaze();

            }
            catch
            {
                MessageBox.Show("Izaberite ID kursa i teme za brisanje!", "Greska", MessageBoxButton.OK);
            }

        }


        public void DobaviPodatkeIzBaze()
        {
            SvaPitanjaIOdgovori.Clear();
            IDSvakogPitanja.Clear();
            IDSvakogOdgovora.Clear();
            IDSvakogPitanjaPoseduje.Clear();
            IDSvakogOdgovoraPoseduje.Clear();

            List<Poseduje> PitanjaOdgovori = Baza.UcitajSvaPitanjaIOdgovore();
            foreach (var p in PitanjaOdgovori)
            {

                SvaPitanjaIOdgovori.Add(p);
                //da se ne dupliraju u combobox-u
                if (!IDSvakogPitanjaPoseduje.Contains(p.PitanjeIdPitanja.ToString()))
                    IDSvakogPitanjaPoseduje.Add(p.PitanjeIdPitanja.ToString());

                if (!IDSvakogOdgovoraPoseduje.Contains(p.OdgovorIdOdgovora.ToString()))
                    IDSvakogOdgovoraPoseduje.Add(p.OdgovorIdOdgovora.ToString());

            }

            BazaPitanja.UcitajSvaPitanja().ForEach(x => IDSvakogPitanja.Add(x.IdPitanja.ToString()));
            BazaOdgovora.UcitajSveOdgovore().ForEach(x => IDSvakogOdgovora.Add(x.IdOdgovora.ToString()));



            OnPropertyChanged("SvaPitanjaIOdgovori");
            OnPropertyChanged("IDSvakogPitanja");
            OnPropertyChanged("IDSvakogOdgovora");
            OnPropertyChanged("IDSvakogPitanjaPoseduje");
            OnPropertyChanged("IDSvakogOdgovoraPoseduje");
        }


        public bool ValidacijaDodavanje()
        {
            if (string.IsNullOrEmpty(PitanjeDodavanje) || string.IsNullOrWhiteSpace(PitanjeDodavanje))
            {
                MessageBox.Show("Polje za Pitanje id ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(OdgovorDodavanje) || string.IsNullOrWhiteSpace(OdgovorDodavanje))
            {
                MessageBox.Show("Polje za Odgovor id ne sme biti prazno", "Greska", MessageBoxButton.OK);
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

                int idOdgovora = int.Parse(OdgovorDodavanje);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali odgovor id!", "Greska", MessageBoxButton.OK);
                return false;
            }


            return true;

        }

    }
}
