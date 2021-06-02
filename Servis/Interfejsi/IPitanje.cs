using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface IPitanje
    {

        bool DodajPitanje(string tekst, int nivo, int bodovi, List<Odgovor> odgovori, List<Test> testovi);

        bool IzmeniPitanje(int id, string tekst,int nivo, int bodovi, List<Odgovor> odgovori, List<Test> testovi);

        bool ObrisiPitanje(int id);

        List<Pitanje> UcitajSvaPitanja();
    }
}
