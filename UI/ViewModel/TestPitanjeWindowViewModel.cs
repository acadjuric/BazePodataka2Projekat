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
    public class TestPitanjeWindowViewModel:BindableBase
    {
        public ObservableCollection<CheckBoxModel> ListaPitanja { get; set; }
        private List<Pitanje> SvaPitanja { get; set; }
        Servis.Servisi.PitanjeServis Baza = new Servis.Servisi.PitanjeServis();
        public MyICommand Zavrsi { get; set; }

        private bool? dialogResult;

        public bool? DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; OnPropertyChanged("DialogResult"); }
        }

        public bool Izmena = false;

        public TestPitanjeWindowViewModel(bool izmena)
        {
            Izmena = izmena;
            SvaPitanja = new List<Pitanje>();
            ListaPitanja = new ObservableCollection<CheckBoxModel>();
            Zavrsi = new MyICommand(OnZavrsi);
            SvaPitanja = Baza.UcitajSvaPitanja();
            if (Izmena)
            {
                SvaPitanja.ForEach(x =>
                    ListaPitanja.Add(new CheckBoxModel()
                    {
                        Id = x.IdPitanja,
                        Name = x.IdPitanja.ToString() + " " + x.Tekst + " " + x.Nivo,
                        IsChecked = TestViewModel.IzabranaPitanjaZaTest.Exists(item => item.IdPitanja == x.IdPitanja),
                    }
                ));
            }
            else
            {
                SvaPitanja.ForEach(x =>
                   ListaPitanja.Add(new CheckBoxModel()
                   {
                       Id = x.IdPitanja,
                       Name = x.IdPitanja.ToString() + " " + x.Tekst + " " + x.Nivo,
                       IsChecked = TestViewModel.IzabranaPitanjaZaTestDodavanje.Exists(item => item.IdPitanja == x.IdPitanja),
                   }
               ));
            }

            OnPropertyChanged("ListaPitanja");

        }

        public void OnZavrsi()
        {
            if (Izmena)
                TestViewModel.IzabranaPitanjaZaTest.Clear();
            else
                TestViewModel.IzabranaPitanjaZaTestDodavanje.Clear();

            foreach (CheckBoxModel c in ListaPitanja)
            {
                if (c.IsChecked)
                {
                    if (Izmena)
                        TestViewModel.IzabranaPitanjaZaTest.Add(SvaPitanja.FirstOrDefault(x => x.IdPitanja == c.Id));
                    else
                        TestViewModel.IzabranaPitanjaZaTestDodavanje.Add(SvaPitanja.FirstOrDefault(x => x.IdPitanja == c.Id));
                }

            }
            DialogResult = false;

        }
    }
}
