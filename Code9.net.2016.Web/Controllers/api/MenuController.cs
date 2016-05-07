using Code9.net._2016.data.Entities;
using Code9.net._2016.Repositories;
using Code9.net._2016.Web.Models.api;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Code9.net._2016.Web.Controllers.api
{
    public class MenuController : ApiController
    {

        private IRestourantRepository repository { get; set; }

        public MenuController(IRestourantRepository repo)
        {
            repository = repo;
        }

        // GET api/<controller>?kind=<kind>
        public IEnumerable<Menu> Get([EnumDataType(typeof(MenuItemKind))] MenuItemKind kind)
        {
            if (!Enum.IsDefined(typeof(MenuItemKind), kind))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return repository.GetMenuForItemKind(kind)
                .Select(m => new Menu()
                {
                    Id = m.ID,
                    Name = m.DisplayName,
                    Price = m.Price,
                    Kind = m.Kind,
                    IsActive = m.Active
                });
        }

        // GET api/<controller>
        public IEnumerable<Menu> Get()
        {
            var meals = repository.GetMenuForItemKind(MenuItemKind.MEAL)
                    .Select(m => new Menu()
                    {
                        Id = m.ID,
                        Name = m.DisplayName,
                        Price = m.Price,
                        Kind = m.Kind,
                        IsActive = m.Active
                    });
            var drinks = repository.GetMenuForItemKind(MenuItemKind.DRINK)
                .Select(m => new Menu()
                {
                    Id = m.ID,
                    Name = m.DisplayName,
                    Price = m.Price,
                    Kind = m.Kind,
                    IsActive = m.Active
                });

            return drinks.Concat(meals);
        }

        // GET api/<controller>/5
        public Menu Get(int id)
        {
            var m = repository.GetMenuItemByID(id);
            if (m!=null)
            {
                return new Menu()
                {
                    Name = m.DisplayName,
                    Id = m.ID,
                    Price = m.Price,
                    Kind = m.Kind,
                    IsActive = m.Active
                };
            } else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // POST api/<controller>/
        [HttpPost]
        public IHttpActionResult Post([FromBody]Menu value)
        {
            if (!Enum.IsDefined(typeof(MenuItemKind), value.Kind))
            {
                ModelState.AddModelError("kind", "Invalid value specified");
            }
            if (ModelState.IsValid)
            {
                var menuItem = repository.AddMenuItemToMenu(value.Name, value.Price, value.Kind, EmployeeRole.COOK);
                return Created<Menu>("~/api/menu", new Menu()
                {
                    Id = menuItem.ID,
                    IsActive = menuItem.Active,
                    Kind = menuItem.Kind,
                    Name = menuItem.DisplayName,
                    Price = menuItem.Price
                });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT api/<controller>/5
        [HttpPut]
        public void Put([FromBody]Menu value)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var item = repository.GetMenuItemByID(value.Id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            item.DisplayName = value.Name;
            item.Price = value.Price;
            item.Kind = value.Kind;
            item.Active = value.IsActive;
            repository.UpdateMenuItem(item);
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public void Delete(int id)
        {
            var item = repository.GetMenuItemByID(id);
            if (item==null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            item.Active = false;
            repository.UpdateMenuItem(item);
        }
    }
}