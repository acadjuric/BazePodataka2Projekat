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
    public class OdgovorViewModel : BindableBase
    {

        public static ObservableCollection<Odgovor> SviOdgovori { get; set; } = new ObservableCollection<Odgovor>();
        public static ObservableCollection<string> IDSvakogOdgovora { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> OpcijeZaTacanOdgovor { get; set; } = new ObservableCollection<string>() { "True", "False" };
        public static List<Pitanje> IzabranaPitanja { get; set; }
        public static List<Pitanje> IzabranaPitanjaDodavanje { get; set; }

        public MyICommand Kreiraj { get; set; }
        public MyICommand Izmeni { get; set; }
        public MyICommand Izbrisi { get; set; }
        public MyICommand DodajPitanja { get; set; }
        public MyICommand IzmeniPitanja { get; set; }

        Servis.Servisi.OdgovorServis Baza = new Servis.Servisi.OdgovorServis();
        Servis.Servisi.PitanjeServis BazaPitanja = new Servis.Servisi.PitanjeServis();
        Servis.Servisi.PitanjeOdgovorServis BazaPitanjaIOdgovora = new Servis.Servisi.PitanjeOdgovorServis();

        #region Properties

        private string tbTacan;

        public string TBTacan
        {
            get { return tbTacan; }
            set { tbTacan = value; OnPropertyChanged("TBTacan"); }
        }

        private string tbtekst;

        public string TBTekst
        {
            get { return tbtekst; }
            set { tbtekst = value; OnPropertyChanged("TBTekst"); }
        }

        private string index;

        public string Index
        {
            get { return index; }
            set { index = value; OnPropertyChanged("Index");
                Izmeni.RaiseCanExecuteChanged(); IzmeniPitanja.RaiseCanExecuteChanged();
                if(Index != null)
                {
                    DobaviOdgovorISveNjegoveAtributeIzBaze(int.Parse(Index), true);
                }
            }
        }

        private string index1;

        public string Index1
        {
            get { return index1; }
            set { index1 = value; OnPropertyChanged("Index1"); Izbrisi.RaiseCanExecuteChanged(); }
        }


        private bool CanEdit() { return Index != null; }
        private bool CanDelete() { return Index1 != null; }


        private string tbIzmenaTekst;

        public string TBIzmenaTekst
        {
            get { return tbIzmenaTekst; }
            set { tbIzmenaTekst = value; OnPropertyChanged("TBIzmenaTekst"); }
        }

        private string tbTacanIzmena;

        public string TBTacanIzmena
        {
            get { return tbTacanIzmena; }
            set { tbTacanIzmena = value; OnPropertyChanged("TBTacanIzmena"); }
        }

        #endregion

        int poslednjiIdOdgovoraZaKojiJeKliknutoIzmeniPitanja = -1;

        public OdgovorViewModel()
        {
            IzabranaPitanja = new List<Pitanje>();
            IzabranaPitanjaDodavanje = new List<Pitanje>();

            Kreiraj = new MyICommand(OnKreiraj);
            Izmeni = new MyICommand(OnIzmeni, CanEdit);
            Izbrisi = new MyICommand(OnIzbrisi, CanDelete);
            DodajPitanja = new MyICommand(OnDodajPitanja);
            IzmeniPitanja = new MyICommand(OnIzmeniPitanja, CanEdit);

            poslednjiIdOdgovoraZaKojiJeKliknutoIzmeniPitanja = -1;

            DobaviPodatkeIzBaze();
        }

        public void OnDodajPitanja()
        {
            Views.OdgovorPitanjeWindow prozor = new Views.OdgovorPitanjeWindow(false);
            prozor.ShowDialog();
        }

        public void OnIzmeniPitanja()
        {
            int id = int.Parse(Index);

            if (poslednjiIdOdgovoraZaKojiJeKliknutoIzmeniPitanja != id)
            {
                //IzabranaPitanja.Clear(); ubaceno u metodu
                DobaviOdgovorISveNjegoveAtributeIzBaze(id, false);
                //Odgovor o = Baza.VratiOdgovor(id);
                //if (o == null) return;

                //List<Poseduje> SvaPitanjaZaOdgovor = BazaPitanjaIOdgovora.VratiPitanjaZaOdgovor(id);
                //if (SvaPitanjaZaOdgovor != null)
                //{
                //    foreach (var poseduje in SvaPitanjaZaOdgovor)
                //    {
                //        Pitanje p = null;
                //        p = BazaPitanja.VratiPitanje(poseduje.PitanjeIdPitanja);
                //        if (p != null)
                //            IzabranaPitanja.Add(p);
                //    }
                //}
            }

            poslednjiIdOdgovoraZaKojiJeKliknutoIzmeniPitanja = id;
            Views.OdgovorPitanjeWindow prozor = new Views.OdgovorPitanjeWindow(true);
            prozor.ShowDialog();
        }

        private void DobaviOdgovorISveNjegoveAtributeIzBaze(int id,bool IndikatorZaPopunjavanjePoljaZaIzmenu)
        {
            Odgovor o = Baza.VratiOdgovor(id);
            if (o == null) return;

            IzabranaPitanja.Clear();

            if (IndikatorZaPopunjavanjePoljaZaIzmenu)
            {
                TBIzmenaTekst = o.Tekst;
                TBTacanIzmena = o.Tacan.ToString();
            }

            List<Poseduje> SvaPitanjaZaOdgovor = BazaPitanjaIOdgovora.VratiPitanjaZaOdgovor(id);
            if (SvaPitanjaZaOdgovor != null)
            {
                foreach (var poseduje in SvaPitanjaZaOdgovor)
                {
                    Pitanje p = null;
                    p = BazaPitanja.VratiPitanje(poseduje.PitanjeIdPitanja);
                    if (p != null)
                        IzabranaPitanja.Add(p);
                }
            }

        }

        public void OnIzmeni()
        {
            try
            {
                if (!ValidacijaIzmena()) return;
                //Index je zapravi ID iz comboBox-a
                int id = int.Parse(Index);

                Baza.IzmeniOdgovor(id, TBIzmenaTekst, bool.Parse(TBTacanIzmena), IzabranaPitanja);
                DobaviPodatkeIzBaze();

                TBIzmenaTekst = string.Empty;
                Index = null;
                TBTacanIzmena = null;
                IzabranaPitanja.Clear();
                poslednjiIdOdgovoraZaKojiJeKliknutoIzmeniPitanja = -1;

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
                //Index1 je zapravi ID iz comboBox-a
                int id = int.Parse(Index1);

                Baza.ObrisiOdgovor(id);
                DobaviPodatkeIzBaze();
            }
            catch
            {
                MessageBox.Show("Izaberite ID za brisanje!", "Greska", MessageBoxButton.OK);
            }
        }

        public void OnKreiraj()
        {
            try
            {
                if (!ValidacijaDodavanje()) return;
                Baza.DodajOdgovor(TBTekst, bool.Parse(TBTacan), IzabranaPitanjaDodavanje);
                DobaviPodatkeIzBaze();

                TBTekst = string.Empty;
                TBTacan = null;
                IzabranaPitanjaDodavanje.Clear();
            }
            catch (Exception e)
            {

            }
        }

        public void DobaviPodatkeIzBaze()
        {
            SviOdgovori.Clear();
            IDSvakogOdgovora.Clear();

            List<Odgovor> Odgovri = Baza.UcitajSveOdgovore();
            foreach (var o in Odgovri)
            {
                SviOdgovori.Add(o);
                IDSvakogOdgovora.Add(o.IdOdgovora.ToString());
            }

            OnPropertyChanged("IDSvakogOdgovora");
            OnPropertyChanged("SviOdgovori");

        }

        public bool ValidacijaDodavanje()
        {
            if (string.IsNullOrEmpty(TBTekst) || string.IsNullOrWhiteSpace(TBTekst))
            {
                MessageBox.Show("Polje za Tekst ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBTacan) || string.IsNullOrWhiteSpace(TBTacan))
            {
                MessageBox.Show("Polje za Tacan ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBTekst.Length < 2)
            {
                //odgovor 'da' ili 'ne'
                MessageBox.Show("Tekst mora imati minimum 2 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                bool tacan = bool.Parse(TBTacan);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali polje Tacan za odgovor!", "Greska", MessageBoxButton.OK);
                return false;
            }

            return true;

        }

        public bool ValidacijaIzmena()
        {
            if (string.IsNullOrEmpty(TBIzmenaTekst) || string.IsNullOrWhiteSpace(TBIzmenaTekst))
            {
                MessageBox.Show("Polje za Novi Tekst ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBTacanIzmena) || string.IsNullOrWhiteSpace(TBTacanIzmena))
            {
                MessageBox.Show("Polje za Izmena Tacan ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBIzmenaTekst.Length < 2)
            {
                //odgovor 'da' ili 'ne'
                MessageBox.Show("Novi Tekst mora imati minimum 2 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                bool tacan = bool.Parse(TBTacanIzmena);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali polje Izmena Tacan za odgovor!", "Greska", MessageBoxButton.OK);
                return false;
            }

            return true;
        }
    }
}
