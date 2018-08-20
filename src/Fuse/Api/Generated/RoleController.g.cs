
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
    [Route("api/Role")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class RoleController
        : BaseApiController<Fuse.Domain.Models.Role, RoleDtoGen, Fuse.Domain.Models.ApplicationDbContext>
    {
        public RoleController(Fuse.Domain.Models.ApplicationDbContext db) : base(db)
        {
            GeneratedForClassViewModel = ReflectionRepository.Global.GetClassViewModel<Fuse.Domain.Models.Role>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<RoleDtoGen>> Get(
            int id,
            DataSourceParameters parameters,
            IDataSource<Fuse.Domain.Models.Role> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<RoleDtoGen>> List(
            ListParameters parameters,
            IDataSource<Fuse.Domain.Models.Role> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            FilterParameters parameters,
            IDataSource<Fuse.Domain.Models.Role> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Authorize]
        public virtual Task<ItemResult<RoleDtoGen>> Save(
            RoleDtoGen dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<Fuse.Domain.Models.Role> dataSource,
            IBehaviors<Fuse.Domain.Models.Role> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("delete/{id}")]
        [Authorize]
        public virtual Task<ItemResult<RoleDtoGen>> Delete(
            int id,
            IBehaviors<Fuse.Domain.Models.Role> behaviors,
            IDataSource<Fuse.Domain.Models.Role> dataSource)
            => DeleteImplementation(id, new DataSourceParameters(), dataSource, behaviors);

        /// <summary>
        /// Downloads CSV of RoleDtoGen
        /// </summary>
        [HttpGet("csvDownload")]
        [Authorize]
        public virtual Task<FileResult> CsvDownload(
            ListParameters parameters,
            IDataSource<Fuse.Domain.Models.Role> dataSource)
            => CsvDownloadImplementation(parameters, dataSource);

        /// <summary>
        /// Returns CSV text of RoleDtoGen
        /// </summary>
        [HttpGet("csvText")]
        [Authorize]
        public virtual Task<string> CsvText(
            ListParameters parameters,
            IDataSource<Fuse.Domain.Models.Role> dataSource)
            => CsvTextImplementation(parameters, dataSource);

        /// <summary>
        /// Saves CSV data as an uploaded file
        /// </summary>
        [HttpPost("csvUpload")]
        [Authorize]
        public virtual Task<IEnumerable<ItemResult>> CsvUpload(
            IFormFile file,
            IDataSource<Fuse.Domain.Models.Role> dataSource,
            IBehaviors<Fuse.Domain.Models.Role> behaviors,
            bool hasHeader = true)
            => CsvUploadImplementation(file, dataSource, behaviors, hasHeader);

        /// <summary>
        /// Saves CSV data as a posted string
        /// </summary>
        [HttpPost("csvSave")]
        [Authorize]
        public virtual Task<IEnumerable<ItemResult>> CsvSave(
            string csv,
            IDataSource<Fuse.Domain.Models.Role> dataSource,
            IBehaviors<Fuse.Domain.Models.Role> behaviors,
            bool hasHeader = true)
            => CsvSaveImplementation(csv, dataSource, behaviors, hasHeader);

        // Methods from data class exposed through API Controller.
    }
}
