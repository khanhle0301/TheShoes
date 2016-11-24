(function (app) {
    app.controller('materialAddController', materialAddController);

    materialAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location', 'commonService'];

    function materialAddController(apiService, $scope, notificationService, $state, $location, commonService) {
        $scope.material = {
        }

        $scope.AddMaterial = AddMaterial;

        function AddMaterial() {
            apiService.post('api/material/create', $scope.material, addSuccessed, addFailed);
        }
        function addSuccessed() {
            notificationService.displaySuccess($scope.material.Name + ' đã được thêm mới.');
            $state.go('materials');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }
    }

})(angular.module('myshop.materials'));