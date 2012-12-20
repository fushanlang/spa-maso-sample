﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SpaMasoSample.Models;

namespace SpaMasoSample.Controllers
{
    public class PostsController : ApiController
    {
        private readonly BlogContext _db = new BlogContext();

        // GET api/Posts
        public IEnumerable<Post> GetPosts()
        {
            return _db.Posts.AsEnumerable();
        }

        // GET api/Posts/5
        public Post GetPost(int id)
        {
            var post = _db.Posts.Find(id);
            if (post == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return post;
        }

        // PUT api/Posts/5
        public HttpResponseMessage PutPost(int id, Post post)
        {
            if (!ModelState.IsValid || id != post.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else
            {
                _db.Entry(post).State = EntityState.Modified;

                try
                {
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        // POST api/Posts
        public HttpResponseMessage PostPost(Post post)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else
            {
                _db.Posts.Add(post);
                _db.SaveChanges();

                var response = Request.CreateResponse(HttpStatusCode.Created, post);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new {id = post.Id}));
                return response;
            }
        }

        // DELETE api/Posts/5
        public HttpResponseMessage DeletePost(int id)
        {
            var post = _db.Posts.Find(id);
            if (post == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            _db.Posts.Remove(post);

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, post);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}