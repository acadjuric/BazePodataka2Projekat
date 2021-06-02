using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface IObavestenje
    {

        bool DodajObavestenje(string naslov, string tekst, DateTime datum, int korisnik);

        bool IzmeniObavestenje(int id, string naslov, string tekst, DateTime datum, int korisnik );

        bool ObrisiObavestenje(int id);

        List<Obavestenje> UcitajSvaObavestenja();
    }
}
