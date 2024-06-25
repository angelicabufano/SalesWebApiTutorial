﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebApiTutorial.Data;
using SalesWebApiTutorial.Models;

namespace SalesWebApiTutorial.Controllers {
    // http://localhost:5000/api/customers
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context) {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer() {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5             GETBYPK
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id) {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null) {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5             UPDATE
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer) {
            if (id != customer.Id) {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!CustomerExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers                  ADD
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer) {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5              DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id) {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id) {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}