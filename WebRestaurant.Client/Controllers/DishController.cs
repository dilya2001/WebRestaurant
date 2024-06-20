﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebRestaurant.Adapter.Services;
using WebRestaurant.App.Interactors;
using WebRestaurant.Domain.Entity;
using WebRestaurant.Shared.Dtos;

namespace WebRestaurant.Client.Controllers
{
	[Authorize(Roles = "admin")]
	public class DishController : Controller
    {
        private readonly DishInteractor interactor;
        IWebHostEnvironment _appEnvironment;
        

        public DishController(DishInteractor interactor, IWebHostEnvironment appEnvironment)
        {
            this.interactor = interactor;
            _appEnvironment = appEnvironment;
        }

        // GET: Dish
        public async Task<IActionResult> Index()
        {
            var response = await interactor.GetAll();
            return View(response.Value);
        }

        // GET: Dish/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await interactor.GetById(id);
            return response.IsSuccess ? View(response.Value) : NotFound();
        }

        // GET: Dish/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dish/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Weight,PhotoPath,Description")] DishDto dish, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                //dish.PhotoPath = "/images/items/" + Image.FileName;
                var response = await interactor.Create(dish);
                if (response.IsSuccess)
                {
                    if (Image != null)
                    {
						string path = "/images/items/" + Image.FileName;
						using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await Image.CopyToAsync(fileStream);
                        }
					}
                }
            }

            return View(dish);
        }

        // GET: Dish/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await interactor.GetById(id);
            return response.IsSuccess ? View(response.Value) : NotFound();
        }

        // POST: Dish/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Weight,PhotoPath,Description")] DishDto dish, IFormFile Image)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                dish.PhotoPath = "/images/items/" + Image.FileName;
                var response = await interactor.Update(dish);
                if (response.IsSuccess)
                {
                    if (Image != null)
                    {
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + dish.PhotoPath, FileMode.Create))
                        {
                            await Image.CopyToAsync(fileStream);
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(dish);
        }

        // GET: Dish/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await interactor.GetById(id);
            return response.IsSuccess ? View(response.Value) : NotFound();
        }

        // POST: Dish/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await interactor.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
