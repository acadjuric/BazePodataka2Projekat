using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Helpers;

namespace UI.ViewModel
{
    class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> NavCommand { get; private set; }

        private ObavestenjaViewModel obavestenjaViewModel = new ObavestenjaViewModel();
        private NastavnaTemaViewModel nastavnaTemaViewModel = new NastavnaTemaViewModel();
        private KursViewModel kursViewModel = new KursViewModel();
        private TestViewModel testViewModel = new TestViewModel();
        private PitanjeViewModel pitanjeViewModel = new PitanjeViewModel();
        private OdgovorViewModel odgovorViewModel = new OdgovorViewModel();
        private NastavnikViewModel nastavnikViewModel = new NastavnikViewModel();
        private UcenikViewModel ucenikViewModel = new UcenikViewModel();
        private BindableBase currentViewModel;

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }


        public MainWindowViewModel()
        {
            currentViewModel = obavestenjaViewModel;
            NavCommand = new MyICommand<string>(OnNav);
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "obavestenje":
                    CurrentViewModel = new ObavestenjaViewModel();// obavestenjaViewModel;
                    break;
                case "kurs":
                    CurrentViewModel = new KursViewModel();//kursViewModel;
                    break;
                case "nastavnaTema":
                    CurrentViewModel = new NastavnaTemaViewModel();//nastavnaTemaViewModel;
                    break;
                case "test":
                    CurrentViewModel = new TestViewModel(); //testViewModel;
                    break;
                case "pitanje":
                    CurrentViewModel = new PitanjeViewModel();//pitanjeViewModel;
                    break;
                case "odgovor":
                    CurrentViewModel = new OdgovorViewModel();//odgovorViewModel;
                    break;
                case "nastavnik":
                    CurrentViewModel = new NastavnikViewModel(); //nastavnikViewModel;
                    break;
                case "ucenik":
                    CurrentViewModel = new UcenikViewModel();//ucenikViewModel;
                    break;
                case "kursNastavnaTema":
                    CurrentViewModel = new KursNastavnaTemaViewModel();
                    break;
                case "prima":
                    CurrentViewModel = new PrimaViewModel();
                    break;
                case "prijavljen":
                    CurrentViewModel = new PrijavljenViewModel();
                    break;
                case "pitanjaOdgovori":
                    CurrentViewModel = new PitanjeOdgovorViewModel();
                    break;
                case "testoviPitanja":
                    CurrentViewModel = new TestPitanjeViewModel();
                    break;
                case "prijavljeniTest":
                    CurrentViewModel = new PrijavljenTestViewModel();
                    break;
                default:
                    CurrentViewModel = new ObavestenjaViewModel(); //obavestenjaViewModel;
                    break;
            }

        }
    }
}
