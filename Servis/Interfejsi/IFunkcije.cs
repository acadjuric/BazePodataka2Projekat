using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface IFunkcije
    {
        int FuncBrojTestovaZaKurs(int kursId);

        int FuncBrojSvihObavestenjaZaKorisnika(int korisnikId);

        int FuncBrojUcenikaKojiPolazuIzabraniKursITest(int kursId, int testId);
    }
}
