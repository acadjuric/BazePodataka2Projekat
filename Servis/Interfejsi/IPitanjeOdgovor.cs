using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface IPitanjeOdgovor
    {
        bool Dodaj(int pitanje, int odgovor);

        //bool Izmeni(int stariKurs, int staraTema, int noviKurs, int NovaTema);

        bool Obrisi(int pitanje, int odgovor);

        List<Poseduje> UcitajSvaPitanjaIOdgovore();

    }
}
