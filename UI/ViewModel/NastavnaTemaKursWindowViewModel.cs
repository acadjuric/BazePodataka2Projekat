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
    public class NastavnaTemaKursWindowViewModel:BindableBase
    {

        public ObservableCollection<CheckBoxModel> ListaKurseva { get; set; }
        private List<Kurs> SviKursevi { get; set; }
        Servis.Servisi.KursServis Baza = new Servis.Servisi.KursServis();
        public MyICommand Zavrsi { get; set; }


        private bool? dialogResult;

        public bool? DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; OnPropertyChanged("DialogResult"); }
        }

        public bool Izmena = false;

        public NastavnaTemaKursWindowViewModel(bool izmena)
        {
            Izmena = izmena;
            SviKursevi = new List<Kurs>();
            ListaKurseva = new ObservableCollection<CheckBoxModel>();
            Zavrsi = new MyICommand(OnZavrsi);
            SviKursevi = Baza.UcitajSveKurseve();
            if (Izmena)
            {
                SviKursevi.ForEach(x =>
                    ListaKurseva.Add(new CheckBoxModel()
                    {
                        Id = x.IdKursa,
                        Name = x.IdKursa.ToString() + " " + x.Naziv,
                        IsChecked = NastavnaTemaViewModel.IzabraniKursevi.Exists(item => item.IdKursa == x.IdKursa),
                    }
                ));
            }
            else
            {
                SviKursevi.ForEach(x =>
                    ListaKurseva.Add(new CheckBoxModel()
                    {
                        Id = x.IdKursa,
                        Name = x.IdKursa.ToString() + " " + x.Naziv,
                        IsChecked = NastavnaTemaViewModel.IzabraniKurseviDodavanje.Exists(item => item.IdKursa == x.IdKursa),
                    }
                ));

            }

            OnPropertyChanged("ListaKurseva");

        }

        public void OnZavrsi()
        {
            if (Izmena)
                NastavnaTemaViewModel.IzabraniKursevi.Clear();
            else
                NastavnaTemaViewModel.IzabraniKurseviDodavanje.Clear();

            foreach (CheckBoxModel c in ListaKurseva)
            {
                if (c.IsChecked)
                {
                    if(Izmena)
                        NastavnaTemaViewModel.IzabraniKursevi.Add(SviKursevi.FirstOrDefault(x => x.IdKursa == c.Id));
                    else
                        NastavnaTemaViewModel.IzabraniKurseviDodavanje.Add(SviKursevi.FirstOrDefault(x => x.IdKursa == c.Id));
                }

            }
            DialogResult = false;

        }

    }
}
