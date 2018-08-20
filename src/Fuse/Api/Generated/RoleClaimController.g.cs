
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
    [Route("api/RoleClaim")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class RoleClaimController
        : BaseApiController<Fuse.Domain.Models.RoleClaim, RoleClaimDtoGen, Fuse.Domain.Models.ApplicationDbContext>
    {
        public RoleClaimController(Fuse.Domain.Models.ApplicationDbContext db) : base(db)
        {
            GeneratedForClassViewModel = ReflectionRepository.Global.GetClassViewModel<Fuse.Domain.Models.RoleClaim>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<RoleClaimDtoGen>> Get(
            int id,
            DataSourceParameters parameters,
            IDataSource<Fuse.Domain.Models.RoleClaim> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<RoleClaimDtoGen>> List(
            ListParameters parameters,
            IDataSource<Fuse.Domain.Models.RoleClaim> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            FilterParameters parameters,
            IDataSource<Fuse.Domain.Models.RoleClaim> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Authorize]
        public virtual Task<ItemResult<RoleClaimDtoGen>> Save(
            RoleClaimDtoGen dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<Fuse.Domain.Models.RoleClaim> dataSource,
            IBehaviors<Fuse.Domain.Models.RoleClaim> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("delete/{id}")]
        [Authorize]
        public virtual Task<ItemResult<RoleClaimDtoGen>> Delete(
            int id,
            IBehaviors<Fuse.Domain.Models.RoleClaim> behaviors,
            IDataSource<Fuse.Domain.Models.RoleClaim> dataSource)
            => DeleteImplementation(id, new DataSourceParameters(), dataSource, behaviors);

        /// <summary>
        /// Downloads CSV of RoleClaimDtoGen
        /// </summary>
        [HttpGet("csvDownload")]
        [Authorize]
        public virtual Task<FileResult> CsvDownload(
            ListParameters parameters,
            IDataSource<Fuse.Domain.Models.RoleClaim> dataSource)
            => CsvDownloadImplementation(parameters, dataSource);

        /// <summary>
        /// Returns CSV text of RoleClaimDtoGen
        /// </summary>
        [HttpGet("csvText")]
        [Authorize]
        public virtual Task<string> CsvText(
            ListParameters parameters,
            IDataSource<Fuse.Domain.Models.RoleClaim> dataSource)
            => CsvTextImplementation(parameters, dataSource);

        /// <summary>
        /// Saves CSV data as an uploaded file
        /// </summary>
        [HttpPost("csvUpload")]
        [Authorize]
        public virtual Task<IEnumerable<ItemResult>> CsvUpload(
            IFormFile file,
            IDataSource<Fuse.Domain.Models.RoleClaim> dataSource,
            IBehaviors<Fuse.Domain.Models.RoleClaim> behaviors,
            bool hasHeader = true)
            => CsvUploadImplementation(file, dataSource, behaviors, hasHeader);

        /// <summary>
        /// Saves CSV data as a posted string
        /// </summary>
        [HttpPost("csvSave")]
        [Authorize]
        public virtual Task<IEnumerable<ItemResult>> CsvSave(
            string csv,
            IDataSource<Fuse.Domain.Models.RoleClaim> dataSource,
            IBehaviors<Fuse.Domain.Models.RoleClaim> behaviors,
            bool hasHeader = true)
            => CsvSaveImplementation(csv, dataSource, behaviors, hasHeader);

        // Methods from data class exposed through API Controller.
    }
}
