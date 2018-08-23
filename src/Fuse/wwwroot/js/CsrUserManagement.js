var CsrUserManagement;
(function (CsrUserManagement) {
    var CsrUserManagementViewModel = /** @class */ (function () {
        function CsrUserManagementViewModel() {
            this.allUsers = new ListViewModels.UserList();
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

//# sourceMappingURL=CsrUserManagement.js.map
