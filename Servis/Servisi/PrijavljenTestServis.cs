using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class PrijavljenTestServis : IPrijavljenTest
    {
        public PrijavljenTestServis()
        {

        }

        public bool Dodaj(int kurs, int ucenik, int test)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Polaze p = null;
                p = db.Polazes.FirstOrDefault(x => x.PrijavljenKursIdKursa == kurs && x.PrijavljenUcenikIdKorisnika == ucenik &&
                x.TestIdTesta == test);
                if (p != null) return false;

                p = new Polaze();
                p.PrijavljenKursIdKursa = kurs;
                p.PrijavljenUcenikIdKorisnika = ucenik;
                p.TestIdTesta = test;

                db.Polazes.Add(p);
                db.SaveChanges();

                return true;
            }
        }

        public bool Obrisi(int kurs, int ucenik, int test)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Polaze p = null;
                p = db.Polazes.FirstOrDefault(x => x.PrijavljenKursIdKursa == kurs && x.PrijavljenUcenikIdKorisnika == ucenik &&
                x.TestIdTesta == test);
                if (p == null) return false;

                db.Polazes.Remove(p);
                db.SaveChanges();
                return true;
            }

        }

        public List<Polaze> UcitajSvePrijavljeneITestove()
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                return db.Polazes.ToList();
            }
        }

        public List<Polaze> VratiPrijavljeneZaTest(int test)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                return db.Polazes.Where(x => x.TestIdTesta == test).ToList();
            }
        }

        public List<Polaze> VratiTestZaPrijavljene(int kurs, int ucenik)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                return db.Polazes.Where(x => x.PrijavljenKursIdKursa == kurs && x.PrijavljenUcenikIdKorisnika == ucenik).ToList();
            }
        }
    }
}
