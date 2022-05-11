#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grad_Project.Data;
using Grad_Project.Models;

namespace Grad_Project.Controllers
{
    public class paymentController : Controller
    {
        private readonly ProjectItiContext _context;

        public paymentController(ProjectItiContext context)
        {
            _context = context;
        }

        // GET: payment
        public async Task<IActionResult> Index(string id,decimal? ttlx)
        {
            var payX = _context.payments.Include(p => p.user)
                .FirstOrDefault(o => o.userID == id);
                
            if (payX == null)
            {
                return Content("this user has no payment method");
            }
            else
            {
                payX.user.totalAmount = ttlx;
                _context.SaveChanges();
            }

            var projectItiContext = _context.payments.Include(c => c.user);
            return View(await projectItiContext.ToListAsync());

        }

        // GET: payment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.payments
                .Include(p => p.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: payment/Create
        public IActionResult Create()
        {
            ViewData["cartID"] = new SelectList(_context.carts, "id", "id");
            return View();
        }

        // POST: payment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,cartID,creditCard")] payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["cartID"] = new SelectList(_context.carts, "id", "id", payment.userID);
            return View(payment);
        }

        // GET: payment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["cartID"] = new SelectList(_context.carts, "id", "id", payment.userID);
            return View(payment);
        }

        // POST: payment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,cartID,creditCard")] payment payment)
        {
            if (id != payment.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!paymentExists(payment.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["cartID"] = new SelectList(_context.carts, "id", "id", payment.userID);
            return View(payment);
        }

        // GET: payment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.payments
                .Include(p => p.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.payments.FindAsync(id);
            _context.payments.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool paymentExists(int id)
        {
            return _context.payments.Any(e => e.id == id);
        }
    }
}
