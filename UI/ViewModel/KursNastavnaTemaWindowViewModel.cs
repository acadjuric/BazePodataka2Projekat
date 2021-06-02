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
    public class KursNastavnaTemaWindowViewModel:BindableBase
    {

        public ObservableCollection<CheckBoxModel> ListaNastavnihTema { get; set; }
        private List<NastavnaTema> SveNastavneTeme { get; set; }
        Servis.Servisi.NastavnaTemaServis Baza = new Servis.Servisi.NastavnaTemaServis();
        public MyICommand Zavrsi { get; set; }
        

        private bool? dialogResult;

        public bool? DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; OnPropertyChanged("DialogResult"); }
        }

        bool Izmena = false;

        public KursNastavnaTemaWindowViewModel(bool izmena)
        {
            Izmena = izmena;
            SveNastavneTeme = new List<NastavnaTema>();
            ListaNastavnihTema = new ObservableCollection<CheckBoxModel>();
            Zavrsi = new MyICommand(OnZavrsi);
            SveNastavneTeme = Baza.UcitajSveNastavneTeme();
            if (izmena)
            {
                SveNastavneTeme.ForEach(x =>
                    ListaNastavnihTema.Add(new CheckBoxModel()
                    {
                        Id = x.IdTeme,
                        Name = x.IdTeme.ToString() + " " + x.Naziv,
                        IsChecked = KursViewModel.IzabraneTemeZaKurs.Exists(item => item.IdTeme == x.IdTeme),

                    }
                ));
            }
            else
            {
                SveNastavneTeme.ForEach(x =>
                    ListaNastavnihTema.Add(new CheckBoxModel()
                    {
                        Id = x.IdTeme,
                        Name = x.IdTeme.ToString() + " " + x.Naziv,
                        IsChecked = KursViewModel.IzabraneTemeZaKursDodavanje.Exists(item => item.IdTeme == x.IdTeme),

                    }
                ));
            }
            
            OnPropertyChanged("ListaNastavnihTema");
        }

        public void OnZavrsi()
        {
            if (Izmena)
                KursViewModel.IzabraneTemeZaKurs.Clear();
            else
                KursViewModel.IzabraneTemeZaKursDodavanje.Clear();

            foreach(CheckBoxModel c in ListaNastavnihTema)
            {
                if (c.IsChecked)
                {
                    if (Izmena)
                        KursViewModel.IzabraneTemeZaKurs.Add(SveNastavneTeme.FirstOrDefault(x => x.IdTeme == c.Id));
                    else
                        KursViewModel.IzabraneTemeZaKursDodavanje.Add(SveNastavneTeme.FirstOrDefault(x => x.IdTeme == c.Id));
                }

            }
            DialogResult = false;
            
        }
    }
}
