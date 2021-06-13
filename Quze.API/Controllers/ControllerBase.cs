using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Quze.DAL.Stores;
using Quze.Infrastruture.Extensions;
using Quze.Models;
using Quze.Models.Entities;
using Quze.Models.Models.ViewModels;


namespace Quze.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ControllerBase<TEntity, TEntityVM, TStore> : Controller
        where TEntity : EntityBase
        where TEntityVM : BaseVM
    where TStore : StoreBase<TEntity>
    {
        protected readonly TStore store;
        protected readonly IMapper mapper;


        public ControllerBase(IMapper mapper, TStore store)
        {
            this.store = store;
            this.mapper = mapper;
        }

        // GET
        [HttpGet]
        public async Task<Response<TEntityVM>> GetAsync()
        {
            var entities = await store.ToListAsync();
            var entitiesVM = mapper.Map<List<TEntityVM>>(entities);
            var response = new Response<TEntityVM> { Entities = entitiesVM };
            return response;
        }

        // GET
        [HttpGet("{id}")]
        public async Task<Response<TEntityVM>> GetByIdAsync(int id)
        {
            var entity = await store.GetByIdAsync(id);
            var entityVM = mapper.Map<TEntityVM>(entity);
            var response = new Response<TEntityVM> { Entity = entityVM };
            return response;
        }

        [HttpPost]
        public async Task<Response<TEntityVM>> PostAsync(Request<TEntityVM> request)
        {
            var response = new Response<TEntityVM>();

            List<TEntity> entities = new List<TEntity>();
            try
            {


                if (request.Entities.IsNotNullOrEmpty())
                {
                    entities = mapper.Map<List<TEntity>>(request.Entities);
                }
                else if (request.Entity.IsNotNull())
                {
                    var entity = mapper.Map<TEntity>(request.Entity);
                    entities.Add(entity);
                }

                var recordsAffected = await store.CreateAsync(entities);
                var entitiesVm = mapper.Map<List<TEntityVM>>(entities);

                if (request.Entities.IsNotNullOrEmpty())
                {
                    response.Entities = entitiesVm;
                }
                else
                {
                    response.Entity = entitiesVm[0];
                }
            }
            catch (ApplicationException ex)
            {

            }
            return response;
        }

        [HttpPut]
        public async Task<Response<TEntityVM>> PutAsync(Request<TEntityVM> request)
        {
            var entity = mapper.Map<TEntity>(request.Entity);
            await store.SaveAsync(entity);
            var entityVm = mapper.Map<TEntityVM>(entity);
            var response = new Response<TEntityVM> { Entity = entityVm };
            return response;
        }

        [HttpDelete]
        public async Task<Response<TEntityVM>> DeleteAsync(Request<TEntityVM> request)
        {
            TEntity entity;
            if (request.Entity.IsNotNull())
            {
                entity = mapper.Map<TEntity>(request.Entity);
                await store.DeleteAsync(entity);
            }
            else if (request.EntityId.IsNotNull())
            {
                await store.DeleteAsync(request.EntityId.Value);
            }
            var response = new Response<TEntityVM>();
            return response;
        }

    }
}
