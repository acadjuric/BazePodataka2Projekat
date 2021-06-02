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
    public class PitanjeOdgovorWindowViewModel:BindableBase
    {

        public ObservableCollection<CheckBoxModel> ListaOdgovora { get; set; }
        private List<Odgovor> SviOdgovori { get; set; }
        Servis.Servisi.OdgovorServis Baza = new Servis.Servisi.OdgovorServis();
        public MyICommand Zavrsi { get; set; }

        private bool? dialogResult;

        public bool? DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; OnPropertyChanged("DialogResult"); }
        }

        public bool Izmena = false;

        public PitanjeOdgovorWindowViewModel(bool izmena)
        {
            Izmena = izmena;

            SviOdgovori = new List<Odgovor>();
            ListaOdgovora = new ObservableCollection<CheckBoxModel>();
            Zavrsi = new MyICommand(OnZavrsi);
            SviOdgovori = Baza.UcitajSveOdgovore();
            if (Izmena)
            {
                SviOdgovori.ForEach(x =>
                    ListaOdgovora.Add(new CheckBoxModel()
                    {
                        Id = x.IdOdgovora,
                        Name = x.IdOdgovora.ToString() + " " + x.Tekst + " " + x.Tacan.ToString(),
                        IsChecked = PitanjeViewModel.IzabraniOdgovori.Exists(item => item.IdOdgovora == x.IdOdgovora),
                    }
                ));
            }
            else
            {
                SviOdgovori.ForEach(x =>
                   ListaOdgovora.Add(new CheckBoxModel()
                   {
                       Id = x.IdOdgovora,
                       Name = x.IdOdgovora.ToString() + " " + x.Tekst + " " + x.Tacan.ToString(),
                       IsChecked = PitanjeViewModel.IzabraniOdgovoriDodavanje.Exists(item => item.IdOdgovora == x.IdOdgovora),
                   }
               ));
            }

            OnPropertyChanged("ListaOdgovora");
        }

        public void OnZavrsi()
        {
            if (Izmena)
                PitanjeViewModel.IzabraniOdgovori.Clear();
            else
                PitanjeViewModel.IzabraniOdgovoriDodavanje.Clear();

            foreach (CheckBoxModel c in ListaOdgovora)
            {
                if (c.IsChecked)
                {
                    if(Izmena)
                        PitanjeViewModel.IzabraniOdgovori.Add(SviOdgovori.FirstOrDefault(x => x.IdOdgovora == c.Id));
                    else
                        PitanjeViewModel.IzabraniOdgovoriDodavanje.Add(SviOdgovori.FirstOrDefault(x => x.IdOdgovora == c.Id));
                }

            }
            DialogResult = false;

        }
    }
}
