(function (app) {
    app.controller('typeEditController', typeEditController);

    typeEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function typeEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.type = {};
        $scope.UpdateType = UpdateType;

        function loadTypeDetail() {
            apiService.get('api/type/getbyid/' + $stateParams.id, null, function (result) {
                $scope.type = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateType() {
            apiService.put('api/type/update', $scope.type, updateSuccessed, addFailed);
        }
        function updateSuccessed() {
            notificationService.displaySuccess($scope.type.Name + ' đã được cập nhật.');
            $state.go('types');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }

        loadTypeDetail();
    }

})(angular.module('myshop.types'));