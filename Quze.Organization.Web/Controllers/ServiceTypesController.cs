using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Infrastruture.Extensions;
using Quze.Infrastruture.Security;
using Quze.Models;
using Quze.Models.Entities;
using Quze.Organization.Web.Controllers;
using Quze.Organization.Web.Helpers;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Quze.Organization.Web.ViewModels;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quze.Organization.Web.Controllers
{


    [Route("api/[controller]")]
    public class ServiceTypesController : BaseController
    {
        protected readonly QueueStore queueStore;
        protected readonly ServiceTypeStore serviceTypesStore;
        protected readonly QuzeContext context;
        protected readonly IMapper mapper ;
        IConfiguration Configuration;
        IUserService userService;
        IUserStore<User> userStore;
        private Quze.Models.Response<ServiceTypeVM> response;


        public ServiceTypesController(
            QuzeContext ctx,
            IConfiguration configuration,
            IMapper mapper,
            ServiceTypeStore serviceTypesStore,
            QueueStore queueStore,
            IUserService userService,
           IUserStore<User> userStore
            ) : base(ctx, mapper)
        {
            this.queueStore = queueStore;
            this.serviceTypesStore = serviceTypesStore;
            this.context = ctx;
            this.mapper = mapper;
            this.Configuration = configuration;
            this.userService = userService;
            this.userStore = userStore;
        }


        [HttpGet("[action]")]
        public Quze.Models.Response<ServiceTypeVM> LoadServiceTypeDatailsByOrganizationId()
        {
            response = new Quze.Models.Response<ServiceTypeVM>();

            var organizationId = JwtUser.OrganizationId;
            var serviceTypes = serviceTypesStore.GetSTByOrganization(organizationId ?? 0);
            var serviceTypesVM = mapper.Map<List<ServiceTypeVM>>(serviceTypes);

            response.Entities = serviceTypesVM;
            return response;
        }

        [HttpGet("[action]")]//Good version.
        public Tree<ServiceType> GetSTAsTree()
        {
            var organizationId = JwtUser.OrganizationId;

            var serviceTypes = serviceTypesStore.GetSTByOrganization(organizationId ?? 0);

            TreeNode<ServiceType>[] treeElementsArray = new TreeNode<ServiceType>[serviceTypes.Count];

            int j = 0;
            foreach (var st in serviceTypes)
            {
                treeElementsArray[j] = new TreeNode<ServiceType>(st, st.Id);
                j++;
            }
            if (treeElementsArray == null || treeElementsArray.Length == 0)
                return null;
            var root = treeElementsArray[0];
            TreeNode<ServiceType> parent;
            for (int i = 0; i < treeElementsArray.Length; i++)
            {
                if (treeElementsArray[i].Content.ParentServiceId != null)
                {
                    parent = new TreeNode<ServiceType>(treeElementsArray[i].Content.ParentServiceId ?? 0);
                    var p = Array.BinarySearch(treeElementsArray, parent);
                    treeElementsArray[p].AddChild(treeElementsArray[i]);
                }
            }
            var tree = new Tree<ServiceType>(root);

            string jsonValue = JsonConvert.SerializeObject(tree);

            return tree;

        }



        [HttpGet("[action]")]
        public Tree<ServiceType> GetSTAsLeveledTree()
        {
            var organizationId = JwtUser.OrganizationId;

            var serviceTypes = serviceTypesStore.GetSTByOrganization(organizationId ?? 0);

            var root = new TreeNode<ServiceType>(serviceTypes[0], serviceTypes[0].Id);
            var tree = new Tree<ServiceType>(root);

            var qOfElementsInTheTree = new Queue<TreeNode<ServiceType>>();

            qOfElementsInTheTree.Enqueue(root);

            var currenParent = qOfElementsInTheTree.Dequeue();

            //This code assume that the parentId is smaller then his child Id!
            foreach (var st in serviceTypes)
            {
                if (st.ParentServiceId != null)
                {
                    var node = new TreeNode<ServiceType>(st, st.Id);
                    qOfElementsInTheTree.Enqueue(node);

                    while (st.ParentServiceId != currenParent.Id)
                    {
                        currenParent = qOfElementsInTheTree.Dequeue();
                    }

                    tree.AddTreeNode(node, currenParent);
                }

            }

            string jsonValue = JsonConvert.SerializeObject(tree);

            return tree;

        }

        [HttpGet("[action]")]
        public Quze.Models.Response<ServiceTypeVM> GetSTChildren(int serviceTypeId)
        {
            response = new Quze.Models.Response<ServiceTypeVM>();

            var organizationId = JwtUser.OrganizationId;

            var serviceTypes = serviceTypesStore.GetSTChildren(organizationId ?? 0, serviceTypeId);

            var serviceTypesVM = mapper.Map<List<ServiceTypeVM>>(serviceTypes);

            response.Entities = serviceTypesVM;

            return response;


        }

    }
}


