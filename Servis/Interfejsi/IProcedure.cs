using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servis.Pomocne_Klase;
namespace Servis.Interfejsi
{
    public interface IProcedure
    {

        List<ProcUcenikVidiPitanjaZaTest> ProcUcenikVidiPitanjaZaTest(int idKursa, int idTesta);

        List<ProcNastavnikoveNastavneTeme> ProcNastavnikoveNastavneTeme(int idNastavnika);

        List<ProcImePrezimeZvanjeNastavnikaZaKurs> ProcImePrezimeNastavnikaZaKurs(int idKurs);
    }
}
