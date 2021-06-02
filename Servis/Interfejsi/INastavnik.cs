using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    interface INastavnik
    {

        bool DodajNastavnika(string email, string sifra, string ime, string prezime, string zvanje);

        bool IzmeniNastavnika(int id,string email, string sifra, string ime, string prezime, string zvanje);

        bool ObrisiNastavnika(int id);

        List<Nastavnik> UcitajSveNastavnike();
    }
}
