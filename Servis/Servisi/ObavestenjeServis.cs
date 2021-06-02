using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class ObavestenjeServis:IObavestenje
    {
        public ObavestenjeServis()
        {

        }

        public bool DodajObavestenje(string naslov, string tekst, DateTime datum, int korisnik)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Obavestenje o = new Obavestenje()
                {
                    IdObavestenja= IzgenerisiID(),
                    Naslov = naslov,
                    Tekst = tekst,
                    DatumSlanja = datum,
                    KorisnikIdKorisnika = korisnik,
                };

                db.Obavestenja.Add(o);
                db.SaveChanges();
                return true;
            }
        }

        public bool IzmeniObavestenje(int id, string naslov, string tekst, DateTime datum, int korisnik)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Obavestenje o = null;
                o = db.Obavestenja.FirstOrDefault(x => x.IdObavestenja == id);
                if (o == null) return false;

                o.Naslov = naslov;
                o.Tekst = tekst;
                o.DatumSlanja = datum;
                o.KorisnikIdKorisnika = korisnik;
                db.SaveChanges();
                return true;
            }
        }

        public bool ObrisiObavestenje(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Obavestenje o = null;
                o = db.Obavestenja.FirstOrDefault(x => x.IdObavestenja == id);
                if (o == null) return false;

                PrimaServis primaServis = new PrimaServis();
                var listaSvihPoslatihObavestenja = primaServis.UcitajSvaPrimljenaObavestenja();
                if(listaSvihPoslatihObavestenja != null)
                {
                    foreach (var item in listaSvihPoslatihObavestenja)
                    {
                        if(item.ObavestenjeIdObavestenja == id)
                        {
                            primaServis.ObrisiPrimljenoObavestenje(id, item.KorisnikIdKorisnika);
                        }
                    }
                }

                db.Obavestenja.Remove(o);
                db.SaveChanges();
                return true;
            }
        }

        public List<Obavestenje> UcitajSvaObavestenja()
        {
            using(var db= new BazePodataka2ModelContainer())
            {
                return db.Obavestenja.ToList();
            }
        }


        private int IzgenerisiID()
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                int id = new Random().Next(1, 1000);
                Obavestenje k = null;
                k = db.Obavestenja.FirstOrDefault(x => x.IdObavestenja == id); ;

                while (k != null)
                {
                    id = new Random().Next(1, 1000);
                    k = db.Obavestenja.FirstOrDefault(x => x.IdObavestenja == id); ;
                }

                return id;
            }
        }

        public Obavestenje VratiObavestenje(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Obavestenje k = null;
                k = db.Obavestenja.FirstOrDefault(x => x.IdObavestenja == id);
                return k;

            }
        }

        public void ObrisiSvaObavestenjaKreiranaOdKorisnika(int idKorisnika)
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                while(db.Obavestenja.Any(x=> x.KorisnikIdKorisnika == idKorisnika))
                {
                    ObrisiObavestenje(db.Obavestenja.First(x => x.KorisnikIdKorisnika == idKorisnika).IdObavestenja);
                }
            }
        }
    }
}
