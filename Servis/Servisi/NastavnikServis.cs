using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class NastavnikServis : INastavnik
    {
        public NastavnikServis()
        {

        }
        public bool DodajNastavnika(string email, string sifra, string ime, string prezime,string zvanje)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Nastavnik n = new Nastavnik()
                {
                    IdKorisnika = IzgenerisiID(),
                    EmailAdresa = email,
                    Sifra = sifra,
                    Ime = ime,
                    Prezime = prezime,
                    Zvanje = zvanje,
                };

                db.Korisnici.Add(n);
                db.SaveChanges();
                return true;
            }
        }

        public bool IzmeniNastavnika(int id, string email, string sifra, string ime, string prezime,string zvanje)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                
                Nastavnik n = null;
                n = db.Korisnici_Nastavnik.FirstOrDefault(x => x.IdKorisnika == id);
                if (n ==null) return false;
                
                n.EmailAdresa = email;
                n.Sifra = sifra;
                n.Ime = ime;
                n.Prezime = prezime;
                n.Zvanje = zvanje;
                
                db.SaveChanges();
                return true;
            }
        }

        public bool ObrisiNastavnika(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Korisnik n = null;
                n = db.Korisnici.FirstOrDefault(x => x.IdKorisnika == id);
                if (n == null) return false;

                KursServis kursServis = new KursServis();
                kursServis.ObrisiSveKurseveZaNastavnika(id);

                ObavestenjeServis obavestenjeServis = new ObavestenjeServis();
                obavestenjeServis.ObrisiSvaObavestenjaKreiranaOdKorisnika(id);

                PrimaServis primaServis = new PrimaServis();
                var listaSvihPrimljenihObavestenja = primaServis.UcitajSvaPrimljenaObavestenja();
                if(listaSvihPrimljenihObavestenja != null)
                {
                    foreach(var item in listaSvihPrimljenihObavestenja)
                    {
                        if(item.KorisnikIdKorisnika == id)
                        {
                            primaServis.ObrisiPrimljenoObavestenje(item.ObavestenjeIdObavestenja, id);
                        }
                    }
                }

                db.Korisnici.Remove(n);
                db.SaveChanges();
                return true;
            }
        }

        public List<Nastavnik> UcitajSveNastavnike()
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                return db.Korisnici_Nastavnik.ToList();
            }
        }


        private int IzgenerisiID()
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                int id = new Random().Next(1, 1000);
                Korisnik k = null;
                k = db.Korisnici.FirstOrDefault(x => x.IdKorisnika == id); ;

                while (k != null)
                {
                    id = new Random().Next(1, 1000);
                    k = db.Korisnici.FirstOrDefault(x => x.IdKorisnika == id); ;
                }

                return id;
            }
        }

        public Nastavnik VratiNastavnika(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Nastavnik k = null;
                k = db.Korisnici_Nastavnik.FirstOrDefault(x => x.IdKorisnika == id);
                return k;

            }
        }
    }
}
