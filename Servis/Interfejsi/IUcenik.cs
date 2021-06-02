using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface IUcenik
    {
        bool DodajUcenika(string email, string sifra, string ime, string prezime, string razred);

        bool IzmeniUcenika(int id, string email, string sifra, string ime, string prezime, string razred);

        bool ObrisiUcenika(int id);

        List<Ucenik> UcitajSveUcenike();
    }
}
