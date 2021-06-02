using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface ITestPitanje
    {
        bool Dodaj(int pitanje, int test);

        //bool Izmeni(int stariKurs, int staraTema, int noviKurs, int NovaTema);

        bool Obrisi(int pitanje, int test);

        List<Sastoji> UcitajSvaPitanjaITestove();
    }
}
