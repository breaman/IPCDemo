module CsrUserManagement {
    class CsrUserManagementViewModel {
        allUsers = new ListViewModels.UserList();

        constructor() {
            this.allUsers.load();
        }

        confirmEmail(user: ViewModels.User) {
            user.emailConfirmed(true);
            user.save();
        }

        unlockAccount(user: ViewModels.User) {
            user.accessFailedCount(0);
            user.lockoutEnd(null);

            user.save();
        }
    }

    $(function () {
        ko.applyBindings(new CsrUserManagementViewModel());
    })
}