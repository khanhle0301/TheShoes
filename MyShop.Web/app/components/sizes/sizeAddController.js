(function (app) {
    app.controller('sizeAddController', sizeAddController);

    sizeAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$location', 'commonService'];

    function sizeAddController(apiService, $scope, notificationService, $state, $location, commonService) {
        $scope.size = {
        }

        $scope.AddSize = AddSize;

        function AddSize() {
            apiService.post('api/size/create', $scope.size, addSuccessed, addFailed);
        }
        function addSuccessed() {
            notificationService.displaySuccess($scope.size.Name + ' đã được thêm mới.');
            $state.go('sizes');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }
    }

})(angular.module('myshop.sizes'));