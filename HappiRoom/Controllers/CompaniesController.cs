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
    public class CompaniesController : ApiController
    {
        private HappiRoomContext db = new HappiRoomContext();

        // GET api/Companies        
        public DataSourceResult Get(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
                // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );

            return db.Companies
                //Entity Framework can page only sorted data
                .OrderBy(company => company.id)
                .Select(company => new
                {
                    // Skip the EntityState and EntityKey properties inherited from EF. It would break model binding.
                    company.id,
                    company.name,
                    company.term_of_condition
                })
            .ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }

        // PUT api/Companies/5
        public HttpResponseMessage Put(int id, Company company)
        {
            if (ModelState.IsValid && id == company.id)
            {
                db.Companies.Attach(company);
                db.Entry(company).State = EntityState.Modified;

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

        // POST api/Companies
        public HttpResponseMessage Post(Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, company);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = company.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Companies/5
        public HttpResponseMessage Delete(int id)
        {
            Company company = db.Companies.Single(r => r.id == id);
            if (company == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Companies.Remove(company);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, company);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompanyExists(int id)
        {
            return db.Companies.Count(e => e.id == id) > 0;
        }
    }
}