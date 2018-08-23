var CsrUserManagement;
(function (CsrUserManagement) {
    var CsrUserManagementViewModel = /** @class */ (function () {
        function CsrUserManagementViewModel() {
            this.allUsers = new ListViewModels.UserList();
            var dataSource = new this.allUsers.dataSources.FetchUsersForApplication();
            dataSource.applicationName(applicationName);
            dataSource.subscribe(this.allUsers);
            this.allUsers.dataSource = dataSource;
            this.allUsers.load();
        }
        CsrUserManagementViewModel.prototype.confirmEmail = function (user) {
            user.emailConfirmed(true);
            user.save();
        };
        CsrUserManagementViewModel.prototype.unlockAccount = function (user) {
            user.accessFailedCount(0);
            user.lockoutEnd(null);
            user.save();
        };
        return CsrUserManagementViewModel;
    }());
    $(function () {
        ko.applyBindings(new CsrUserManagementViewModel());
    });
})(CsrUserManagement || (CsrUserManagement = {}));

//# sourceMappingURL=GisUserManagement.js.map
