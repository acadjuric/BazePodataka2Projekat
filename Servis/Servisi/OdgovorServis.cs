using Servis.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Servisi
{
    public class OdgovorServis : IOdgovor
    {
        

        public OdgovorServis()
        {
            
        }

        public bool DodajOdgovor(string tekst,bool tacan, List<Pitanje> pitanja)
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                Odgovor o = new Odgovor()
                {
                    IdOdgovora = IzgenerisiID(),
                    Tacan = tacan,
                    Tekst = tekst
                };

                db.Odgovori.Add(o);
                db.SaveChanges();

                MapirajUTabeluPosedujeOdgovorNaPitanje(o, pitanja);

                return true;

            }
        }

        public bool IzmeniOdgovor(int id, string tekst, bool tacan, List<Pitanje> pitanja)
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                Odgovor o = null;
                o = db.Odgovori.FirstOrDefault(x => x.IdOdgovora == id);

                if (o == null) return false;

                o.Tekst = tekst;
                o.Tacan = tacan;

                db.SaveChanges();

                MapirajUTabeluPosedujeOdgovorNaPitanje(o, pitanja);
                return true;
            }
        }

        public bool ObrisiOdgovor(int id)
        {
            using(var db = new BazePodataka2ModelContainer())
            {
                Odgovor o = null;
                o = db.Odgovori.FirstOrDefault(x => x.IdOdgovora == id);
                if (o == null) return false;

                PitanjeOdgovorServis servis = new PitanjeOdgovorServis();
                var lista = servis.VratiPitanjaZaOdgovor(id);
                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        servis.Obrisi(item.PitanjeIdPitanja, id);
                    }
                }

                db.Odgovori.Remove(o);
                db.SaveChanges();
                return true;
            }
        }

        public List<Odgovor> UcitajSveOdgovore()
        {
            using(var db  = new BazePodataka2ModelContainer())
            {
                return db.Odgovori.ToList();
            }
        }


        private int IzgenerisiID()
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                int id = new Random().Next(1, 1000);
                Odgovor k = null;
                k = db.Odgovori.FirstOrDefault(x => x.IdOdgovora == id); ;

                while (k != null)
                {
                    id = new Random().Next(1, 1000);
                    k = db.Odgovori.FirstOrDefault(x => x.IdOdgovora == id); ;
                }

                return id;
            }
        }

        public Odgovor VratiOdgovor(int id)
        {
            using (var db = new BazePodataka2ModelContainer())
            {
                Odgovor k = null;
                k = db.Odgovori.FirstOrDefault(x => x.IdOdgovora == id);
                return k;

            }
        }

        private void MapirajUTabeluPosedujeOdgovorNaPitanje(Odgovor o, List<Pitanje> pitanja)
        {
            if (pitanja.Count != 0)
            {
                PitanjeOdgovorServis servis = new PitanjeOdgovorServis();
                List<Poseduje> svaPitanjaZaOdgovor = servis.VratiPitanjaZaOdgovor(o.IdOdgovora);
                foreach (var poseduje in svaPitanjaZaOdgovor)
                {
                    //Provera da li postoje u bazi neke koje su bile cekirane na ui
                    // pa ih je korisnik prilikom izmene uklonio -> treba da ih uklonim iz baze
                    if (!pitanja.Exists(x => x.IdPitanja == poseduje.PitanjeIdPitanja))
                    {
                        servis.Obrisi(poseduje.PitanjeIdPitanja, o.IdOdgovora);
                    }
                }

                foreach (var pitanje in pitanja)
                {
                    //u dodaj imas proveru da li vec postoji, tako da je obezbedjeno da ce biti svaki par jedinstven
                    servis.Dodaj(pitanje.IdPitanja, o.IdOdgovora);
                }
            }
        }
    }
}
