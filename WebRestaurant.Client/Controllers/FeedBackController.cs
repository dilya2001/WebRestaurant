﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebRestaurant.Adapter.Services;
using WebRestaurant.App.Interactors;
using WebRestaurant.Entity.Entity;
using WebRestaurant.Shared.Dtos;

namespace WebRestaurant.Client.Controllers
{
	[Authorize(Roles = "admin")]
	public class FeedBackController : Controller
    {
		private readonly FeedBackInteractor interactor;

        public FeedBackController(FeedBackInteractor interactor)
        {
			this.interactor = interactor;
        }

        // GET: FeedBack
        public async Task<IActionResult> Index()
        {
			var response = await interactor.GetAll();
			return View(response.Value);
		}

        // GET: FeedBack/Details/5
        public async Task<IActionResult> Details(int id)
        {
			var response = await interactor.GetById(id);
			return response.IsSuccess ? View(response.Value) : NotFound();
		}

        // GET: FeedBack/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FeedBack/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Title,Context,CreateTime")] FeedBackDto FeedBackDto)
        {
			
			if (ModelState.IsValid)
			{
				var response = await interactor.Create(FeedBackDto);
				if (response.IsSuccess)
				{
					return RedirectToAction(nameof(Index));
				}
			}
			return View(FeedBackDto);
		}

        // GET: FeedBack/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
			var response = await interactor.GetById(id);
			if (response.IsSuccess)
			{
				return View(response.Value);
			}
			return NotFound();
		}

        // POST: FeedBack/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Title,Context,CreateTime")] FeedBackDto FeedBackDto)
        {
			if (id != FeedBackDto.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				var response = await interactor.Update(FeedBackDto);
				if (response.IsSuccess)
				{
					return RedirectToAction(nameof(Index));
				}
			}
			return View(FeedBackDto);
		}

        // GET: FeedBack/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
			var response = await interactor.GetById(id);
			return response.IsSuccess ? View(response.Value) : NotFound();
		}

        // POST: FeedBack/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			await interactor.Delete(id);
			return RedirectToAction(nameof(Index));
		}
    }
}
