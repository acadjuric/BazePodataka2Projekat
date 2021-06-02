using Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using UI.Helpers;

namespace UI.ViewModel
{
    public class UcenikViewModel:BindableBase
    {
        
        public static ObservableCollection<Ucenik> SviUcenici { get; set; } = new ObservableCollection<Ucenik>();
        public static ObservableCollection<string> EmailSvakogUcenika { get; set; } = new ObservableCollection<string>();
        public MyICommand Kreiraj { get; set; }
        public MyICommand Izmeni { get; set; }
        public MyICommand Izbrisi { get; set; }

        Servis.Servisi.UcenikServis Baza = new Servis.Servisi.UcenikServis();


        #region Properties
        private string tbemail;

        public string TBEmail
        {
            get { return tbemail; }
            set { tbemail = value; OnPropertyChanged("TBEmail"); }
        }

        private string tbsifra;

        public string TBSifra
        {
            get { return tbsifra; }
            set { tbsifra = value; OnPropertyChanged("TBSifra"); }
        }

        private string tbime;

        public string TBIme
        {
            get { return tbime; }
            set { tbime = value; OnPropertyChanged("TBIme"); }
        }

        private string tbprezime;

        public string TBPrezime
        {
            get { return tbprezime; }
            set { tbprezime = value; OnPropertyChanged("TBPrezime"); }
        }

        private string tbrazred;

        public string TBRazred
        {
            get { return tbrazred; }
            set { tbrazred = value; OnPropertyChanged("TBRazred"); }
        }



        private string tbemailIzmena;

        public string TBEmailIzmena
        {
            get { return tbemailIzmena; }
            set { tbemailIzmena = value; OnPropertyChanged("TBEmailIzmena"); }
        }

        private string tbsifraIzmena;

        public string TBSifraIzmena
        {
            get { return tbsifraIzmena; }
            set { tbsifraIzmena = value; OnPropertyChanged("TBSifraIzmena"); }
        }

        private string tbimeIzmena;

        public string TBImeIzmena
        {
            get { return tbimeIzmena; }
            set { tbimeIzmena = value; OnPropertyChanged("TBImeIzmena"); }
        }

        private string tbprezimeIzmena;

        public string TBPrezimeIzmena
        {
            get { return tbprezimeIzmena; }
            set { tbprezimeIzmena = value; OnPropertyChanged("TBPrezimeIzmena"); }
        }

        private string tbrazredIzmena;

        public string TBRazredIzmena
        {
            get { return tbrazredIzmena; }
            set { tbrazredIzmena = value; OnPropertyChanged("TBRazredIzmena"); }
        }




        private string emailIzmena;

        public string EmailIzmena
        {
            get { return emailIzmena; }
            set { emailIzmena = value; OnPropertyChanged("EmailIzmena"); Izmeni.RaiseCanExecuteChanged();
                if (EmailIzmena != null)
                {
                    Ucenik n = null;
                    n = Baza.VratiUcenika(int.Parse(EmailIzmena));
                    if (n != null)
                    {
                        TBEmailIzmena = n.EmailAdresa;
                        TBSifraIzmena = n.Sifra;
                        TBImeIzmena = n.Ime;
                        TBPrezimeIzmena = n.Prezime;
                        TBRazredIzmena = n.Razred;
                    }

                }
            }
        }

        private string emailBrisanje;

        public string EmailBrisanje
        {
            get { return emailBrisanje; }
            set { emailBrisanje = value; OnPropertyChanged("EmailBrisanje"); Izbrisi.RaiseCanExecuteChanged(); }
        }

        #endregion

        public UcenikViewModel()
        {
            Kreiraj = new MyICommand(OnKreiraj);
            Izmeni = new MyICommand(OnIzmeni, CanEdit);
            Izbrisi = new MyICommand(OnIzbrisi, CanDelete);


            DobaviPodatkeIzBaze();
        }

        private bool CanEdit() { return EmailIzmena != null; }
        private bool CanDelete() { return EmailBrisanje != null; }


        public void OnKreiraj()
        {
        
            try
            {
                if (!ValidacijaDodavanje()) return;

                Baza.DodajUcenika(TBEmail, TBSifra, TBIme, TBPrezime, TBRazred);
                DobaviPodatkeIzBaze();
                TBEmail = string.Empty;
                TBSifra = string.Empty;
                TBIme = string.Empty;
                TBPrezime = string.Empty;
                TBRazred = string.Empty;
            }
            catch(Exception e)
            {

            }
        }

        public void OnIzmeni()
        {
            try
            {
                //ovde treba validacija

                if (!ValidacijaIzmena()) return;

                int id = int.Parse(EmailIzmena);
                Baza.IzmeniUcenika(id, TBEmailIzmena, TBSifraIzmena, TBImeIzmena, TBPrezimeIzmena, TBRazredIzmena);
                DobaviPodatkeIzBaze();

                TBEmailIzmena = string.Empty;
                TBSifraIzmena = string.Empty;
                TBImeIzmena = string.Empty;
                TBPrezimeIzmena = string.Empty;
                TBRazredIzmena = string.Empty;
                EmailIzmena = null;
            }
            catch
            {
                MessageBox.Show("Izaberite Email za izmenu!", "Greska", MessageBoxButton.OK);
            }

        }

