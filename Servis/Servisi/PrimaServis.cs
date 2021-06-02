using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class PrimaServis : IPrima
    {

        public PrimaServis()
        {

        }

        public bool DodajPrimljenoObavestenje(int idObavestenja, int idKorisnika, bool obrisano, DateTime datum)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Prima p = null;
                p = db.PrimljenaObavestenja.FirstOrDefault(x => x.KorisnikIdKorisnika == idKorisnika && x.ObavestenjeIdObavestenja == idObavestenja);
                if (p != null) return false;

                p = new Prima();
                p.ObavestenjeIdObavestenja = idObavestenja;
                p.KorisnikIdKorisnika = idKorisnika;
                p.Obrisano = obrisano;
                p.DatumCitanja = datum;

                db.PrimljenaObavestenja.Add(p);
                db.SaveChanges();
                return true;
            }
        }

        public bool IzmeniPrimljenoObavestenje(int idObavestenja, int idKorisnika, bool obrisano, DateTime datum)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Prima p = null;
                p = db.PrimljenaObavestenja.FirstOrDefault(x => x.KorisnikIdKorisnika == idKorisnika && x.ObavestenjeIdObavestenja == idObavestenja);
                if (p == null) return false;

                p.Obrisano = obrisano;
                p.DatumCitanja = datum;

                db.SaveChanges();
                return true;
            }
        }

        public bool ObrisiPrimljenoObavestenje(int idObavestenja, int idKorisnika)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Prima p = null;
                p = db.PrimljenaObavestenja.FirstOrDefault(x => x.KorisnikIdKorisnika == idKorisnika && x.ObavestenjeIdObavestenja == idObavestenja);
                if (p == null) return false;

                db.PrimljenaObavestenja.Remove(p);
                db.SaveChanges();
                return true;
            }
        }

        public List<Prima> UcitajSvaPrimljenaObavestenja()
        {
            using(var db= new BazePodataka2ModelContainer())
            {
                return db.PrimljenaObavestenja.ToList();
            }
        }

        public Prima VratiPrimljenoObavestenje(int korisnik,int obavestenje)
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                Prima p = null;
                p = db.PrimljenaObavestenja.FirstOrDefault(x => x.KorisnikIdKorisnika == korisnik && x.ObavestenjeIdObavestenja == obavestenje);
                return p;
            }
        }
    }
}
