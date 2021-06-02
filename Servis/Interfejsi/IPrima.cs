using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface IPrima
    {

        bool DodajPrimljenoObavestenje(int idObavestenja, int idKorisnika, bool obrisano, DateTime datum);

        bool IzmeniPrimljenoObavestenje(int idObavestenja, int idKorisnika, bool obrisano, DateTime datum);

        bool ObrisiPrimljenoObavestenje(int idObavestenja, int idKorisnika);

        List<Prima> UcitajSvaPrimljenaObavestenja();
    }
}
