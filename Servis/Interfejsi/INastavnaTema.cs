using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Interfejsi
{
    public interface INastavnaTema
    {
        bool DodajNastavnuTemu(string naziv, List<Kurs> kursevi);

        bool IzmeniNastavnuTemu(int id, string naziv, List<Kurs> kursevi);

        bool ObrisiNastavnuTemu(int id);

        List<NastavnaTema> UcitajSveNastavneTeme();
    }
}
