﻿using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using TPDDSGrupo44.Models;
using System.Data.Entity.Spatial;

namespace TPDDSGrupo44.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View(recuperarBusquedas());
        }

        public ActionResult SearchPerDay()
        {

            return View(recuperarBusquedas());
        }


        public ActionResult ResultsPerSearch()
        {
            return View(recuperarBusquedas());
        }


        private List<Busqueda> recuperarBusquedas()
        {
            List<Busqueda> busquedas;
            using (var db = new BuscAR())
            {
                busquedas = (from b in db.Busquedas
                             orderby b.Id
                             select b).ToList();
            }

            return busquedas;
        }

 // ---------------------------------------------------------------------------------------
 //                             A B M   P A R A D A
 //----------------------------------------------------------------------------------------

        public ActionResult ABMParada()
        {
            List<ParadaDeColectivo> paradas;
            using (var db = new BuscAR())
            {
                paradas = (from p in db.puntosInteres.OfType<ParadaDeColectivo>()
                           orderby p.nombreDePOI
                             select p).ToList();
            }

            return View(paradas);
        }


        public ActionResult CreateParada()
        {
            return View();
        }

        // POST: Default/Create
        [HttpPost]
        public ActionResult CreateParada(FormCollection collection)
        {
            try
            {

                DbGeography coordenada = DbGeography.FromText("POINT(" + collection["coordenada.Latitude"] + " " + collection["coordenada.Longitude"] + ")");
                List<string> palabrasClave = collection["palabrasClave"].Split(new char[] { ',' }).ToList();

 

                ParadaDeColectivo parada = new ParadaDeColectivo(coordenada, collection["calle"], Convert.ToInt32(collection["numeroAltura"]),
                    0, 0,Convert.ToInt32(collection["codigoPostal"]),collection["localidad"], collection["barrio"], collection["provincia"], 
                    collection["pais"], collection["entreCalles"],palabrasClave, collection["nombreDePOI"], "7 remal Samoré");

                parada.agregarParada(parada);

                return RedirectToAction("ABMParada");
            }
            catch
            {
                return View();
            }
        }
        
        public ActionResult DeleteParada(int id)
        {
            ParadaDeColectivo parada;
            using (var db = new BuscAR())
            {
                parada = (ParadaDeColectivo) db.puntosInteres.Where(p => p.id == id).Single();
            }
                return View(parada);
        }

        // POST: Default/Create
        [HttpPost]
        public ActionResult DeleteParada(int id, FormCollection collection)
        {
            try
            {
                ParadaDeColectivo parada;
                using (var db = new BuscAR())
                {
                    parada = (ParadaDeColectivo) db.puntosInteres.Where(p => p.id == id).Single();
                }

                parada.eliminarParada(id);

                return RedirectToAction("ABMParada");
            }
            catch
            {
                return View();
            }
        }



        public ActionResult EditParada(int id)
        {
            ParadaDeColectivo parada;
            using (var db = new BuscAR())
            {
                parada = (ParadaDeColectivo) db.puntosInteres.Where(p => p.id == id).Single();
            }
            return View(parada);
        }

        // POST: Default/Edit
        [HttpPost]
        public ActionResult EditParada(FormCollection collection)
        {
            try
            {
                ParadaDeColectivo parada;
                using (var db = new BuscAR())
                {
                    int id = Convert.ToInt16(collection["id"]);
                    parada = (ParadaDeColectivo) db.puntosInteres.Where(p => p.id == id).Single();

                    DbGeography coordenada = DbGeography.FromText("POINT(" + collection["coordenada.Latitude"] + " " + collection["coordenada.Longitude"] + ")");
                    List<string> palabrasClave = collection["palabrasClave"].Split(new char[] { ',' }).ToList();


                    ParadaDeColectivo paradaActalizada = new ParadaDeColectivo();

                    paradaActalizada.calle = collection["calle"];
                    paradaActalizada.numeroAltura = Convert.ToInt32(collection["numeroAltura"]);
                    paradaActalizada.codigoPostal = Convert.ToInt32(collection["codigoPostal"]);
                    paradaActalizada.localidad = collection["localidad"];
                    paradaActalizada.barrio = collection["barrio"];
                    paradaActalizada.calle = collection["calle"];
                    paradaActalizada.provincia = collection["provincia"];
                    paradaActalizada.pais = collection["pais"];
                    paradaActalizada.entreCalles = collection["entreCalles"];
                    paradaActalizada.palabrasClave = palabrasClave;
                    paradaActalizada.calle = collection["nombreDePOI"];

                   
                    parada.actualizar(paradaActalizada);
                    db.SaveChanges();
                }
              
                return RedirectToAction("ABMParada");
            }
            catch
            {
                return View();
            }
        }



        // ---------------------------------------------------------------------------------------
        //                             A B M   B A N C O
        //----------------------------------------------------------------------------------------

        public ActionResult ABMBanco()
        {
            List<Banco> bancos;
            using (var db = new BuscAR())
            {
                bancos = (from p in db.puntosInteres.OfType<Banco>()
                           orderby p.nombreDePOI
                           select p).ToList();
            }

            return View(bancos);
        }


        public ActionResult CreateBanco()
        {
            return View();
        }

        // POST: Default/Create
          [HttpPost]
          public ActionResult CreateBanco(FormCollection collection)
          {
              try
              {


                DbGeography coordenada = DbGeography.FromText("POINT(" + collection["coordenada.Latitude"] + " " + collection["coordenada.Longitude"] + ")");
                List<string> palabrasClave = collection["palabrasClave"].Split(new char[] { ',' }).ToList();
                List<HorarioAbierto> horariosAbierto = new List<HorarioAbierto>();

    //         HorarioAbierto horarios = new HorarioAbierto(DayOfWeek.Monday, Convert.ToInt32(collection["abreLunes"]), Convert.ToInt32(collection["cierraLunes"]));
    //         horariosAbierto.Add(horarios);

                List<HorarioAbierto> horariosFeriado = new List<HorarioAbierto>();

                List<Servicio> servicios = new List<Servicio>();


                Banco banco = new Banco(coordenada, collection["calle"], Convert.ToInt32(collection["numeroAltura"]),
                      Convert.ToInt32(collection["piso"]), Convert.ToInt32(collection["unidad"]), Convert.ToInt32(collection["codigoPostal"]),
                      collection["localidad"], collection["barrio"], collection["provincia"], collection["pais"], collection["entreCalles"],
                      palabrasClave, collection["nombreDePOI"],  horariosAbierto, horariosFeriado, servicios);

                banco.agregarBanco(banco);

                  return RedirectToAction("ABMBanco");
              }
              catch
              {
                  return View();
              }
          }

        public ActionResult DeleteBanco(int id)
        {
            Banco banco;
            using (var db = new BuscAR())
            {
                banco = (Banco) db.puntosInteres.Where(p => p.id == id).Single();
            }
            return View(banco);
        }

        // POST: Default/Create
        [HttpPost]
        public ActionResult DeleteBanco(int id, FormCollection collection)
        {
            try
            {
                Banco banco;
                using (var db = new BuscAR())
                {
                    banco = (Banco) db.puntosInteres.Where(p => p.id == id).Single();
                }

                banco.eliminarBanco(id);

                return RedirectToAction("ABMBanco");
            }
            catch
            {
                return View();
            }
        }



        public ActionResult EditBanco(int id)
        {
            Banco banco;
            using (var db = new BuscAR())
            {
                banco = (Banco) db.puntosInteres.Where(p => p.id == id).Single();
            }
            return View(banco);
        }

        // POST: Default/Edit
        [HttpPost]
        public ActionResult EditBanco(FormCollection collection)
        {
            try
            {
                Banco banco;
                using (var db = new BuscAR())
                {
                    int id = Convert.ToInt16(collection["id"]);
                    banco = (Banco) db.puntosInteres.Where(p => p.id == id).Single();


                    DbGeography coordenada = DbGeography.FromText("POINT(" + collection["coordenada.Latitude"] + " " + collection["coordenada.Longitude"] + ")");
                    List<string> palabrasClave = collection["palabrasClave"].Split(new char[] { ',' }).ToList();
                    List<HorarioAbierto> horariosAbierto = new List<HorarioAbierto>();

                    //         HorarioAbierto horarios = new HorarioAbierto(DayOfWeek.Monday, Convert.ToInt32(collection["abreLunes"]), Convert.ToInt32(collection["cierraLunes"]));
                    //         horariosAbierto.Add(horarios);

                    List<HorarioAbierto> horariosFeriado = new List<HorarioAbierto>();

                    List<Servicio> servicios = new List<Servicio>();

                    banco.actualizar(collection["calle"], Convert.ToInt32(collection["numeroAltura"]),
                      Convert.ToInt32(collection["piso"]), Convert.ToInt32(collection["unidad"]), Convert.ToInt32(collection["codigoPostal"]),
                      collection["localidad"], collection["barrio"], collection["provincia"], collection["pais"], collection["entreCalles"],
                      collection["nombreDePOI"], palabrasClave, horariosAbierto, horariosFeriado, servicios);


                    db.SaveChanges();
                }

                return RedirectToAction("ABMBanco");
            }
            catch
            {
                return View();
            }
        }



        // ---------------------------------------------------------------------------------------
        //                             A B M   C G P
        //----------------------------------------------------------------------------------------

        public ActionResult ABMCGP()
        {
            List<CGP> cgp;
            using (var db = new BuscAR())
            {
                cgp = (from p in db.puntosInteres.OfType<CGP>()
                       orderby p.nombreDePOI
                          select p).ToList();
            }

            return View(cgp);
        }


        public ActionResult CreateCGP()
        {
            return View();
        }

        // POST: Default/Create
        [HttpPost]
        public ActionResult CreateCGP(FormCollection collection)
        {
            try
            {
                
                DbGeography coordenada = DbGeography.FromText("POINT(" + collection["coordenada.Latitude"] + " " + collection["coordenada.Longitude"] + ")");
                List<string> palabrasClave = collection["palabrasClave"].Split(new char[] { ',' }).ToList();
                List<HorarioAbierto> horariosAbierto = new List<HorarioAbierto>();
               
                //HorarioAbierto horarios = new HorarioAbierto(DayOfWeek.Monday, Convert.ToInt32(collection["abreLunes"]), Convert.ToInt32(collection["cierraLunes"]));
                //horariosAbierto.Add(horarios);

                List<HorarioAbierto> horariosFeriado = new List<HorarioAbierto>();
                List<Servicio> servicios = new List<Servicio>();


                /* convert list to string -- 
                 * var result = string.Join(",", list.ToArray()); */

                CGP cgp = new CGP(coordenada, collection["calle"], Convert.ToInt32(collection["numeroAltura"]),
                      Convert.ToInt32(collection["piso"]), Convert.ToInt32(collection["unidad"]), Convert.ToInt32(collection["codigoPostal"]),
                      collection["localidad"], collection["barrio"], collection["provincia"], collection["pais"], collection["entreCalles"],
                      palabrasClave, collection["nombreDePOI"], Convert.ToInt32(collection["numeroDeComuna"]),
                          servicios, Convert.ToInt32(collection["zonaDelimitadaPorLaComuna"]),horariosAbierto, horariosFeriado);

                cgp.agregarCGP(cgp);

                return RedirectToAction("ABMCGP");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult DeleteCGP(int id)
        {
            CGP cgp;
            using (var db = new BuscAR())
            {
                cgp = (CGP) db.puntosInteres.Where(p => p.id == id).Single();
            }
            return View(cgp);
        }

        // POST: Default/Create
        [HttpPost]
        public ActionResult DeleteCGP(int id, FormCollection collection)
        {
            try
            {
                CGP cgp;
                using (var db = new BuscAR())
                {
                    cgp = (CGP) db.puntosInteres.Where(p => p.id == id).Single();
                }

                cgp.eliminarCGP(id);

                return RedirectToAction("ABMCGP");
            }
            catch
            {
                return View();
            }
        }



        public ActionResult EditCGP(int id)
        {
            CGP cgp;
            using (var db = new BuscAR())
            {
                cgp = (CGP) db.puntosInteres.Where(p => p.id == id).Single();
            }
            return View(cgp);
        }

        // POST: Default/Edit
        [HttpPost]
        public ActionResult EditCGP(FormCollection collection)
        {
            try
            {
                CGP cgp;
                using (var db = new BuscAR())
                {
                    int id = Convert.ToInt16(collection["id"]);
                    cgp = (CGP) db.puntosInteres.Where(p => p.id == id).Single();


                    DbGeography coordenada = DbGeography.FromText("POINT(" + collection["coordenada.Latitude"] + " " + collection["coordenada.Longitude"] + ")");
                    List<string> palabrasClave = collection["palabrasClave"].Split(new char[] { ',' }).ToList();
                    List<HorarioAbierto> horariosAbierto = new List<HorarioAbierto>();

                    //         HorarioAbierto horarios = new HorarioAbierto(DayOfWeek.Monday, Convert.ToInt32(collection["abreLunes"]), Convert.ToInt32(collection["cierraLunes"]));
                    //         horariosAbierto.Add(horarios);

                    List<HorarioAbierto> horariosFeriado = new List<HorarioAbierto>();

                    List<Servicio> servicios = new List<Servicio>();



                    cgp.actualizar(collection["calle"], Convert.ToInt32(collection["numeroAltura"]),
                      Convert.ToInt32(collection["piso"]), Convert.ToInt32(collection["unidad"]), Convert.ToInt32(collection["codigoPostal"]),
                      collection["localidad"], collection["barrio"], collection["provincia"], collection["pais"], collection["entreCalles"],
                      palabrasClave, collection["nombreDePOI"], Convert.ToInt32(collection["numeroDeComuna"]),
                          servicios, Convert.ToInt32(collection["zonaDelimitadaPorLaComuna"]), horariosAbierto, horariosFeriado);


                    db.SaveChanges();
                }

                return RedirectToAction("ABMCGP");
            }
            catch
            {
                return View();
            }
        }












        // ---------------------------------------------------------------------------------------
        //                             A B M   L O C A L 
        //----------------------------------------------------------------------------------------
        public ActionResult CreateLocal()
        {
            return View();
        }

        // POST: Default/Create
        [HttpPost]
        public ActionResult CreateLocal(FormCollection collection)
        {
            try
            {

                DbGeography coordenada = DbGeography.FromText("POINT(" + collection["coordenada.Latitude"] + " " + collection["coordenada.Longitude"] + ")");
                List<string> palabrasClave = collection["palabrasClave"].Split(new char[] { ',' }).ToList();

                List<HorarioAbierto> horariosAbierto = new List<HorarioAbierto>();
                horariosAbierto.Add(new HorarioAbierto(DayOfWeek.Monday, Convert.ToInt32(collection["abreLunes"]), Convert.ToInt32(collection["cierraLunes"])));

                List<HorarioAbierto> horariosFeriado = new List<HorarioAbierto>();

                Rubro rubro = new Rubro();



                LocalComercial localC = new LocalComercial(coordenada, collection["calle"], Convert.ToInt32(collection["numeroAltura"]),
                      Convert.ToInt32(collection["piso"]), Convert.ToInt32(collection["unidad"]), Convert.ToInt32(collection["codigoPostal"]),
                      collection["localidad"], collection["barrio"], collection["provincia"], collection["pais"], collection["entreCalles"], palabrasClave,
                      collection["nombreDePOI"], horariosAbierto, horariosFeriado,rubro, "nombreFantasia");
                /* convert list to string -- 
                 * var result = string.Join(",", list.ToArray()); */


                ////collection["horarioAbierto"], collection["horarioFeriado"], collection["servicios"], collection["zonaDelimitadaPorLaComuna"]
                localC.agregarLocComercial(localC);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


    }
}