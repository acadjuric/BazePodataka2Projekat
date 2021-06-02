using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface IPrijavljen
    {

        bool DodajPrijavljen(int idKursa, int idUcenika);

        bool IzmeniPrijavljen(int idKursa, int idUcenika, int noviIdKursa, int noviIdUcenika);

        bool ObrisiPrijavljen(int idKursa, int idUcenika);

        List<Prijavljen> UcitajSvePrijavljene();
    }
}
