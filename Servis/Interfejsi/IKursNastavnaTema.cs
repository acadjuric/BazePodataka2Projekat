using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface IKursNastavnaTema
    {
        bool Dodaj(int kurs, int tema);

        //bool Izmeni(int stariKurs, int staraTema, int noviKurs, int NovaTema);

        bool Obrisi(int kurs,int tema);

        List<Sadrzi> UcitajSveKurseveINastavneTeme();
    }
}
