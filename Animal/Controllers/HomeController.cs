using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Animal.Models;
using Animal.Models.Repository;
using WebGrease.Css.Extensions;
using Animal = Animal.Models.Animal;

namespace Animal.Controllers
{
    public class HomeController : Controller
    {
        private IDataContext dc;

        public HomeController(IDataContext dataContext)
        {
            dc = dataContext;
        }
        
        public ActionResult Index()
        {
            if (ViewBag.Animal == null)
                ViewBag.Animal = dc.Animals.GetAll();
            ViewBag.AnimalType = dc.AnimalType.GetAll();
            ViewBag.FellColor = dc.FellColor.GetAll();
            ViewBag.Region = dc.Region.GetAll();
            return View();
        }

        public JsonResult FillLocation(int regionId)
        {
            var locations = dc.Location.GetAll().Where(l=>l.Region.Id == regionId);
            return Json(locations, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddAnimal()
        {
            ViewBag.AnimalType = dc.AnimalType.GetAll();
            ViewBag.FellColor = dc.FellColor.GetAll();
            ViewBag.Region = dc.Region.GetAll();
            return View("AddAnimal");
        }

        [HttpGet]
        public ActionResult EditAnimal(int id)
        {

            try
            {
              ViewBag.AnimalType = dc.AnimalType.GetAll();;
               ViewBag.FellColor = dc.FellColor.GetAll();
               ViewBag.Region = dc.Location.GetAll();
               ViewBag.Location = dc.Region.GetAll();

                return View("EditAnimal");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public ActionResult DeleteAnimal(int id)
        {
            try
            {
                dc.Animals.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult AddAnimal(string animalName, int typeId, int fellColorId, int regionId, int locationId)
        {
            try
            {
                var animal = new Models.Animal( animalName, dc.AnimalType.GetbyId(typeId), dc.FellColor.GetbyId(fellColorId)
                                              , dc.Location.GetbyId(locationId));
                dc.Animals.Add(animal);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public ActionResult DetailAnimal(int id)
        {
            try
            {
                ViewBag.Animal = dc.Animals.GetbyId(id);
                return View("DetailAnimalInfo"); 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult FindAnimals(int? typeId, int? fellColorId, int[] regionList)
        {
            try
            {
                List<Region> regions = new List<Region>();
                regionList.ForEach(r => regions.Add(dc.Region.GetbyId(r)));
                ViewBag.Animal = dc.Animals.FindAnimals(typeId, fellColorId, regions);
                return PartialView("Animal");
                //return View("Index");
                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
       
    }
}