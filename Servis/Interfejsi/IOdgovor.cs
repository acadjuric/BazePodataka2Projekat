using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface IOdgovor
    {
        bool DodajOdgovor(string tekst, bool tacan, List<Pitanje> pitanja);

        bool IzmeniOdgovor(int id, string tekst, bool tacan, List<Pitanje> pitanja);

        bool ObrisiOdgovor(int id);

        List<Odgovor> UcitajSveOdgovore();

    }
}
