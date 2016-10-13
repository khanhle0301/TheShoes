(function (app) {
    app.controller('rootController', rootController);

    rootController.$inject = ['$state', 'authData', 'loginService', '$scope', 'authenticationService'];

    function rootController($state, authData, loginService, $scope, authenticationService) {
        $scope.collapseSlidebar = function () {
            $scope.isCollapsed = $scope.isCollapsed != '' ? '' : "sidebar-collapse";
        }
        $scope.logOut = function () {
            loginService.logOut();
            $state.go('login');
        }
        $scope.authentication = authData.authenticationData;

        //authenticationService.validateRequest();
       
    }
})(angular.module('myshop'));