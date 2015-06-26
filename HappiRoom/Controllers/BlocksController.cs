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
    public class BlocksController : ApiController
    {
        private HappiRoomContext db = new HappiRoomContext();

        // GET api/Blocks        
        public DataSourceResult Get(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
                // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );           

            return db.Blocks
                //Entity Framework can page only sorted data
                .OrderBy(block => block.id)
                .Select(block => new
                {
                    // Skip the EntityState and EntityKey properties inherited from EF. It would break model binding.
                    block.id,
                    block.name
                })
            .ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }

        // PUT api/Blocks/5
        public HttpResponseMessage Put(int id, Block block)
        {
            if (ModelState.IsValid && id == block.id)
            {
                db.Blocks.Attach(block);
                db.Entry(block).State = EntityState.Modified;

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

        // POST api/Blocks
        public HttpResponseMessage Post(Block block)
        {
            if (ModelState.IsValid)
            {
                db.Blocks.Add(block);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, block);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = block.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Blocks/5
        public HttpResponseMessage Delete(int id)
        {
            Block block = db.Blocks.Single(r => r.id == id);
            if (block == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Blocks.Remove(block);
            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, block);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BlockExists(int id)
        {
            return db.Blocks.Count(e => e.id == id) > 0;
        }
    }
}