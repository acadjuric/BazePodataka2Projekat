using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class PrijavljenServis : IPrijavljen
    {

        public PrijavljenServis()
        {

        }

        public bool DodajPrijavljen(int idKursa, int idUcenika)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Prijavljen p = null;
                p = db.Prijavljeni.FirstOrDefault(x => x.KursIdKursa == idKursa && x.UcenikIdKorisnika == idUcenika);
                if (p != null) return false;

                p = new Prijavljen();
                p.KursIdKursa = idKursa;
                p.UcenikIdKorisnika = idUcenika;

                db.Prijavljeni.Add(p);
                db.SaveChanges();
                return true;
            }
        }

        public bool IzmeniPrijavljen(int idKursa, int idUcenika, int noviIdKursa, int noviIdUcenika)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Prijavljen p = null;
                p = db.Prijavljeni.FirstOrDefault(x => x.KursIdKursa == idKursa && x.UcenikIdKorisnika == idUcenika);
                if (p == null) return false;

                //ne moze oba da menja, jer je to zapravo dodavanje onda,
                if(noviIdUcenika == -1)
                    p.KursIdKursa = noviIdKursa;
                if(noviIdKursa == -1)
                    p.UcenikIdKorisnika = noviIdUcenika;
                
                db.SaveChanges();
                return true;
            }
        }

        public bool ObrisiPrijavljen(int idKursa, int idUcenika)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Prijavljen p = null;
                p = db.Prijavljeni.FirstOrDefault(x => x.KursIdKursa == idKursa && x.UcenikIdKorisnika == idUcenika);
                if (p == null) return false;

                PrijavljenTestServis prijavljenTestServis = new PrijavljenTestServis();
                var listaTestova = prijavljenTestServis.VratiTestZaPrijavljene(idKursa, idUcenika);
                if(listaTestova != null)
                {
                    listaTestova.ForEach(item => prijavljenTestServis.Obrisi(idKursa, idUcenika, item.TestIdTesta));
                }

                db.Prijavljeni.Remove(p);
                db.SaveChanges();
                return true;
            }
        }

        public List<Prijavljen> UcitajSvePrijavljene()
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                return db.Prijavljeni.ToList();
            }
        }
    }
}