        public void OnIzbrisi()
        {
            try
            {
                int id = int.Parse(EmailBrisanje);
                Baza.ObrisiUcenika(id);
                DobaviPodatkeIzBaze();

               
            }
            catch
            {
                MessageBox.Show("Izaberite Email za brisanje!", "Greska", MessageBoxButton.OK);
            }

        }

        public void DobaviPodatkeIzBaze()
        {
            SviUcenici.Clear();
            EmailSvakogUcenika.Clear();

            List<Ucenik> Ucenici = Baza.UcitajSveUcenike();
            foreach (var p in Ucenici)
            {
                SviUcenici.Add(p);
                EmailSvakogUcenika.Add(p.IdKorisnika.ToString());
            }

            OnPropertyChanged("EmailSvakogUcenika");
            OnPropertyChanged("SviUcenici");

        }

        public bool ValidacijaDodavanje()
        {
            if(string.IsNullOrEmpty(TBEmail) || string.IsNullOrWhiteSpace(TBEmail))
            {
                MessageBox.Show("Polje za EMAIL ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBSifra) || string.IsNullOrWhiteSpace(TBSifra))
            {
                MessageBox.Show("Polje za SIFRU ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBIme) || string.IsNullOrWhiteSpace(TBIme))
            {
                MessageBox.Show("Polje za IME ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBPrezime) || string.IsNullOrWhiteSpace(TBPrezime))
            {
                MessageBox.Show("Polje za PREZIME ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBRazred) || string.IsNullOrWhiteSpace(TBRazred))
            {
                MessageBox.Show("Polje za RAZRED ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if(TBSifra.Length < 3)
            {
                MessageBox.Show("Sifra mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBIme.Length < 3)
            {
                MessageBox.Show("Ime mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
                return false;
                
            }
            if (TBPrezime.Length < 3)
            {
                MessageBox.Show("Prezime mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }
            

            // '^[^\\s@]+@[^\\s@]+\\.[^\\s@]{2,}$'  patter za email
            Regex r = new Regex("^[^\\s@]+@[^\\s@]+\\.[^\\s@]{2,}$");
            if (!r.IsMatch(TBEmail))
            {
                MessageBox.Show("Email nije u validnom formatu email", "Greska", MessageBoxButton.OK);
                return false;
            }
            Regex r1 = new Regex("\\d+");
            if (r1.IsMatch(TBIme))
            {
                MessageBox.Show("Ime ne sme da sadrzi brojeve", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (r1.IsMatch(TBPrezime))
            {
                MessageBox.Show("Prezime ne sme da sadrzi brojeve", "Greska", MessageBoxButton.OK);
                return false;
            }
            return true;

        }

        public bool ValidacijaIzmena()
        {
            if (string.IsNullOrEmpty(TBEmailIzmena) || string.IsNullOrWhiteSpace(TBEmailIzmena))
            {
                MessageBox.Show("Polje za Novi Email ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBSifraIzmena) || string.IsNullOrWhiteSpace(TBSifraIzmena))
            {
                MessageBox.Show("Polje za Nova Sifra ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBImeIzmena) || string.IsNullOrWhiteSpace(TBImeIzmena))
            {
                MessageBox.Show("Polje za Novo Ime ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBPrezimeIzmena) || string.IsNullOrWhiteSpace(TBPrezimeIzmena))
            {
                MessageBox.Show("Polje za Novo Prezime ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(TBRazredIzmena) || string.IsNullOrWhiteSpace(TBRazredIzmena))
            {
                MessageBox.Show("Polje za Novi Razred ne sme biti prazno", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBSifraIzmena.Length < 3)
            {
                MessageBox.Show("Nova Sifra mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBImeIzmena.Length < 3)
            {
                MessageBox.Show("Novo Ime mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
                return false;

            }
            if (TBPrezimeIzmena.Length < 3)
            {
                MessageBox.Show("Novo Prezime mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (TBEmailIzmena.Length < 3)
            {
                MessageBox.Show("Novi Email mora imati minimum 3 karaktera", "Greska", MessageBoxButton.OK);
                return false;
            }

            Regex r = new Regex("^[^\\s@]+@[^\\s@]+\\.[^\\s@]{2,}$");
            if (!r.IsMatch(TBEmailIzmena))
            {
                MessageBox.Show("Email nije u validnom formatu email", "Greska", MessageBoxButton.OK);
                return false;
            }

            Regex r1 = new Regex("\\d+");
            if (r1.IsMatch(TBImeIzmena))
            {
                MessageBox.Show("Ime izmena ne sme da sadrzi brojeve", "Greska", MessageBoxButton.OK);
                return false;
            }
            if (r1.IsMatch(TBPrezimeIzmena))
            {
                MessageBox.Show("Prezime izmena ne sme da sadrzi brojeve", "Greska", MessageBoxButton.OK);
                return false;
            }
            return true;
        }
    }
}
