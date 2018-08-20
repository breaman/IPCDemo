
/// <reference path="../coalesce.dependencies.d.ts" />

// Generated by IntelliTect.Coalesce

module ListViewModels {
    
    export namespace RoleClaimDataSources {
        export class Default extends Coalesce.DataSource<ViewModels.RoleClaim> { }
    }
    
    export class RoleClaimList extends Coalesce.BaseListViewModel<ViewModels.RoleClaim> {
        public readonly modelName: string = "RoleClaim";
        public readonly apiController: string = "/RoleClaim";
        public modelKeyName: string = "id";
        public itemClass: new () => ViewModels.RoleClaim = ViewModels.RoleClaim;
        
        public filter: {
            roleId?: string;
            claimType?: string;
            claimValue?: string;
            id?: string;
        } | null = null;
        
        /** The namespace containing all possible values of this.dataSource. */
        public dataSources: typeof RoleClaimDataSources = RoleClaimDataSources;
        
        /** The data source on the server to use when retrieving objects. Valid values are in this.dataSources. */
        public dataSource: Coalesce.DataSource<ViewModels.RoleClaim> = new this.dataSources.Default();
        
        /** Configuration for all instances of RoleClaimList. Can be overidden on each instance via instance.coalesceConfig. */
        public static coalesceConfig = new Coalesce.ListViewModelConfiguration<RoleClaimList, ViewModels.RoleClaim>(Coalesce.GlobalConfiguration.listViewModel);
        
        /** Configuration for this RoleClaimList instance. */
        public coalesceConfig: Coalesce.ListViewModelConfiguration<RoleClaimList, ViewModels.RoleClaim>
            = new Coalesce.ListViewModelConfiguration<RoleClaimList, ViewModels.RoleClaim>(RoleClaimList.coalesceConfig);
        
        
        protected createItem = (newItem?: any, parent?: any) => new ViewModels.RoleClaim(newItem, parent);
        
        constructor() {
            super();
        }
    }
}
