
using DogsApp.Core.Contracts;
using DogsApp.Infrastructure.Data;
using DogsApp.Infrastructure.Data.Domain;
using DogsApp.Models.Breed;
using DogsApp.Models.Dog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

using System.Security.Cryptography.Xml;

namespace DogsApp.Controllers
{
    public class DogController : Controller
    {
        private readonly IDogService _dogService;
        private readonly IBreedService _breedService;

        public DogController(IDogService dogsService, IBreedService breedService)
        {
            this._dogService = dogsService;
            this._breedService = breedService;
        }
       

        // GET: DogController
        public IActionResult Index(string searchStringBreed, string searchStringName)
        {
            List<DogAllViewModel> dogs = _dogService.GetDogs(searchStringBreed, searchStringName).Select(dogFromDb => new DogAllViewModel
            {
                Id = dogFromDb.Id,
                Name = dogFromDb.Name,
                Age = dogFromDb.Age,
                Breed = dogFromDb.Breed,
                Picture = dogFromDb.Picture
            }).ToList();
            return this.View(dogs);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            Dog item = _dogService.GetDogById(id);
            if (item == null)
            {
                return NotFound();
            }
            DogDetailsViewModel dog = new DogDetailsViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                Breed = item.Breed,
                Picture = item.Picture
            };
            return View(dog);
        }

        // GET: DogController/Create
        public ActionResult Create()
        {
            var Dog = new DogCreateViewModel();
            Dog.Breeds = _breedService.GetBreeds()
                .Select(c => new BreedPairViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
            return View(dog);
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] DogCreateViewModel dog)
        {
            if (ModelState.IsValid)
            {
                var created = _dogService.Create(dog.Name, dog.Age, dog.BreedId, dog.Picture);
                if (createdId)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return this.View();
        }

        public IActionResult Success()
        {
            return this.View();

        }


        // GET: DogController/Edit/5
        public ActionResult Edit(int id)
        {
            Dog item  = _dogService.GetDogById(id);
            if (id == null)
            {
                return NotFound();
            }
            DogCreateViewModel dog = new DogCreateViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                BreedId = item.Breed,
                Picture = item.Picture
            };
            return View(dog);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DogEditViewModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                var updated = _dogService.UpdateDog(id, bindingModel.Name, bindingModel.Age, bindingModel.Breed, bindingModel.Picture);
                if (updated)
                {
                    return this.RedirectToAction("Index");
                }
            }
            return View(bindingModel);
        }

        // GET: DogController/Delete/5
        public ActionResult Delete(int id)
        {
            Dog item = _dogService.GetDogById(id);
            if (item == null)
            {
                return NotFound();
            }
            DogCreateViewModel dog = new DogCreateViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                BreedId = item.Breed,
                Picture = item.Picture
            };
            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            var deleted = _dogService.RemoveById(id);

            if (deleted)
            {
                return this.RedirectToAction("Index", "Dog");
            }
            else
            {
                return View();
            }
        }
        
    }
}
