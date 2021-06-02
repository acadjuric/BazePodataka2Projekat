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
    public class PrimaViewModel:BindableBase
    {

        public static ObservableCollection<Prima> SvaPrimljenaObavestenja { get; set; } = new ObservableCollection<Prima>();
        public static ObservableCollection<string> IDSvakogObavestenja { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> IdSvakogKorisnika { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> OpcijeZaObrisano { get; set; } = new ObservableCollection<string>() { "True", "False" };

        public static ObservableCollection<string> PrimljenaObavestenjaIDObavestenja { get; set; } = new ObservableCollection<string>();
        public static ObservableCollection<string> PrimljenaObavestenjaIDKorisnika { get; set; } = new ObservableCollection<string>();


        public MyICommand Kreiraj { get; set; }
        public MyICommand Izmeni { get; set; }
        public MyICommand Izbrisi { get; set; }


        Servis.Servisi.PrimaServis Baza = new Servis.Servisi.PrimaServis();
        Servis.Servisi.NastavnikServis BazaNastavnika = new Servis.Servisi.NastavnikServis();
        Servis.Servisi.UcenikServis BazaUcenika = new Servis.Servisi.UcenikServis();
        Servis.Servisi.ObavestenjeServis BazaObavestenja = new Servis.Servisi.ObavestenjeServis();

        #region Properties
        private string izabranoObavestenje;

        public string IzabranoObavestenje
        {
            get { return izabranoObavestenje; }
            set { izabranoObavestenje = value; OnPropertyChanged("IzabranoObavestenje"); }
        }

        private string izabraniKorisnik;

        public string IzabraniKorisnik
        {
            get { return izabraniKorisnik; }
            set { izabraniKorisnik = value; OnPropertyChanged("IzabraniKorisnik"); }
        }

        private DateTime? datum;

        public DateTime? Datum
        {
            get { return datum; }
            set { datum = value; OnPropertyChanged("Datum"); }
        }

        private string obirsano;

        public string Obrisano
        {
            get { return obirsano; }
            set { obirsano = value; OnPropertyChanged("Obrisano"); }
        }

        private string izabranoObavestenjeBrisanje;

        public string IzabranoObavestenjeBrisanje
        {
            get { return izabranoObavestenjeBrisanje; }
            set { izabranoObavestenjeBrisanje = value; OnPropertyChanged("IzabranoObavestenjeBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }

        private string izabraniKorisnikBrisanje;

        public string IzabraniKorisnikBrisanje
        {
            get { return izabraniKorisnikBrisanje; }
            set { izabraniKorisnikBrisanje = value; OnPropertyChanged("IzabraniKorisnikBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }

        private string izabranoObavestenjeIzmena;

        public string IzabranoObavestenjeIzmena
        {
            get { return izabranoObavestenjeIzmena; }
            set { izabranoObavestenjeIzmena = value; OnPropertyChanged("IzabranoObavestenjeIzmena"); Izmeni.RaiseCanExecuteChanged();  }
        }

        private string izabraniKorisnikIzmena;

        public string IzabraniKorisnikIzmena
        {
            get { return izabraniKorisnikIzmena; }
            set { izabraniKorisnikIzmena = value; OnPropertyChanged("IzabraniKorisnikIzmena"); Izmeni.RaiseCanExecuteChanged(); }
        }

        private DateTime? datumIzmena;

        public DateTime? DatumIzmena
        {
            get { return datumIzmena; }
            set { datumIzmena = value; OnPropertyChanged("DatumIzmena"); }
        }

        private string obirsanoIzmena;

        public string ObrisanoIzmena
        {
            get { return obirsanoIzmena; }
            set { obirsanoIzmena = value; OnPropertyChanged("ObrisanoIzmena"); }
        }



        #endregion

        public PrimaViewModel()
        {
            Kreiraj = new MyICommand(OnKreiraj);
            Izmeni = new MyICommand(OnIzmeni, CanEdit);
            Izbrisi = new MyICommand(OnIzbrisi, CanDelete);


            DobaviPodatkeIzBaze();
        }

        private bool CanEdit() {
            if(IzabraniKorisnikIzmena != null && IzabranoObavestenjeIzmena != null)
            {
                Prima p = Baza.VratiPrimljenoObavestenje(int.Parse(izabraniKorisnikIzmena), int.Parse(IzabranoObavestenjeIzmena));
                if(p != null)
                {
                    DatumIzmena = p.DatumCitanja;
                    ObrisanoIzmena = p.Obrisano.ToString();
                    return true;
                }
            }
            return false;
            //return IzabraniKorisnikIzmena != null && IzabranoObavestenjeIzmena != null;
        }
        private bool CanDelete() { return izabraniKorisnikBrisanje != null && IzabranoObavestenjeBrisanje != null; }

        public void OnKreiraj()
        {

            try
            {
                if (!ValidacijaDodavanje()) return;

                Baza.DodajPrimljenoObavestenje(int.Parse(IzabranoObavestenje), int.Parse(IzabraniKorisnik),bool.Parse(Obrisano),(DateTime)Datum);
                DobaviPodatkeIzBaze();

                IzabraniKorisnik = null;
                IzabranoObavestenje = null;
                Obrisano = null;
                Datum = null;
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


                Baza.IzmeniPrimljenoObavestenje(int.Parse(IzabranoObavestenjeIzmena), int.Parse(IzabraniKorisnikIzmena), bool.Parse(ObrisanoIzmena), (DateTime)DatumIzmena);
                DobaviPodatkeIzBaze();

                IzabraniKorisnikIzmena = null;
                IzabranoObavestenjeIzmena = null;
                ObrisanoIzmena = null;
                DatumIzmena = null;
            }
            catch
            {
                MessageBox.Show("Izaberite ID za staru temu i stari kurs!", "Greska", MessageBoxButton.OK);
            }

        }

        public void OnIzbrisi()
        {
            try
            {

                int idKorisnika = int.Parse(IzabraniKorisnikBrisanje);
                int idObavestenja = int.Parse(IzabranoObavestenjeBrisanje);
                Baza.ObrisiPrimljenoObavestenje(idObavestenja, idKorisnika);
                DobaviPodatkeIzBaze();

            }
            catch
            {
                MessageBox.Show("Izaberite ID kursa i teme za brisanje!", "Greska", MessageBoxButton.OK);
            }

        }

        public void DobaviPodatkeIzBaze()
        {
            SvaPrimljenaObavestenja.Clear();
            IDSvakogObavestenja.Clear();
            IdSvakogKorisnika.Clear();
            PrimljenaObavestenjaIDKorisnika.Clear();
            PrimljenaObavestenjaIDObavestenja.Clear();

            List<Prima> Obavestenja = Baza.UcitajSvaPrimljenaObavestenja();
            foreach (var t in Obavestenja)
            {
                SvaPrimljenaObavestenja.Add(t);
                //takodje da ne duplira podatke u combobox-ovima
                if( !PrimljenaObavestenjaIDKorisnika.Contains(t.KorisnikIdKorisnika.ToString()) )
                    PrimljenaObavestenjaIDKorisnika.Add(t.KorisnikIdKorisnika.ToString());

                if( !PrimljenaObavestenjaIDObavestenja.Contains(t.ObavestenjeIdObavestenja.ToString()) )
                    PrimljenaObavestenjaIDObavestenja.Add(t.ObavestenjeIdObavestenja.ToString());
            }

            BazaObavestenja.UcitajSvaObavestenja().ForEach(x => IDSvakogObavestenja.Add(x.IdObavestenja.ToString()));
            BazaUcenika.UcitajSveUcenike().ForEach(x => IdSvakogKorisnika.Add(x.IdKorisnika.ToString()));
            BazaNastavnika.UcitajSveNastavnike().ForEach(x => IdSvakogKorisnika.Add(x.IdKorisnika.ToString()));

            OnPropertyChanged("SvaPrimljenaObavestenja");
            OnPropertyChanged("IDSvakogObavestenja");
            OnPropertyChanged("IdSvakogKorisnika");
            OnPropertyChanged("PrimljenaObavestenjaIDObavestenja");
            OnPropertyChanged("PrimljenaObavestenjaIDKorisnika");

        }


        public bool ValidacijaDodavanje()
        {
            if (string.IsNullOrEmpty(IzabranoObavestenje) || string.IsNullOrWhiteSpace(IzabranoObavestenje))
            {
                MessageBox.Show("Polje za Obavestenje id ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(IzabraniKorisnik) || string.IsNullOrWhiteSpace(IzabraniKorisnik))
            {
                MessageBox.Show("Polje za Korisnik id ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (Datum == null)
            {
                MessageBox.Show("Polje za DATUM citanja ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (Datum.Value == DateTime.MinValue)
            {
                MessageBox.Show("Polje za DATUM citanja nije validno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(Obrisano) || string.IsNullOrWhiteSpace(Obrisano))
            {
                MessageBox.Show("Polje za Obrisano ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                int idObavestenj = int.Parse(IzabranoObavestenje);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali Obavestenje id!", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                int idKorisnika = int.Parse(IzabraniKorisnik);
            }
            catch
            {
                MessageBox.Show("Niste dobro izbarali Korisnik id!", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                bool tacno = bool.Parse(Obrisano);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali vrednost za polje 'Obrisano' !", "Greska", MessageBoxButton.OK);
                return false;
            }

            return true;

        }

        public bool ValidacijaIzmena()
        {
            if (string.IsNullOrEmpty(IzabranoObavestenjeIzmena) || string.IsNullOrWhiteSpace(IzabranoObavestenjeIzmena))
            {
                MessageBox.Show("Polje za Obavestenje id kod IZMENE ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(IzabraniKorisnikIzmena) || string.IsNullOrWhiteSpace(IzabraniKorisnikIzmena))
            {
                MessageBox.Show("Polje za Korisnik id kod IZMENE ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (DatumIzmena == null)
            {
                MessageBox.Show("Polje za DATUM citanja kod IZMENE ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (DatumIzmena.Value == DateTime.MinValue)
            {
                MessageBox.Show("Polje za DATUM citanja kod IZMENE nije validno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(ObrisanoIzmena) || string.IsNullOrWhiteSpace(ObrisanoIzmena))
            {
                MessageBox.Show("Polje za Obrisano kod IZMENE ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                int idObavestenj = int.Parse(IzabranoObavestenjeIzmena);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali Obavestenje id kod IZMENE!", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                int idKorisnika = int.Parse(IzabraniKorisnikIzmena);
            }
            catch
            {
                MessageBox.Show("Niste dobro izbarali Korisnik id kod IZMENE!", "Greska", MessageBoxButton.OK);
                return false;
            }

            try
            {
                bool tacno = bool.Parse(ObrisanoIzmena);
            }
            catch
            {
                MessageBox.Show("Niste dobro izabrali vrednost za polje 'Obrisano' kod IZMENE !", "Greska", MessageBoxButton.OK);
                return false;
            }

            return true;
        }


    }
}
