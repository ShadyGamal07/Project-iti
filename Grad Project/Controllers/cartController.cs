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
using Microsoft.AspNetCore.Authorization;

namespace Grad_Project.Controllers
{
    public class cartController : Controller
    {
        private readonly ProjectItiContext _context;

        public cartController(ProjectItiContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: cart
        public async Task<IActionResult> Index(int? id)
        {
            if (id != null)
            {
                cart c1 = _context.carts.Include(c => c.product).FirstOrDefault(ob => ob.productID == id);

                if (c1 != null)
                {
                    //product px = c1.product;
                    c1.Qty += 1;
                    c1.ttlPrice = (c1.product.price) * (c1.Qty);
                }
                else
                {
                    c1 = new cart();
                    product p1 = _context.products.Find(id);
                    c1.productID = p1.id;
                    c1.product = p1;
                    c1.Qty = 1;
                    c1.ttlPrice = (c1.product.price) * (c1.Qty);
                    _context.Add(c1);
                }
                
                _context.SaveChanges();
            }
            var projectItiContext = _context.carts.Include(c => c.product);
            return View(await projectItiContext.ToListAsync());
        }

        // GET: cart/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.carts
                .Include(c => c.product)
                .FirstOrDefaultAsync(m => m.id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: cart/Create
        public IActionResult Create()
        {
            ViewData["productID"] = new SelectList(_context.products, "id", "id");
            return View();
        }

        // POST: cart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,userID,productID,Qty")] cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["productID"] = new SelectList(_context.products, "id", "id", cart.productID);
            return View(cart);
        }

        // GET: cart/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["productID"] = new SelectList(_context.products, "id", "id", cart.productID);
            return View(cart);
        }

        // POST: cart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,userID,productID,Qty,ttlPrice,product")] cart cart)
        {
            
            if (id != cart.id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cartExists(cart.id))
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
            ViewData["productID"] = new SelectList(_context.products, "id", "id", cart.productID);
            return View(cart);
        }

        // GET: cart/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.carts
                .Include(c => c.product)
                .FirstOrDefaultAsync(m => m.id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.carts.FindAsync(id);
            if (cart.Qty > 1) { cart.Qty--; }
            else {
                _context.carts.Remove(cart);                
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool cartExists(int id)
        {
            return _context.carts.Any(e => e.id == id);
        }
    }
}
