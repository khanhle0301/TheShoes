(function (app) {
    app.controller('typeAddController', typeAddController);

    typeAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location', 'commonService'];

    function typeAddController(apiService, $scope, notificationService, $state, $location, commonService) {
        $scope.type = {
        }

        $scope.AddType = AddType;

        function AddType() {
            apiService.post('api/type/create', $scope.type, addSuccessed, addFailed);
        }
        function addSuccessed() {
            notificationService.displaySuccess($scope.type.Name + ' đã được thêm mới.');
            $state.go('types');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }
    }

})(angular.module('myshop.types'));