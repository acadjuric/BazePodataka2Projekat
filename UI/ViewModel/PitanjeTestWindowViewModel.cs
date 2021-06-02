using Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Helpers;

namespace UI.ViewModel
{
    public class PitanjeTestWindowViewModel:BindableBase
    {
        public ObservableCollection<CheckBoxModel> ListaTestova { get; set; }
        private List<Test> SviTestovi { get; set; }
        Servis.Servisi.TestServis Baza = new Servis.Servisi.TestServis();
        public MyICommand Zavrsi { get; set; }

        private bool? dialogResult;

        public bool? DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; OnPropertyChanged("DialogResult"); }
        }

        public bool Izmeni = false;

        public PitanjeTestWindowViewModel(bool izmeni)
        {
            Izmeni = izmeni;

            SviTestovi = new List<Test>();
            ListaTestova = new ObservableCollection<CheckBoxModel>();
            Zavrsi = new MyICommand(OnZavrsi);
            SviTestovi = Baza.UcitajSveTestove();
            if (Izmeni)
            {
                SviTestovi.ForEach(x =>
                    ListaTestova.Add(new CheckBoxModel()
                    {
                        Id = x.IdTesta,
                        Name = x.IdTesta.ToString() + " " + x.Naziv + " " + x.Datum.ToString(),
                        IsChecked = PitanjeViewModel.IzabraniTestovi.Exists(item => item.IdTesta == x.IdTesta),
                    }
                ));
            }
            else
            {
                SviTestovi.ForEach(x =>
                    ListaTestova.Add(new CheckBoxModel()
                    {
                        Id = x.IdTesta,
                        Name = x.IdTesta.ToString() + " " + x.Naziv + " " + x.Datum.ToString(),
                        IsChecked = PitanjeViewModel.IzabraniTestoviDodavanje.Exists(item => item.IdTesta == x.IdTesta),
                    }
                ));
            }

            OnPropertyChanged("ListaTestova");

        }

        public void OnZavrsi()
        {
            if (Izmeni)
                PitanjeViewModel.IzabraniTestovi.Clear();
            else
                PitanjeViewModel.IzabraniTestoviDodavanje.Clear();

            foreach (CheckBoxModel c in ListaTestova)
            {
                if (c.IsChecked)
                {
                    if(Izmeni)
                        PitanjeViewModel.IzabraniTestovi.Add(SviTestovi.FirstOrDefault(x => x.IdTesta == c.Id));
                    else
                        PitanjeViewModel.IzabraniTestoviDodavanje.Add(SviTestovi.FirstOrDefault(x => x.IdTesta == c.Id));
                }

            }
            DialogResult = false;

        }

    }
}
