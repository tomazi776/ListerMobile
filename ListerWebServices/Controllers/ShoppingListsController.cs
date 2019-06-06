﻿using ListerMobile.Models;
using ListerWebServices.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace ListerWebServices.Controllers
{
    public class ShoppingListsController : ApiController
    {
        private ShoppingListsContext db = new ShoppingListsContext();

        // GET: api/ShoppingLists
        public IQueryable<ShoppingList> GetShoppingLists()
        {
            var result = db.ShoppingLists;
            return result;
        }


        /// <summary>
        /// RECEIVED SHOPPING LISTS
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// 
        // GET: api/ShoppingLists
        //[HttpGet]
        //[Route("api/ShoppingLists")]
        public IQueryable<ShoppingList> GetUserShoppingLists(string username)
        {
            try
            {
                var matchingUsers = db.ShoppingLists.AsEnumerable().Where(item => item.Users.Split(' ').Any(a => a == username));
                var result = matchingUsers.AsQueryable();
                return result;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.InnerException.Message);
                throw;
            }
        }

        // GET: api/ShoppingLists/5
        [ResponseType(typeof(ShoppingList))]
        public IHttpActionResult GetShoppingList(int id)
        {
            ShoppingList shoppingList = db.ShoppingLists.Find(id);
            if (shoppingList == null)
            {
                return NotFound();
            }

            return Ok(shoppingList);
        }

        // PUT: api/ShoppingLists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutShoppingList(int id, ShoppingList shoppingList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shoppingList.Id)
            {
                return BadRequest();
            }

            db.Entry(shoppingList).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ShoppingLists
        [ResponseType(typeof(ShoppingList))]
        public IHttpActionResult PostShoppingList(ShoppingList shoppingList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ShoppingLists.Add(shoppingList);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = shoppingList.Id }, shoppingList);
        }

        // DELETE: api/ShoppingLists/5
        [ResponseType(typeof(ShoppingList))]
        public IHttpActionResult DeleteShoppingList(int id)
        {
            ShoppingList shoppingList = db.ShoppingLists.Find(id);
            if (shoppingList == null)
            {
                return NotFound();
            }

            db.ShoppingLists.Remove(shoppingList);
            db.SaveChanges();

            return Ok(shoppingList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShoppingListExists(int id)
        {
            return db.ShoppingLists.Count(e => e.Id == id) > 0;
        }

    }
}