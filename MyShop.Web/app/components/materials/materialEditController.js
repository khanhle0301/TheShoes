(function (app) {
    app.controller('materialEditController', materialEditController);

    materialEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function materialEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.material = {};
        $scope.UpdateMaterial = UpdateMaterial;

        function loadMaterialDetail() {
            apiService.get('api/material/getbyid/' + $stateParams.id, null, function (result) {
                $scope.material = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateMaterial() {
            apiService.put('api/material/update', $scope.material, updateSuccessed, addFailed);
        }
        function updateSuccessed() {
            notificationService.displaySuccess($scope.material.Name + ' đã được cập nhật.');
            $state.go('materials');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }

        loadMaterialDetail();
    }

})(angular.module('myshop.materials'));