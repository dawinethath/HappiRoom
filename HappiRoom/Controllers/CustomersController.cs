using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HappiRoom.Models;
using Newtonsoft.Json;

namespace HappiRoom.Controllers
{
    public class CustomersController : ApiController
    {
        private HappiRoomContext db = new HappiRoomContext();
        
        // GET api/Customers        
        public DataSourceResult Get(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
                // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );

            return db.Customers
                //Entity Framework can page only sorted data
                .OrderBy(customer => customer.id)
                .Select(customer => new
                {
                    // Skip the EntityState and EntityKey properties inherited from EF. It would break model binding.
                    customer.id,
                    customer.room_id,
                    customer.number,
                    customer.surname,
                    customer.name,
                    customer.gender,
                    customer.date_of_birth,
                    customer.place_of_birth,
                    customer.national_card_no,
                    customer.balance,
                    customer.deposit,
                    customer.memo,                    
                    customer.registered_date,
                    customer.date_in,
                    customer.date_going_out,
                    customer.date_out,
                    customer.status                   
                })
            .ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }

        // PUT api/Customers/5
        public HttpResponseMessage Put(int id, Customer customer)
        {
            if (ModelState.IsValid && id == customer.id)
            {
                db.Customers.Attach(customer);               
                db.Entry(customer).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Customers
        public HttpResponseMessage Post(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, customer);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = customer.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Customers/5
        public HttpResponseMessage Delete(int id)
        {
            Customer customer = db.Customers.Single(c => c.id == id);
            if (customer == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Customers.Remove(customer);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, customer);
        }
                
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.id == id) > 0;
        }
    }
}