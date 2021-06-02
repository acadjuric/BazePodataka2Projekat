using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface ITest
    {
        bool DodajTest(string naziv, DateTime datum,int bodovi, int kurs, List<Pitanje> pitanja);

        bool IzmeniTest(int id, string naziv, DateTime datum,int bodovi,int kurs, List<Pitanje> pitanja);

        bool ObrisiTest(int id);

        List<Test> UcitajSveTestove();

    }
}
