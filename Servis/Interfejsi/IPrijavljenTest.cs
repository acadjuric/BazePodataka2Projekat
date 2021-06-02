using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface IPrijavljenTest
    {
        bool Dodaj(int kurs, int ucenik, int test);

        //bool Izmeni(int stariKurs, int staraTema, int noviKurs, int NovaTema);

        bool Obrisi(int kurs,int ucenik, int test);

        List<Polaze> UcitajSvePrijavljeneITestove();

    }
}
