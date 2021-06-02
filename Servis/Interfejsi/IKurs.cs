using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface IKurs
    {

        bool DodajKurs(string naziv, int poeni, int nastavnik, List<NastavnaTema> nastavneTeme);

        bool IzmeniKurs(int id, string naziv, int poeni, int nastavnik,List<NastavnaTema> nastavneTeme);

        bool ObrisiKurs(int id);

        List<Kurs> UcitajSveKurseve();
    }
}
