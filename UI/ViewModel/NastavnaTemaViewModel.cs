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
    public class NastavnaTemaViewModel:BindableBase
    {
        
        public static ObservableCollection<NastavnaTema> SveTeme { get; set; } = new ObservableCollection<NastavnaTema>();
        public static ObservableCollection<string> IDSvakeTeme { get; set; } = new ObservableCollection<string>();
        public static List<Kurs> IzabraniKursevi { get; set; }
        public static List<Kurs> IzabraniKurseviDodavanje { get; set; }
        public MyICommand Kreiraj { get; set; }
        public MyICommand Izmeni { get; set; }
        public MyICommand Izbrisi { get; set; }
        public MyICommand IzmeniKurseve { get; set; }
        public MyICommand DodajKurseve { get; set; }

        Servis.Servisi.NastavnaTemaServis Baza = new Servis.Servisi.NastavnaTemaServis();
        Servis.Servisi.KursNastavnaTemaServis BazaKursNastavnaTema = new Servis.Servisi.KursNastavnaTemaServis();
        Servis.Servisi.KursServis BazaKurseva = new Servis.Servisi.KursServis();

        #region Properties
        private string tbNaziv;

        public string TBNaziv
        {
            get { return tbNaziv; }
            set { tbNaziv = value; OnPropertyChanged("TBNaziv"); }
        }
        
        private string tbNazivIzmena;

        public string TBNazivIzmena
        {
            get { return tbNazivIzmena; }
            set { tbNazivIzmena = value; OnPropertyChanged("TBNazivIzmena"); }
        }


        private string idIzmena;

        public string IdIzmena
        {
            get { return idIzmena; }
            set { idIzmena = value; OnPropertyChanged("IdIzmena");
                IzmeniKurseve.RaiseCanExecuteChanged() ; Izmeni.RaiseCanExecuteChanged();
                if(IdIzmena != null)
                {
                    DobaviNastavnuTemuISveNjeneAtribute(int.Parse(IdIzmena), true);
                }
            }
        }

        private string idBrisanje;

        public string IdBrisanje
        {
            get { return idBrisanje; }
            set { idBrisanje = value; OnPropertyChanged("IdBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }

        #endregion

        int poslednjiIdNastavneTemeZaKojuJeKliknutoIzmeniKurseve = -1;

        public NastavnaTemaViewModel()
        {
            IzabraniKurseviDodavanje = new List<Kurs>();
            IzabraniKursevi = new List<Kurs>();
            Kreiraj = new MyICommand(OnKreiraj);
            Izmeni = new MyICommand(OnIzmeni, CanEdit);
            Izbrisi = new MyICommand(OnIzbrisi, CanDelete);
            IzmeniKurseve = new MyICommand(OnIzmeniKurseve, CanEdit);
            DodajKurseve = new MyICommand(OnDodajKurseve);

            poslednjiIdNastavneTemeZaKojuJeKliknutoIzmeniKurseve = -1;

            DobaviPodatkeIzBaze();

            

        }

        public void OnDodajKurseve()
        {
            Views.NastavnaTemaKursWindow prozor = new Views.NastavnaTemaKursWindow(false);
            prozor.ShowDialog();
        }

        public void OnIzmeniKurseve()
        {
            int id = int.Parse(IdIzmena);

            if (poslednjiIdNastavneTemeZaKojuJeKliknutoIzmeniKurseve != id)
            {
                //IzabraniKursevi.Clear(); ubaceno u metodu
                DobaviNastavnuTemuISveNjeneAtribute(id, false);
                //NastavnaTema t = Baza.VratiNastavnuTemu(id);
                //if (t == null) return;

                //List<Sadrzi> SviKurseviZaTemu = BazaKursNastavnaTema.VratiKurseveZaTemu(id);
                //if(SviKurseviZaTemu != null)
                //{
                //    foreach(var sadrzi in SviKurseviZaTemu)
                //    {
                //        Kurs k = null;
                //        k = BazaKurseva.VratiKurs(sadrzi.KursIdKursa);
                //        if (k != null)
                //            IzabraniKursevi.Add(k);
                //    }
                //}
            }

            poslednjiIdNastavneTemeZaKojuJeKliknutoIzmeniKurseve = id;
            Views.NastavnaTemaKursWindow prozor = new Views.NastavnaTemaKursWindow(true);
            prozor.ShowDialog();
        }

        private void DobaviNastavnuTemuISveNjeneAtribute(int id, bool IndikatorZaPopunjavanjePoljaKodIzmene)
        {
            
            NastavnaTema t = Baza.VratiNastavnuTemu(id);
            if (t == null) return;

            IzabraniKursevi.Clear();

            if (IndikatorZaPopunjavanjePoljaKodIzmene)
            {
                TBNazivIzmena = t.Naziv;
            }

            List<Sadrzi> SviKurseviZaTemu = BazaKursNastavnaTema.VratiKurseveZaTemu(id);
            if (SviKurseviZaTemu != null)
            {
                foreach (var sadrzi in SviKurseviZaTemu)
                {
                    Kurs k = null;
                    k = BazaKurseva.VratiKurs(sadrzi.KursIdKursa);
                    if (k != null)
                        IzabraniKursevi.Add(k);
                }
            }
        }

        private bool CanEdit() { return IdIzmena != null; }
        private bool CanDelete() { return IdBrisanje != null; }

        public void OnKreiraj()
        {
            
            try
            {
                if (!ValidacijaDodavanje()) return;

                Baza.DodajNastavnuTemu(TBNaziv,IzabraniKurseviDodavanje);
                DobaviPodatkeIzBaze();
                
                TBNaziv = string.Empty;
                IzabraniKurseviDodavanje.Clear();
            }
            catch(Exception e)
            {

            }
        }

        public void OnIzmeni()
        {
            try
            {
                if (!ValidacijaIzmena()) return;

                int id = int.Parse(IdIzmena);


                Baza.IzmeniNastavnuTemu(id, TBNazivIzmena,IzabraniKursevi);
                DobaviPodatkeIzBaze();

                TBNazivIzmena = string.Empty;
                IdIzmena = null;
                IzabraniKursevi.Clear();
                poslednjiIdNastavneTemeZaKojuJeKliknutoIzmeniKurseve = -1;
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

                int id = int.Parse(IdBrisanje);
                Baza.ObrisiNastavnuTemu(id);
                DobaviPodatkeIzBaze();
              
            }
            catch
            {
                MessageBox.Show("Izaberite ID za brisanje!", "Greska", MessageBoxButton.OK);
            }

        }


        public void DobaviPodatkeIzBaze()
        {
            SveTeme.Clear();
            IDSvakeTeme.Clear();

            List<NastavnaTema> Teme = Baza.UcitajSveNastavneTeme();
            foreach (var p in Teme)
            {
                SveTeme.Add(p);
                IDSvakeTeme.Add(p.IdTeme.ToString());
            }

            OnPropertyChanged("IDSvakeTeme");
            OnPropertyChanged("SveTeme");

        }


        public bool ValidacijaDodavanje()
        {
            if (string.IsNullOrEmpty(TBNaziv) || string.IsNullOrWhiteSpace(TBNaziv))
            {
                MessageBox.Show("Polje za Naziv ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            
            if (TBNaziv.Length < 3)
            {
                
                MessageBox.Show("Naziv mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
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

            if (TBNazivIzmena.Length < 3)
            {

                MessageBox.Show("Novi Naziv mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }

            return true;
        }
    }
}
