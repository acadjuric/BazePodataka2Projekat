using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class UcenikServis : IUcenik
    {
        public UcenikServis()
        {

        }

        public bool DodajUcenika(string email, string sifra, string ime, string prezime, string razred)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Ucenik u = new Ucenik()
                {
                    IdKorisnika = IzgenerisiID(),
                    EmailAdresa = email,
                    Sifra = sifra,
                    Ime = ime,
                    Prezime = prezime,
                    Razred = razred,
                };

                db.Korisnici.Add(u);
                db.SaveChanges();
                return true;
            }
        }

        public bool IzmeniUcenika(int id, string email, string sifra, string ime, string prezime, string razred)
        {
            using (var db = new BazePodataka2ModelContainer())
            {

                Ucenik n = null;
                n = db.Korisnici_Ucenik.FirstOrDefault(x => x.IdKorisnika == id);
                if (n == null) return false;

                n.EmailAdresa = email;
                n.Sifra = sifra;
                n.Ime = ime;
                n.Prezime = prezime;
                n.Razred = razred;

                db.SaveChanges();
                return true;
            }
        }

        public bool ObrisiUcenika(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Korisnik n = null;
                n = db.Korisnici.FirstOrDefault(x => x.IdKorisnika == id);
                if (n == null) return false;

                PrijavljenServis prijavljenServis = new PrijavljenServis();
                var listaSvihPrijavljenih = prijavljenServis.UcitajSvePrijavljene();
                if(listaSvihPrijavljenih != null)
                {
                    foreach(var item in listaSvihPrijavljenih)
                    {
                        if(item.UcenikIdKorisnika == id)
                        {
                            prijavljenServis.ObrisiPrijavljen(item.KursIdKursa, id);
                        }
                    }
                }

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

                ObavestenjeServis obavestenjeServis = new ObavestenjeServis();
                obavestenjeServis.ObrisiSvaObavestenjaKreiranaOdKorisnika(id);

                db.Korisnici.Remove(n);
                db.SaveChanges();
                return true;
            }
        }

        public List<Ucenik> UcitajSveUcenike()
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                return db.Korisnici_Ucenik.ToList();
            }
        }

        private int IzgenerisiID()
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                int id = new Random().Next(1, 1000);
                Korisnik k = null;
                k = db.Korisnici.FirstOrDefault(x => x.IdKorisnika == id); ;

                while(k != null)
                {
                    id = new Random().Next(1, 1000);
                    k = db.Korisnici.FirstOrDefault(x => x.IdKorisnika == id); ;
                }

                return id;
            }
        }

        public Ucenik VratiUcenika(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Ucenik k = null;
                k = db.Korisnici_Ucenik.FirstOrDefault(x => x.IdKorisnika == id);
                return k;

            }
        }
    }
}
