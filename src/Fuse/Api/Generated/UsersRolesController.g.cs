
using Fuse.Models;
using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Api;
using IntelliTect.Coalesce.Api.Controllers;
using IntelliTect.Coalesce.Api.DataSources;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Mapping.IncludeTrees;
using IntelliTect.Coalesce.Models;
using IntelliTect.Coalesce.TypeDefinition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Fuse.Api
{
    [Route("api/UsersRoles")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class UsersRolesController
        : BaseApiController<Fuse.Domain.Models.UsersRoles, UsersRolesDtoGen, Fuse.Domain.Models.ApplicationDbContext>
    {
        public UsersRolesController(Fuse.Domain.Models.ApplicationDbContext db) : base(db)
        {
            GeneratedForClassViewModel = ReflectionRepository.Global.GetClassViewModel<Fuse.Domain.Models.UsersRoles>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<UsersRolesDtoGen>> Get(
            int id,
            DataSourceParameters parameters,
            IDataSource<Fuse.Domain.Models.UsersRoles> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<UsersRolesDtoGen>> List(
            ListParameters parameters,
            IDataSource<Fuse.Domain.Models.UsersRoles> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            FilterParameters parameters,
            IDataSource<Fuse.Domain.Models.UsersRoles> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Authorize]
        public virtual Task<ItemResult<UsersRolesDtoGen>> Save(
            UsersRolesDtoGen dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<Fuse.Domain.Models.UsersRoles> dataSource,
            IBehaviors<Fuse.Domain.Models.UsersRoles> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("delete/{id}")]
        [Authorize]
        public virtual Task<ItemResult<UsersRolesDtoGen>> Delete(
            int id,
            IBehaviors<Fuse.Domain.Models.UsersRoles> behaviors,
            IDataSource<Fuse.Domain.Models.UsersRoles> dataSource)
            => DeleteImplementation(id, new DataSourceParameters(), dataSource, behaviors);

        /// <summary>
        /// Downloads CSV of UsersRolesDtoGen
        /// </summary>
        [HttpGet("csvDownload")]
        [Authorize]
        public virtual Task<FileResult> CsvDownload(
            ListParameters parameters,
            IDataSource<Fuse.Domain.Models.UsersRoles> dataSource)
            => CsvDownloadImplementation(parameters, dataSource);

        /// <summary>
        /// Returns CSV text of UsersRolesDtoGen
        /// </summary>
        [HttpGet("csvText")]
        [Authorize]
        public virtual Task<string> CsvText(
            ListParameters parameters,
            IDataSource<Fuse.Domain.Models.UsersRoles> dataSource)
            => CsvTextImplementation(parameters, dataSource);

        /// <summary>
        /// Saves CSV data as an uploaded file
        /// </summary>
        [HttpPost("csvUpload")]
        [Authorize]
        public virtual Task<IEnumerable<ItemResult>> CsvUpload(
            IFormFile file,
            IDataSource<Fuse.Domain.Models.UsersRoles> dataSource,
            IBehaviors<Fuse.Domain.Models.UsersRoles> behaviors,
            bool hasHeader = true)
            => CsvUploadImplementation(file, dataSource, behaviors, hasHeader);

        /// <summary>
        /// Saves CSV data as a posted string
        /// </summary>
        [HttpPost("csvSave")]
        [Authorize]
        public virtual Task<IEnumerable<ItemResult>> CsvSave(
            string csv,
            IDataSource<Fuse.Domain.Models.UsersRoles> dataSource,
            IBehaviors<Fuse.Domain.Models.UsersRoles> behaviors,
            bool hasHeader = true)
            => CsvSaveImplementation(csv, dataSource, behaviors, hasHeader);

        // Methods from data class exposed through API Controller.
    }
}
